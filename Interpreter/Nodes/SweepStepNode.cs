using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Easis.Wfs.EasyFlow.Model;
using Easis.Wfs.EasyFlow.Model.Expressions;

namespace Easis.Wfs.Interpreting.Nodes
{
    class SweepStepNode : StepNode
    {
        public SweepStepNode(StepBlock block, IStepNodeContext context, WfLog log)
            : base(block, context, log)
        {
        }

        public const string STATE_SWEEPING = "state_sweeping";
        public const string STATE_SWEEPING_FINISHED = "state_sweeping_finished";
        public const string ACTION_SWEEP_FINISH = "action_sweep_finish";

        #region sweepers
        public List<StepNode> SweepChildren = new List<StepNode>();


        /// <summary>
        /// Generate decart multiplication of all the lists fetched.
        /// Order of enities in lists is important.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="par">List of param values(set)</param>
        /// <returns>Generates set of values</returns>
        public static IEnumerable<IList<T>> DecartIt<T>(IList<IEnumerable<T>> par) // is public only for testing
        {
            if (par == null) throw new ArgumentNullException("par");
            if (par.Count == 0) throw new ArgumentException("Can't sweep through empty set");

            List<IList<T>> ret = new List<IList<T>>();

            List<IEnumerator<T>> buf = new List<IEnumerator<T>>();

            // fill
            foreach (var v in par)
            {
                buf.Add(v.GetEnumerator());
            }

            int n = par.Count;

            // make first
            var hasEmpty = !buf.Select(enumerator => enumerator.MoveNext()).Aggregate((workingSentence, next) =>
                                                  next && workingSentence);
            if (hasEmpty)
            {
                throw new ArgumentException("Can't sweep through empty set");
            }

            // main loop
            bool isFinished = false;

            while (!isFinished)
            {
                // snapshot
                ret.Add(buf.Select(enumerator => enumerator.Current).ToList());

                // try to change ->
                for (int i = 0; i < n; i++)
                {
                    // get cur enum
                    if (buf[i].MoveNext())
                    {
                        // return <-
                        for (int j = i - 1; j >= 0; j--)
                        {
                            buf[j].Reset(); // -1
                            buf[j].MoveNext(); // 0
                        }
                        // snapshot 
                        break;
                    }
                    else
                    {
                        if (i == n - 1)
                        {
                            // there are no more variants
                            isFinished = true;
                            break;
                        }
                    }
                }
            }

            return ret;
        }
        /// <summary>
        /// Generate 1 to 1 list
        /// Order of enities in lists is important.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="par">List of param values(set)</param>
        /// <returns>Generates set of values</returns>
        public static IEnumerable<IList<T>> ZipIt<T>(IList<IEnumerable<T>> par) // is public only for testing
        {
            if (par == null) throw new ArgumentNullException("par");
            if (par.Count == 0) throw new ArgumentException("Can't sweep through empty set");

            List<IList<T>> ret = new List<IList<T>>();

            List<IEnumerator<T>> buf = new List<IEnumerator<T>>();

            // fill
            foreach (var v in par)
            {
                buf.Add(v.GetEnumerator());
            }

            int n = par.Count;

            // make first
            var hasEmpty = !buf.Select(enumerator => enumerator.MoveNext()).Aggregate((workingSentence, next) =>
                                                  next && workingSentence);
            if (hasEmpty)
            {
                throw new ArgumentException("Can't sweep through empty set");
            }


            // check that all list has the same count of elements
            var sizes = par.Select(enumerable => enumerable.Count()).ToList();
            int etalon = par[0].Count();
            foreach (var size in sizes)
            {
                if (size != etalon)
                    throw new ArgumentException("Can't zip lists with different size while sweeping");
            }

            // main loop
            for (int i = 0; i < etalon; i++)
            {
                IList<T> el = new List<T>();
                foreach (var p in par)
                {
                    // get ith element of enum
                    el.Add(p.ElementAt(i));
                }
                ret.Add(el);
            }

            return ret;
        }
        #endregion

        protected override void RunStart()
        {
            State = STATE_RUN_START;

            // подписка на run_finished
            Subscribe(ACTION_SET_RUN_RESULTS, new FlowEvent(FlowEvent.RUN_FINISHED, Context.WfId, Id, null), TriggerEventExpressionType.And);

            // Проверка и интерпретация параметров
            IDictionary<string, ValueBase> paramValues;
            try
            {
                paramValues = Context.CodeInterpreter.InterpreteRunParameters(block.RunParameters);
                //TODO: check package run. take a look of StepNode
            }
            catch (InterpretionException e)
            {
                _log.ErrorException(String.Format("Exception while interpreting parameters in step#{0}. Rethrow. Refactor this place manually.", Id), e);
                // Выставляем шагу ошибку
                // генерируем событие
                ErrorUserComment = "Error while interpreting parameters";
                VerboseErrorComment = e.Message;
                ErrorException = e;
                State = STATE_ERROR;
                return;
            }
            // TODO: возможно убрать это при рефакторинге исключений
            catch (InvalidOperationException ex)
            {
                _log.ErrorException(String.Format("Exception while interpreting parameters in step#{0}. Rethrow. Refactor this place manually.", Id), ex);
                // Выставляем шагу ошибку
                // генерируем событие
                ErrorUserComment = "Error while interpreting parameters";
                VerboseErrorComment = ex.Message;
                ErrorException = ex;
                State = STATE_ERROR;
                return;
            }

            // Разбираем sweep
            IList<NamedParameter> sweepParams = new List<NamedParameter>();
            foreach (var runParameter in block.RunParameters.Parameters)
            {
                if (runParameter.IsSweepParam)
                {
                    if (paramValues[runParameter.Name] is UndefinedValue)
                    {
                        _log.Warn("Sweep parameter is undefined. Converting to [Undefined]");
                        paramValues[runParameter.Name] = new ListValue(new List<Expression>() { new LiteralExpression(new UndefinedValue()) });
                    }
                    else if (!(paramValues[runParameter.Name] is ListValue))
                        throw new NotImplementedException(String.Format("Sweeping is only supported for lists, got '{0}'", paramValues[runParameter.Name].GetType()));

                    sweepParams.Add(runParameter);
                }
            }

            // Generating nodes
            _log.Trace("Generating sweep nodes");

            var inp = new List<IEnumerable<Expression>>();
            for (int i = 0; i < sweepParams.Count; i++)
            {
                var lv = (ListValue)paramValues[sweepParams[i].Name];

                if (lv.AsList.Count > 0)
                    inp.Add(lv.AsList);
                else
                    inp.Add(new List<Expression>() { new LiteralExpression(new UndefinedValue()) });
            }

            IEnumerable<IList<Expression>> outp = null;

            bool foundInBlock = false;
            foreach (var namedParameter in block.ExecParameters.Parameters.AsEnumerable())
            {
                if (namedParameter.Name.ToLower() == "sweepmode")
                {
                    foundInBlock = true;
                    if (((StringValue)namedParameter.Value.Evaluate(null).Value).AsString == "zip")
                    {
                        // соответствие
                        outp = ZipIt(inp);
                        _log.Trace("Enabled 'zip' sweep mode");
                    }
                    else
                    {
                        // декартово произведение
                        outp = DecartIt(inp);
                    }
                    break;
                }
            }

            if (!foundInBlock)
            {
                if (Context.ExecutionContext.ExtraElements.ContainsKey("SweepMode"))
                {
                    if (((string)Context.ExecutionContext.ExtraElements["SweepMode"]).ToLower() == "zip")
                    {
                        // соответствие
                        outp = ZipIt(inp);
                        _log.Trace("Enabled 'zip' sweep mode");
                    }
                    else
                        // декартово произведение
                        outp = DecartIt(inp);
                }
                else
                {
                    // декартово произведение
                    outp = DecartIt(inp);
                }
            }

            int ind = 0;

            foreach (var set in outp)
            {
                // form param set

                var newPars = new List<NamedParameter>();

                for (int i = 0; i < sweepParams.Count; i++)
                {
                    NamedParameter newpar = (NamedParameter)(sweepParams[i]).Clone();
                    newpar.IsSweepParam = false;
                    newpar.Value = set[i];
                    newPars.Add(newpar);
                }

                _log.Trace("Clonning node to {0} with params [{1}]", Name + "." + ind.ToString(), String.Join(", ", newPars.Select(parameter => parameter.Name + " = " + parameter.Value.ToString())));

                //TODO: zn: dirty hack
                long newId = (Id + 1) * 1000 + ind;

                // Generate node
                var n = (StepNode)CloneNode(newId, Name + "_" + ind.ToString(), newPars);

                // dependencies
                // start of sweepers after signal
                n.AddDependencyTrigger(this, ACTION_START, FlowEvent.START_SWEEPING, TriggerEventExpressionType.Or);

                AddErrorHandlingDependency(n);

                n.SetParentChildConnection(this);
                //this.SetParentChildConnection(n);

                // list for good representation, depends to shceduler
                SweepChildren.Add(n);

                // join of steprunresults
                //AddDependencyTrigger(n, ACTION_SWEEP_FINISH, FlowEvent.RUN_FINISHED, TriggerEventExpressionType.And);
                Subscribe(ACTION_SWEEP_FINISH, new FlowEvent(FlowEvent.RUN_FINISHED, Context.WfId, n.Id, null), TriggerEventExpressionType.And);


                //TODO: wait for other

                //add node to graph
                Context.NodeGraphController.AddNodeDeffered(n);

                ind++;
            }

            State = STATE_SWEEPING;
            _log.Trace("Node#{0} generated event START_SWEEPING", Id);
            GenerateEvent(FlowEvent.START_SWEEPING);
        }

        protected override void RunFinish(StepRunResult result = null)
        {
            State = STATE_RUN_FINISH;
        }

        protected virtual void SweepFinish(object resultso)
        {
            IEnumerable<StepRunResult> results;
            if (resultso is IEnumerable<StepRunResult>)
            {
                results = (IEnumerable<StepRunResult>)resultso;
            }
            else
            {
                StepRunResult result = resultso as StepRunResult;
                results = new List<StepRunResult>() { result };
            }

            State = STATE_SWEEPING_FINISHED;

            bool wasSucceed = true;
            string errComm = "";
            foreach (var stepRunResult in results)
            {
                if (stepRunResult.Status == StepRunResult.ResultStatus.Failed)
                {
                    wasSucceed = false;
                    errComm += String.Format("- {0}\n", stepRunResult.ErrorComment);
                }
            }

            if (wasSucceed)
            {
                //------------------------
                // sweep result
                //------------------------
                //формируем новый резалт
                BlockStructureValue bsv = new BlockStructureValue(block.Name);
                bsv[BlockStructureValue.RUN_RESULT_STATUS_NAME] = new StringValue(BlockStructureValue.RUN_RESULT_STATUS_FINISHED);

                ListValue sweep_outs = new ListValue(new List<Expression>());

                foreach (var res in results)
                {
                    HashValue outputs = new HashValue(new HashDict(new Dictionary<ValueBase, ValueBase>()));

                    foreach (var fd in res.OutputFiles)
                    {
                        outputs.AsHash.Add(new StringValue(fd.FileIdentifier), new FileValue(fd));
                    }

                    sweep_outs.AsList.Add(new LiteralExpression(outputs));
                }

                bsv[BlockStructureValue.SWEEP_OUTPUTS_NAME] = sweep_outs;

                Context.CodeInterpreter.SetValue(new CompoundVarIdentifier(RUN_RESULT_VARIABLE_NAME), bsv);

                // Обновляем ее в глобальном контексте
                Context.CodeInterpreter.ShareVariable(RUN_RESULT_VARIABLE_NAME);

            }
            else
            {
                _log.Error("Step crashed. Skipping post section.");
                //формируем новый резалт
                BlockStructureValue bsv1 = new BlockStructureValue(block.Name);
                bsv1[BlockStructureValue.RUN_RESULT_STATUS_NAME] = new StringValue(BlockStructureValue.RUN_RESULT_STATUS_FAILED);
                Context.CodeInterpreter.SetValue(new CompoundVarIdentifier(RUN_RESULT_VARIABLE_NAME), bsv1);

                // Обновляем ее в глобальном контексте
                Context.CodeInterpreter.ShareVariable(RUN_RESULT_VARIABLE_NAME);

                // генерируем событие
                ErrorUserComment = "Step crashed in Execution system";
                VerboseErrorComment = errComm;
                State = STATE_ERROR;
            }

        }

        protected override void Action(string actionName, object arg)
        {
            if (actionName == ACTION_SWEEP_FINISH)
            {
                IEnumerable<StepRunResult> ress;
                if (arg is StepRunResult)
                {
                    ress = new List<StepRunResult>() { (StepRunResult)arg };
                }
                else
                {
                    var results = (IEnumerable<object>)arg;
                    ress = results.Select(o => (StepRunResult)o).ToList();
                }
                SweepFinish(ress);

                RunFinish(null);
                if (State != STATE_ERROR)
                {
                    PostSection();
                    // генерируем событие
                    State = STATE_FINISHED;
                }

            }
            else
            {
                base.Action(actionName, arg);
            }

        }
    }
}
