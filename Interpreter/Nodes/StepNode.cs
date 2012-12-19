using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Easis.Common;
using Easis.Common.DataContracts;
using Easis.Wfs.EasyFlow.Model;
using Easis.Wfs.EasyFlow.Model.Expressions;
using Easis.Wfs.Interpreting.Imperative;
using Newtonsoft.Json.Linq;

namespace Easis.Wfs.Interpreting
{
    public class StepNode : NodeBase
    {
        protected readonly StepBlock block;

        public string Name { get { return ((StepBlock)_block).Name; } }

        //internal StepBlock Block
        //{
        //    get { return block; }
        //}

        // перегружаем базовый контекст
        protected new IStepNodeContext Context { get; set; }

        public const string STATE_PRE_SECTION = "state_pre_section";
        public const string STATE_RUN_START = "state_run_start";
        public const string STATE_WAIT_RESULTS = "state_wait_results";
        public const string STATE_RUN_FINISH = "state_run_finish";
        public const string STATE_POST_SECTION = "state_post_section";

        public const string ACTION_SET_RUN_RESULTS = "action_set_run_results";
        public const string ACTION_SET_RUN_INFO = "action_set_run_info";

        #region buffer for status
        protected StepRunResult _results = null;
        protected StepRunDescriptor _descriptor = null;
        protected StepRunInfo _info = null;
        protected IDictionary<string, string> _params = null;
        #endregion

        public StepNode(StepBlock block, IStepNodeContext context, WfLog log)
            : base(block, context, log)
        {
            Context = context;
            this.block = block;
        }

        protected virtual void PreSection()
        {
            State = STATE_PRE_SECTION;
            if (block.PreSection == null)
            {
                _log.Trace("{0} Pre section is NULL ignoring", ToString());
            }
            else
            {
                _log.Trace("{0} Start interpretation of PRE code section", ToString());
                Context.CodeInterpreter.InterpreteCodeSection(block.PreSection);
            }
        }


        protected virtual void HandleExecutionParams(StepRunDescriptor stepRunDescriptor)
        {
            StepRunMode runMode = StepRunMode.Meta;

            // exon hack:
            try
            {
                foreach (var parameter in block.ExecParameters.Parameters.AsEnumerable())
                {
                    // magic for rawmode
                    _log.Trace("Execution param: {0} {1}", parameter.Name, parameter.Value.Evaluate(null).Value.ToJsonString());
                    string name = parameter.Name;
                    var value = parameter.Value.Evaluate(null).Value;
                    string val;
                    if (value is StringValue)
                        val = ((StringValue)value).AsString;
                    else if (value is String)
                        val = (String)value;
                    else if (value is Int32)
                        val = ((Int32)value).ToString();
                    else
                    {
                        _log.Error("Flow attribute parameter '{0}' has unexpected type '{1}'. Ignoring.", name, value.GetType().Name);
                        continue;
                    }

                    if (name.ToLower().Equals("mode") && val.ToLower().Equals("raw"))
                        runMode = StepRunMode.Raw;

                    stepRunDescriptor.ExecutionContext.ExtraElements[name] = val;
                }

            }
            catch (Exception ex)
            {
                _log.ErrorException("Error while investigating runmode", ex);
            }
            stepRunDescriptor.RunMode = runMode;
        }
        /// <summary>
        /// Заполнение информации о параметрах для запуска пакета
        /// </summary>
        /// <param name="stepRunDescriptor"></param>
        /// <param name="paramValues"></param>
        /// <returns></returns>
        protected StepRunParameters GetStepRunParams(StepRunDescriptor stepRunDescriptor, IDictionary<string, ValueBase> paramValues)
        {
            StepRunParameters parameters = new StepRunParameters();
            _log.Trace("Creating parameter list");
            // Заполняем информацию о параметрах- в RunDescriptor
            foreach (var paramValue in paramValues)
            {
                _log.Trace("Adding param {0} {1} {2}", paramValue.Key, paramValue.Value.DataType, paramValue.Value.Value);

                if (paramValue.Value.Value == null)
                {
                    //TODO: bug
                }

                switch (paramValue.Value.DataType)
                {
                    //DRYRUN
                    case DataType.Undefined:
                        // не добавляем неизвестные параметры
                        //parameters.Params.Add(paramValue.Key, paramValue.Value.ToString());
                        _log.Trace("Ignore undefined param '{0}' while constructing StepRunDescriptor", paramValue.Key);
                        break;
                    case DataType.String:
                    case DataType.Int:
                    case DataType.Bool:
                        // TODO: внести метод tostring в valuebase
                        if (paramValue.Value.Value != null)
                            parameters.Params.Add(paramValue.Key, paramValue.Value.Value.ToString());
                        break;
                    case DataType.Double:
                        // TODO: внести метод tostring в valuebase
                        if (paramValue.Value.Value != null)
                        {
                            CultureInfo ci = (CultureInfo)CultureInfo.InvariantCulture.Clone();
                            ci.NumberFormat.NumberDecimalSeparator = ".";
                            parameters.Params.Add(paramValue.Key, ((double)paramValue.Value.Value).ToString(ci));
                        }
                        break;
                    case DataType.File:
                        // в параметрах указываются только инпуты, аутпут забираются автоматически
                        FileParam fp = new FileParam((FileDescriptor)paramValue.Value.Value, paramValue.Key);
                        parameters.InputFiles.Add(fp);
                        break;
                    case DataType.List:
                        // обработка file_group
                        ListValue lv = paramValue.Value as ListValue;

                        string fileGroupStr = "";

                        // mascarading
                        int index = 0;
                        foreach (var l in lv.AsList)
                        {
                            ValueBase vb = Context.CodeInterpreter.EvaluateExpression(l);
                            if (vb is FileValue)
                            {
                                var fv = (FileValue)vb;
                                fileGroupStr += String.Format("{0}_{1}|{2},", index++, fv.AsFileDescriptor.FileIdentifier,
                                                              fv.AsFileDescriptor.NStorageId);
                            }
                            else
                            {
                                _log.Error("List parameters can contain only files. Unexpected parameter with type '{0}'", vb.GetType());
                                throw new InterpretionException(String.Format("List parameters can contain only files. Unexpected parameter with type '{0}'", vb.GetType()));
                            }
                        }
                        fileGroupStr.TrimEnd(new char[] { ',' });

                        parameters.Params.Add(paramValue.Key, fileGroupStr);
                        break;
                    case DataType.Hash:
                    case DataType.Structure:
                    /*  case DataType.Undefined:
                          throw new InterpretionException(String.Format("Unsupported type ({0}) for parameter {1}", paramValue.Value.DataType,paramValue.Key));
                          break;*/
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                //_log.Trace("Added");
            }

            // Проставляем аутпуты
            // TODO: избирательное добавление аутпутов / аутпуты не сохраняются
            IEnumerable<FileParam> outputs;
            try
            {
                outputs = FormOutputs(stepRunDescriptor.PackageName, stepRunDescriptor.MethodName);
            }
            catch (Exception ex)
            {
                _log.Error("[Ignorring temporary] Error while forming outputs");
                outputs = new List<FileParam>();
            }

            foreach (var output in outputs)
            {
                parameters.OutputFiles.Add(output);
            }

            return parameters;
        }
        /// <summary>
        /// Действие. Запуск пакета на исполнение. Подготовка и запуск через механизм запуска.
        /// </summary>
        protected virtual void RunStart()
        {
            State = STATE_RUN_START;

            // подписка на run_finished
            Subscribe(ACTION_SET_RUN_RESULTS, new FlowEvent(FlowEvent.RUN_FINISHED, Context.WfId, Id, null), TriggerEventExpressionType.And);

            Subscribe(ACTION_HANDLE_ERROR, new FlowEvent(FlowEvent.ERROR, Context.WfId, Id, null),
                      TriggerEventExpressionType.Or);

            // подписка на run_started
            Subscribe(ACTION_SET_RUN_INFO, new FlowEvent(FlowEvent.RUN_STARTED, Context.WfId, Id, null), TriggerEventExpressionType.And);

            StepRunDescriptor stepRunDescriptor = new StepRunDescriptor(Context.WfId, Id);

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
                // TODO: Enable package check
                try
                {
                    CheckPackageRun(stepRunDescriptor.PackageName, stepRunDescriptor.MethodName, paramValues);
                }
                catch (Exception ex)
                {
                    _log.Error("[Ignorring temporary] Error while checking package run signature.");
                }
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

            //long[] depends = DependsOnNodeIndexes().ToArray();
            //_log.Trace("Investigated dependencies for step {0}: [{1}]", Name, String.Join(", ", depends));
            //stepRunDescriptor.DependsOn = depends;

            //pare

            //TODO: в параметрах не должно быть internalId
            stepRunDescriptor.RunParameters = GetStepRunParams(stepRunDescriptor, paramValues);

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

        // Helper
        private IEnumerable<FileParam> FormOutputs(string packageName, string methodName)
        {
            // Поиск информации о пакете
            PackageInfo[] pi = Context.PackageRegistry.FindPackage(packageName);
            if (pi.Length > 1 || pi.Length < 1)
            {
                throw new InvalidOperationException(String.Format("Package {0} not found (matched number = {1}).", packageName, pi.Length));
            }

            PackageInfo packageInfo = pi[0];

            // Поиск метода
            PackageMethodInfo mi = packageInfo.GetMethodByName(methodName);

            List<FileParam> ret = new List<FileParam>();
            foreach (var result in mi.Results)
            {
                FileDescriptor fd = Context.FileRegistry.CreateFileDescriptor();
                fd.FileIdentifier = result.Name;
                FileParam fp = new FileParam(fd, result.Name);
                ret.Add(fp);
            }

            return ret;
        }

        /// <summary>
        /// Проверка сигнатуры вызова пакета.
        /// При несоответствии кидается исключение InterpretionException
        /// </summary>
        /// <param name="packageName"></param>
        /// <param name="methodName"></param>
        /// <param name="paramValues"></param>
        /// <returns></returns>
        private void CheckPackageRun(string packageName, string methodName, IEnumerable<KeyValuePair<string, ValueBase>> paramValues)
        {
            // Поиск информации о пакете
            PackageInfo[] pi = Context.PackageRegistry.FindPackage(packageName);
            if (pi.Length > 1 || pi.Length < 1)
            {
                throw new InvalidOperationException(String.Format("Package {0} not found (matched number = {1}).",
                                                                     packageName, pi.Length));
            }

            PackageInfo packageInfo = pi[0];

            // Поиск метода
            PackageMethodInfo mi = packageInfo.GetMethodByName(methodName);

            // Структура данных для определения, все ли необходимые параметры указаны
            IDictionary<string, bool> paramsFlags = new Dictionary<string, bool>();
            foreach (var param in mi.Params)
                paramsFlags.Add(param.Name, !param.Required);

            foreach (var paramValue in paramValues)
            {
                // Валидация типов параметров
                ParamInfo pai = mi.GetParamByName(paramValue.Key);

                if (pai.DataType != paramValue.Value.DataType)
                {
                    //TODO: пробуем привести типы

                    throw new InterpretionException(String.Format("Invalid type of parameter '{0}' {1} need {2} in method {3}", paramValue.Key, pai.DataType, paramValue.Value.DataType, packageInfo.Name + "." + mi.Name));
                }
                else
                {
                    paramsFlags[pai.Name] = true;
                }
            }

            // Проверяем, все ли необходимые параметры указаны
            foreach (var paramsFlag in paramsFlags)
            {
                if (!paramsFlag.Value)
                {
                    throw new InterpretionException("Required parameter {0} is absent.");
                }
            }
        }
        /// <summary>
        /// Действие. Завершение исполнения узла.
        /// </summary>
        /// <param name="result">Результат исполнения пакета</param>
        protected virtual void RunFinish(StepRunResult result)
        {
            if (result == null) throw new ArgumentNullException("result");

            State = STATE_RUN_FINISH;

            _results = result;

            switch (result.Status)
            {
                case StepRunResult.ResultStatus.Completed:

                    //формируем новый резалт
                    BlockStructureValue bsv = new BlockStructureValue(block.Name);
                    bsv[BlockStructureValue.RUN_RESULT_STATUS_NAME] = new StringValue(BlockStructureValue.RUN_RESULT_STATUS_FINISHED);
                    HashValue outputs = new HashValue();
                    bsv[BlockStructureValue.RUN_RESULT_OUTPUTS_NAME] = outputs;

                    foreach (var fd in result.OutputFiles)
                    {
                        fd.Type = FileDescriptor.FileType.GeneratedAfterRun;
                        Context.FileRegistry.RegisterFile(fd);
                        outputs.AsHash.Add(new StringValue(fd.FileIdentifier), new FileValue(fd));
                    }

                    StructureValue varOutputs = new StructureValue(new Dictionary<string, ValueBase>());
                    JsonDataFormatter jdf = new JsonDataFormatter(_log);
                    // output params
                    foreach (var outputParam in result.OutputParams)
                    {
                        try
                        {
                            ValueBase vb = jdf.ConvertValueFromJson(outputParam.Value);
                            varOutputs[outputParam.Key] = vb;
                            _log.Trace("Output param '{0}'({1}) was set to '{2}'", outputParam.Key, vb.DataType, vb);
                        }
                        catch (Exception ex)
                        {
                            _log.ErrorException(String.Format("Exception while converting output param '{0}' with value '{1}'. Ignoring.", outputParam.Key, outputParam.Value), ex);
                        }
                    }
                    bsv["Outputs"] = varOutputs;

                    Context.CodeInterpreter.SetValue(new CompoundVarIdentifier(RUN_RESULT_VARIABLE_NAME), bsv);

                    // Обновляем ее в глобальном контексте
                    Context.CodeInterpreter.ShareVariable(RUN_RESULT_VARIABLE_NAME);

                    break;
                case StepRunResult.ResultStatus.Failed:
                    _log.Error("Step crashed. Skipping post section.");
                    //формируем новый резалт
                    BlockStructureValue bsv1 = new BlockStructureValue(block.Name);
                    bsv1[BlockStructureValue.RUN_RESULT_STATUS_NAME] = new StringValue(BlockStructureValue.RUN_RESULT_STATUS_FAILED);
                    Context.CodeInterpreter.SetValue(new CompoundVarIdentifier(RUN_RESULT_VARIABLE_NAME), bsv1);

                    // Обновляем ее в глобальном контексте
                    Context.CodeInterpreter.ShareVariable(RUN_RESULT_VARIABLE_NAME);

                    // генерируем событие
                    ErrorUserComment = "Step crashed in Execution system";
                    VerboseErrorComment = result.ErrorComment;
                    State = STATE_ERROR;
                    break;
                //---- DRYRUN ----
                case StepRunResult.ResultStatus.Unknown:
                    _log.Trace("Step '{0}' finished in dry run mode. Result set to unknown.", Name);
                    Context.CodeInterpreter.SetValue(new CompoundVarIdentifier(RUN_RESULT_VARIABLE_NAME), new UndefinedValue());
                    // Обновляем ее в глобальном контексте
                    Context.CodeInterpreter.ShareVariable(RUN_RESULT_VARIABLE_NAME);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        /// <summary>
        /// Пост-обработка.
        /// </summary>
        protected virtual void PostSection()
        {
            State = STATE_POST_SECTION;

            if (block.PostSection == null && String.IsNullOrEmpty(block.PostCodeSection))
            {
                _log.Trace("{0} Post section is NULL ignoring", ToString());
            }
            else if (block.PostSection == null)
            {
                if (Context.StepStarter.IsDry)
                {
                    _log.Trace("{0} Skipping post section in dry run mode", ToString());

                    Context.CodeInterpreter.SetValue(new CompoundVarIdentifier(POST_RESULT_VARIABLE_NAME), new UndefinedValue());

                    // Обновляем ее в глобальном контексте
                    Context.CodeInterpreter.ShareVariable(POST_RESULT_VARIABLE_NAME);
                    return;
                }
                else
                {
                    _log.Trace("{0} Start interpretation of post script code section", ToString());
                    ScriptInterpreterFactory fact = new ScriptInterpreterFactory(_log);
                    IScriptInterpreter si = fact.GetScriptInterpreter("ruby");
                    HashValue hv = si.ExecuteScript(Context.CodeInterpreter.GetGlobalDataScope(),
                                     Context.CodeInterpreter.GetBlockDataScope(), block.PostCodeSection);

                    Context.CodeInterpreter.SetValue(new CompoundVarIdentifier(POST_RESULT_VARIABLE_NAME), hv);
                    // Обновляем ее в глобальном контексте
                    Context.CodeInterpreter.ShareVariable(POST_RESULT_VARIABLE_NAME);
                }
            }
            else
            {
                if (Context.StepStarter.IsDry)
                {
                    _log.Trace("{0} Skipping post section in dry run mode", ToString());
                    return;
                }
                {
                    _log.Trace("{0} Start interpretation of POST code section", ToString());
                    Context.CodeInterpreter.InterpreteCodeSection(block.PostSection);
                }
            }
        }
        /// <summary>
        /// Обработка действия
        /// </summary>
        /// <param name="actionName">Действие</param>
        /// <param name="arg">Аргумент</param>
        protected override void Action(string actionName, object arg = null)
        {
            if (actionName == null) throw new ArgumentNullException("actionName");

            _log.Trace("Node {0} action {1} was called with arg {2}", Name, actionName, arg);

            switch (actionName)
            {
                case ACTION_START:
                    if (State == STATE_INITIALIZE)
                    {
                        Init();
                        PreSection();
                        RunStart();
                    }
                    else
                    {
                        _log.Error("DUPLICATE INVOCATION OF ACTION START. DEMO COMMENTED");
                    }
                    break;
                case ACTION_SET_RUN_RESULTS:
                    // TODO: check / duty
                    StepRunResult result = (StepRunResult)arg;
                    RunFinish(result);
                    if (State != STATE_ERROR)
                    {
                        PostSection();
                        // генерируем событие
                        State = STATE_FINISHED;
                    }
                    break;
                case ACTION_SET_RUN_INFO:
                    if (arg != null)
                        _info = (StepRunInfo)arg;
                    else
                    {
                        _log.Warn("StepRunInfo is equal to null. Ignorring.");
                    }
                    State = STATE_WAIT_RESULTS;
                    break;
                case ACTION_HANDLE_ERROR:
                    ErrorUserComment = "Error";
                    VerboseErrorComment = (string)arg;
                    State = STATE_ERROR;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(String.Format("Unknown action {0} for StepNode", actionName));
            }
        }

        public const string RUN_RESULT_VARIABLE_NAME = "Result";
        public const string POST_RESULT_VARIABLE_NAME = "Post";

        protected virtual void Init()
        {
            State = STATE_STARTED;

            // добавляем волшебную переменную result
            Variable result = new Variable(RUN_RESULT_VARIABLE_NAME, new BlockStructureValue(block.Name));

            result.Modifier = VariableModifier.Readonly;
            result.AccessMode = VariableAccessMode.Public;

            // добавляем в текущий контекст
            //TODO: demo убрать !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            try
            {
                Context.CodeInterpreter.DefineVariable(result);
            }
            catch (Exception ex)
            {
                _log.ErrorException("Error in define. Ignoring.", ex);
            }
            // Обновляем ее в глобальном контексте
            Context.CodeInterpreter.ShareVariable(result);
        }

        public override string ToString()
        {
            return String.Format("Step:{2}#{0}({1})", Id, State, block.Name);
        }
        /// <summary>
        /// Клонирование узла. Используется для динамического создания узлов (например, для варьирования параметров).
        /// </summary>
        /// <param name="newId">Новый идентификатор</param>
        /// <param name="newName">Новое имя узла</param>
        /// <param name="newParams">Новые входные параметры</param>
        /// <returns></returns>
        public override NodeBase CloneNode(long newId, string newName, IEnumerable<NamedParameter> newParams)
        {
            // clonning my block
            StepBlock sb = (StepBlock)_block.Clone();
            sb.Name = newName;
            sb.Id = newId;

            _log.Trace("After block cloning. Params [{0}]", string.Join(", ", sb.RunParameters.Parameters.Select(parameter => parameter.Name)));

            foreach (var namedParameter in newParams)
            {
                // копирование вместо вставки
                var par = sb.RunParameters[namedParameter.Name];
                par.Value = namedParameter.Value;
                par.IsSweepParam = namedParameter.IsSweepParam;
                par.IsOntoParam = namedParameter.IsOntoParam;
            }

            StepNodeContext snc = (StepNodeContext)Context.Clone();
            snc.CodeInterpreter = new ImperativeInterpreter(snc.CodeInterpreter.GetGlobalDataScope(), newName);
            StepNode sn = new StepNode(sb, snc, _log);

            return sn;
        }

        public override NodeDescription GetNodeDescription()
        {
            NodeDescription ret = new NodeDescription();

            ret.StepId = (uint)Id;
            ret.Name = block.Name;
            ret.Type = "step";

            //TODO: разобраться с резолвером / duty
            PackageNameResolver resolver = new PackageNameResolver();

            ret.PackageName = resolver.ResolvePackageName(block.RunObjectName);
            ret.MethodName = resolver.ResolveMethodName(block.RunObjectName);
            ret.Parents = new List<uint>(from x in Parents where !(x is FlowSinkNode) && !(x is FlowSourceNode) select (uint)x.Id);
            ret.Children = new List<uint>(from x in Children where !(x is FlowSinkNode) && !(x is FlowSourceNode) select (uint)x.Id);

            if (_descriptor != null)
            {
                if (_descriptor.RunParameters != null)
                {
                    // add in parameters
                    //if (_descriptor.RunParameters.Params != null)
                    //{
                    //TODO: inParams
                    //ret.InParams = new Dictionary<string, string>();
                    //foreach (var stepRunParameter in _descriptor.RunParameters.Params)
                    //{
                    //    ret.InParams.Add(stepRunParameter.Key, stepRunParameter.Value);
                    //}
                    //}
                    // add in files
                    if (_descriptor.RunParameters.InputFiles != null)
                    {
                        ret.InFiles = new List<FileDescription>();
                        foreach (var fileDescriptor in _descriptor.RunParameters.InputFiles)
                        {
                            if (fileDescriptor.FileDescriptor != null)
                            {
                                ret.InFiles.Add(new FileDescription()
                                {
                                    FileIdentifier = fileDescriptor.FileDescriptor.FileIdentifier,
                                    NStorageId = fileDescriptor.FileDescriptor.NStorageId,
                                    StorageId = fileDescriptor.FileDescriptor.StorageId,
                                    Id = fileDescriptor.FileDescriptor.Id.ToString(),
                                    Type = fileDescriptor.FileDescriptor.Type.ToString()
                                });
                            }
                        }
                    }
                }
            }

            if (_results != null)
            {
                ret.ExternalId = _results.ExternalId;
                switch (_results.Status)
                {
                    case StepRunResult.ResultStatus.Completed:
                        ret.OutFiles = new List<FileDescription>();
                        foreach (var fileDescriptor in _results.OutputFiles)
                        {
                            ret.OutFiles.Add(new FileDescription()
                            {
                                FileIdentifier = fileDescriptor.FileIdentifier,
                                NStorageId = fileDescriptor.NStorageId,
                                StorageId = fileDescriptor.StorageId,
                                Id = fileDescriptor.Id.ToString(),
                                Type = fileDescriptor.Type.ToString()
                            });
                        }
                        break;
                    case StepRunResult.ResultStatus.Failed:
                        break;
                    case StepRunResult.ResultStatus.Unknown:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            ret.State = State;
            if (State == STATE_ERROR)
            {
                if (ErrorUserComment != null)
                    ret.ErrorComment = ErrorUserComment.Sanitize();
                if (VerboseErrorComment != null)
                    ret.VerboseErrorComment = VerboseErrorComment.Sanitize();
                if (ErrorException != null)
                    ret.ErrorException = ErrorException.ToString().Sanitize();
            }

            if (_info != null)
            {
                ret.RunInfo = new RunInfo();
                ret.RunInfo.ExternalId = _info.ExternalId;
                ret.RunInfo.ResourceInfo = _info.ResourceInfo;
                ret.RunInfo.NodeInfo = _info.NodeInfos;
                ret.RunInfo.Estimated = _info.Estimated;
                ret.RunInfo.Started = _info.Started;
            }

            return ret;
        }

        #region json
        //public override string GetNodeStatus()
        //{
        //    string ret = "";
        //    ret += "{\n";
        //    ret += "  \"stepid\":" + Id + ",\n";
        //    ret += "  \"name\":'" + block.Name + "',\n";
        //    ret += "  \"type\":" + "\"step\"" + ",\n";

        //    //TODO: разобраться с резолвером / duty
        //    PackageNameResolver resolver = new PackageNameResolver();
        //    ret += "  \"package\": \"" + resolver.ResolvePackageName(block.RunObjectName) + "\",\n";
        //    ret += "  \"method\": \"" + resolver.ResolveMethodName(block.RunObjectName) + "\",\n";

        //    var t = from x in Parents where !(x is FlowSinkNode) && !(x is FlowSourceNode) select x.Id.ToString();
        //    ret += "  \"parents\": [" + String.Join(",", t) + "],\n";

        //    t = from x in Children where !(x is FlowSinkNode) && !(x is FlowSourceNode) select x.Id.ToString();
        //    ret += "  \"children\": [" + String.Join(",", t) + "],\n";

        //    if (_results != null)
        //    {
        //        ret += "  \"externalId\": '" + _results.ExternalId + "',\n";
        //        switch (_results.Status)
        //        {
        //            case StepRunResult.ResultStatus.Completed:
        //                ret += "'outputs': [\n";
        //                foreach (var fileDescriptor in _results.OutputFiles)
        //                {
        //                    ret += "{\n";
        //                    ret += "'fileIdentifier': '" + fileDescriptor.FileIdentifier + "',";
        //                    ret += "'nStorageId': '" + fileDescriptor.NStorageId + "',";
        //                    ret += "'storageId': '" + fileDescriptor.StorageId + "',";
        //                    ret += "'internalId': '" + fileDescriptor.Id + "',";
        //                    ret += "'type': '" + fileDescriptor.Type + "'";
        //                    ret += "},\n";
        //                }
        //                ret += "],\n";
        //                break;
        //            case StepRunResult.ResultStatus.Failed:
        //                break;
        //            default:
        //                throw new ArgumentOutOfRangeException();
        //        }
        //    }

        //    ret += "  \"state\": \"" + State + "\",\n";
        //    if (State == STATE_ERROR)
        //    {
        //        ret += "\"errorComment\": " + ErrorUserComment.Sanitize() + ",\n";
        //        ret += "\"verboseErrorComment\": " + VerboseErrorComment.Sanitize() + ",\n";
        //        if (ErrorException != null)
        //            ret += "\"errorException\": " + ErrorException.ToString().Sanitize() + ",\n";
        //    }

        //    if (_info != null)
        //    {
        //        ret += "'runInfo': {\n";
        //        ret += "'ExternalId': '" + _info.ExternalId + "',";
        //        ret += "'ResourceType': '" + _info.ResourceInfo.ResourceType + "',";
        //        ret += "'ResourceName': '" + _info.ResourceInfo.ResourceName + "',";
        //        ret += "'CoresCount': " + _info.ResourceInfo.CoresCount.ToString() + ",";
        //        if (_info.Started.HasValue)
        //            ret += "'Started': " + JValue.FromObject(_info.Started.Value).ToString() + ",";
        //        if (_info.Ended.HasValue)
        //            ret += "'Ended': " + JValue.FromObject(_info.Ended.Value) + ",";
        //        if (_info.Estimated.HasValue)
        //            ret += "'Estimated': " + JValue.FromObject(_info.Estimated.Value) + ",";
        //        ret += "'State': '" + _info.State + "'";
        //        ret += "},\n";
        //    }

        //    ret += "}\n";
        //    return ret;
        //}
        #endregion
    }
}
