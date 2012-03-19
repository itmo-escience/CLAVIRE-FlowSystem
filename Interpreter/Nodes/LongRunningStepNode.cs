using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Easis.Common;
using Easis.Common.DataContracts;
using Easis.Wfs.EasyFlow.Model;

namespace Easis.Wfs.Interpreting.Nodes
{
    public class LongRunningStepNode : StepNode
    {
        protected new ILongRunningStepNodeContext Context { get; set; }

        public LongRunningStepRunInfo LongRunInfo { get; set; }

        public LongRunningStepNode(StepBlock block, ILongRunningStepNodeContext context)
            : base(block, context)
        {
            Context = context;
        }

        protected override void RunStart()
        {
            State = STATE_RUN_START;

            // подписка на run_finished
            Subscribe(ACTION_SET_RUN_RESULTS, new FlowEvent(FlowEvent.RUN_FINISHED, Context.WfId, Id, null), TriggerEventExpressionType.And);

            Subscribe(ACTION_HANDLE_ERROR, new FlowEvent(FlowEvent.ERROR, Context.WfId, Id, null),
          TriggerEventExpressionType.Or);

            // подписка на run_started
            Subscribe(ACTION_SET_RUN_INFO, new FlowEvent(FlowEvent.RUN_STARTED, Context.WfId, Id, null), TriggerEventExpressionType.And);

            StepRunDescriptor stepRunDescriptor = new StepRunDescriptor(Context.WfId, Id);

            // LRWF FLAG
            stepRunDescriptor.IsLongRunning = true;

            // Set execution context
            stepRunDescriptor.ExecutionContext = (ExecutionContext)Context.ExecutionContext.Clone();

            //TODO: разобраться с резолвером / duty
            PackageNameResolver resolver = new PackageNameResolver();
            stepRunDescriptor.PackageName = resolver.ResolvePackageName(block.RunObjectName);
            stepRunDescriptor.MethodName = resolver.ResolveMethodName(block.RunObjectName);

            HandleExecutionParams(stepRunDescriptor);

            _log.Info("RunMode was set to {0}", stepRunDescriptor.RunMode.ToString());

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

            // Запоминаем подписки
            IDictionary<string, string> subsParams = new Dictionary<string, string>();
            foreach (var runParameter in block.RunParameters.Parameters)
            {
                if (runParameter.IsSubscriptionParam)
                {
                    subsParams.Add(runParameter.Name, ((StringValue)paramValues[runParameter.Name]).AsString);
                }
            }

            // config file representation
            string subs = "";

            foreach (var subsParam in subsParams)
            {
                string[] strs = subsParam.Value.Split(new char[] { '.' });
                string StepName = strs[0];
                string outParamName = strs[1];

                StepNode snode = Context.NodeGraphController.GetStepNodeByName(StepName);
                if (snode is LongRunningStepNode)
                {
                    var lrnode = snode as LongRunningStepNode;
                    subs += String.Format("{0} {1} {3} -> {2}\n", lrnode.Id,
                                          lrnode.LongRunInfo.PublishingEndpoint, subsParam.Key, outParamName);
                }
                else
                {
                    throw new InterpretionException(String.Format("Subscription '{0}' cannot be set to simple batch step node, only to long-running node", subsParam.Key + " <- " + subsParam.Value));
                }
            }

            //-------------------------------
            // Making LRConfig
            //-------------------------------
            _log.Trace("Making lr-config");

            string id = Id.ToString();

            string configContent = String.Format("#---- DEFAULTS ----\n" +
                          "ID = {0}\n" +
                          "#---- SUBSCRIPTIONS ----\n" +
                          "{1}", id, subs);

            //-------------------------------
            var cfd = Context.Storage.SaveFile("lr.config", configContent);
            Context.FileRegistry.RegisterFile(cfd);

            // убираем из списка параметров параметры LRWF
            IDictionary<string, ValueBase> newParamValues = new Dictionary<string, ValueBase>();

            // если параметр был отмечен как подписка, забываем его
            foreach (var a in paramValues.Where(pair => !subsParams.ContainsKey(pair.Key)))
            {
                newParamValues.Add(a);
            }

            newParamValues.Add(new KeyValuePair<string, ValueBase>("lrconfig", new FileValue(cfd)));
            
            //long[] depends = DependsOnNodeIndexes().ToArray();
            //_log.Trace("Investigated dependencies for step {0}: [{1}]", Name, String.Join(", ", depends));
            //stepRunDescriptor.DependsOn = depends;
            
            //TODO: в параметрах не должно быть internalId
            stepRunDescriptor.RunParameters = GetStepRunParams(stepRunDescriptor, newParamValues);

            try
            {
                _descriptor = stepRunDescriptor;
                Context.StepStarter.StartStep(stepRunDescriptor);
            }
            catch (Exception ex)
            {
                _log.ErrorException("Error while starting step in Execution", ex);
                ErrorUserComment = "Error while starting step in Execution: " + ex.Message;
                VerboseErrorComment = ex.Message;
                State = STATE_ERROR;
            }
            // zn: перенесено в ACTION_SET_INFO
            // State = STATE_WAIT_RESULTS;

        }

        protected override void RunFinish(StepRunResult result = null)
        {
            State = STATE_RUN_FINISH;
        }

        protected override void Action(string actionName, object arg)
        {
            if (actionName == ACTION_SET_RUN_INFO)
            {
                if (arg != null)
                {
                    _info = (StepRunInfo)arg;

                    LongRunInfo = (LongRunningStepRunInfo) arg;
                }
                else
                {
                    _log.Warn("StepRunInfo is equal to null. Ignorring.");
                }
                State = STATE_WAIT_RESULTS;

                GenerateEvent(FlowEvent.STEP_RUN_INFO_READY);
            }
            else
            {
                base.Action(actionName, arg);
            }

        }
    }
}
