#undef SCOPE_TRACE
#undef FINISHED_OBJ_TRACE

using System;
using System.Collections.Generic;
using System.Linq;
using Antlr.Runtime;
using Easis.Wfs.EasyFlow.Model;
using Easis.Wfs.EasyFlow.Utils;


namespace Easis.Wfs.EasyFlow.Parsing
{
    public class Пылесос
    {
        public void УбратьсяВКомнате()
        {
            
        }
    }

    /// <summary>
    /// Парсер языка EasyFlow.
    /// </summary>
    public sealed partial class EasyFlowParser
    {
        private static readonly NLog.Logger Log = NLog.LogManager.GetLogger("EasyFlowParser");

        #region Parsing Constants
        
        private const string RootContextName = "root"; // имя корневого контекста

        private static readonly string[] Keywords = new string[]
                                                             {
                                                                 "flow",
                                                                 "run",
                                                                 "obtain",
                                                                 "file",
                                                                 "require",
                                                                 "step",
                                                                 "as",
                                                                 "after",
                                                                 "for",
                                                                 "if",
                                                                 "result",
                                                                 "on"
                                                             };

        #endregion

        #region Error Constants

        private const string InvalidCompoundNameComponentError = "InvalidCompoundNameComponent";
        private const string KeywordAsIdentifierError = "KeywordAsIdentifierError";

        private DynamicallyIndexedProperty<string, string> _textResDip =
            new DynamicallyIndexedProperty<string, string>(GetTextRes);

        private static readonly Dictionary<string, string> _textRes = new Dictionary<string, string>
                                                                          {
                                                                              {
                                                                                  InvalidCompoundNameComponentError,
                                                                                  "Неверная часть составного имени ({0})."
                                                                                  },
                                                                              {
                                                                                  KeywordAsIdentifierError,
                                                                                  "Попытка использования ключевого слова {0} в качестве идентификатора."
                                                                                  }
                                                                          };
        #endregion

        #region Parser private fields
        
        private Stack<Scope> _scopeStack = new Stack<Scope>(); // стек областей
        private Stack<ParserContext> _contextStack = new Stack<ParserContext>(); // стек контекстов разбора
        private ParseResult _parseResult; // общий результат разбора
        private ParserMessageCollection _parserMessages; // коллекция сообщений парсера
        private ParserContext _rootContext; // корневой контекст разбора
        private Scope _rootScope; // корневая область видимости
        private ParserSettings _settings = new ParserSettings(); // настройки разбора

        #endregion

        /// <summary>
        /// Настройки разбора.
        /// </summary>
        public ParserSettings Settings
        {
            get { return _settings; }
            internal set { _settings = value; }

        }                

        private static string GetTextRes(string key)
        {
            return _textRes[key];
        }

        private DynamicallyIndexedProperty<string, string> TextRes
        {
            get { return _textResDip; }
        }

        private Scope CurrentScope
        {
            get
            {
                return _scopeStack.Peek();   
            }
        }

        /// <summary>
        ///   Событие, происходящее при генерации сообщения парсером.
        /// </summary>
        public event EventHandler<ParserMessageEventArgs> MessageEmited;

        public void InvokeMessageEmited(ParserMessageEventArgs e)
        {
            EventHandler<ParserMessageEventArgs> handler = MessageEmited;
            if (handler != null) handler(this, e);                                    
        }

        /// <summary>
        ///   Перегрузка функции восстановления после ошибки (ANTLR).
        /// </summary>
        public override void Recover(IIntStream input, RecognitionException re)
        {            
            Error(re.Message);
            throw re; // просто перебрасываем исключение, чтобы обработать его снаружи
        }

        /// <summary>
        ///   Перегрузка функции сообщения об ошибке (ANTLR).
        /// </summary>
        /// <param name = "e"></param>
        public override void ReportError(RecognitionException e)
        {
            //errorMessage = GetErrorMessage(e, new string[] { }); // формируем сообщение            
            
            Error(e.Message);
            throw e; // перебрасываем исключение, чтобы обработать его снаружи
        }

        public override string GetErrorMessage(RecognitionException e, string [] tokenNames)
        {
            return e.Message;
        }

        private ParserContext StartRootContext()
        {
            return (_rootContext = StartContext(RootContextName));
        }

        private Scope StartRootScope()
        {
            return (_rootScope = StartScope(ScopeType.Script));
        }

        /// <summary>
        ///   Перегрузка функции восстановления после неверного токена (ANTLR).
        /// </summary>
        /// <param name = "input"></param>
        /// <param name = "e"></param>
        /// <param name = "follow"></param>
        /// <returns></returns>
        public override object RecoverFromMismatchedSet(IIntStream input, RecognitionException e, BitSet follow)
        {
            throw e; // просто перебрасываем исключение, чтобы обработать его снаружи
        }

        private void Message(string message, ParserMessageType type, IParsedObject obj = null)
        {
            _parserMessages.Add(new ParserMessage(message, _scopeStack.Peek().ScopeType, type, obj));

            Log.Trace("Parser message emitted [{0} {1}]: {2}", type.ToString(), (obj != null ? obj.TextRange.Start.ToString() : "(-1, -1)"),  message);
        }

        private void Error(string message, IParsedObject obj = null)
        {
            Message(message, ParserMessageType.Error, obj);
        }

        private void Warning(string message, IParsedObject obj = null)
        {
            Message(message, ParserMessageType.Warning, obj);
        }

        private void Hint(string message, IParsedObject obj = null)
        {
            Message(message, ParserMessageType.Hint, obj);
        }

        private void Info(string message, IParsedObject obj = null)
        {
            Message(message, ParserMessageType.Info, obj);
        }

        private ParserContext StartContext(string ctxName)
        {
            ParserContext ctx = new ParserContext(ctxName);
            Log.Trace("Created parser context: {0} (for scope {1}).", ctx.Name, ctx.RootScope);

            _contextStack.Push(ctx);
            Log.Debug("Started using parser context: {0}.", ctx.Name);

            return ctx;
        }        

        /// <summary>
        /// Инициализирует парсер.
        /// </summary>
        private void Init()
        {
            Log.Debug("Parser initialization...");

            _parseResult = null;
            _parserMessages = new ParserMessageCollection();            

            Log.Debug("Parser initialized.");
        }

        /// <summary>
        ///   Заканчивает работу парсера.
        /// </summary>
        private void Finali()
        {
            int i = 4;
            double d = 3.56;



            Log.Debug("Finishing parser...");

            _parseResult = new ParseResult(new ScriptModel(_rootContext.Flow)
                                               { Requirements = _rootContext.Requirements },
                                           new ParserMessageCollection())
                               {
                                   Succeeded =
                                       _parserMessages.FirstOrDefault(
                                           message => message.MessageType == ParserMessageType.Error) == null
                               };

            Log.Debug("Parsing finished.");
        }

        /// <summary>
        ///   Текущий контекст парсера.
        /// </summary>
        private ParserContext CurrentContext
        {
            get { return _contextStack.Peek(); }
        }

        private ParserContext FinishContext()
        {
            ParserContext ctx = CurrentContext;

            Log.Trace("Finalizing parser context: {0}...", ctx.Name);            

            IEnumerable<TriggerForwardDefinition> forwardDefinitions = ctx.FutureTriggers;

            foreach (var def in forwardDefinitions)
            {                
                Trigger targetTrigger = def.TriggerOwner.Triggers[def.TriggerName];
                StepBlock sourceStep =  ctx.Flow.Blocks.OfType<StepBlock>().
                        FirstOrDefault (block => block.Name.Equals(def.EventSourceStepName));

                if (sourceStep == null)
                {
                    Error(string.Format("Шаг"));
                }

                TriggerEventDef eventDef = new TriggerEventDef(def.EventName, sourceStep, def.IsExplicit);
                FinishObj(eventDef, def);

                targetTrigger.EventExpression.AddEvent(eventDef);

                Log.Trace("Added event for trigger: {0}.{1} on {2}.{3}.", def.TriggerOwner, targetTrigger.ActionDef, sourceStep, def.EventName);
            }

            //////
            // Find data dependencies:
            /////
            
            // Create dictionart: stepName -> stepItSelf
            Dictionary<string, StepBlock> stepNames = ctx.Flow.Blocks.OfType<StepBlock>().ToDictionary(stepBlock => stepBlock.Name);


            // Going through all the steps:
            foreach (var block in ctx.Flow.Blocks)
            {
                List<string> addedDependencies = new List<string>();
                if (block is StepBlock)
                {
                    StepBlock targetStep = (StepBlock) block;

                    // Going through all the parameters:
                    foreach (var param in targetStep.RunParameters.Parameters)
                    {
                        Expression paramExpr = param.Value;

                        // if param is a compound identifier:
                        if (paramExpr is CompoundIdentitfierExpression)
                        {
                            var varIdentifier = (paramExpr as CompoundIdentitfierExpression).Identifier;

                            string sourceEventName;
                            string targetActionName;

                            if (param.IsSubscriptionParam)
                            {
                                sourceEventName = TriggerEventDef.StepRunInfoReadyEventName;
                                targetActionName = TriggerActionDef.ACTION_START;
                            }
                            else
                            {
                                sourceEventName = TriggerEventDef.FinishedEventName;
                                targetActionName = TriggerActionDef.ACTION_START;
                            }


                            // if the first part of the id is a some step's name and it hasn't an indexer:
                            string referredStepName = varIdentifier.NameParts[0].Name;
                            if (stepNames.ContainsKey(referredStepName) &&
                                !varIdentifier.NameParts[0].HasIndexer &&
                                !addedDependencies.Contains(referredStepName))
                            {
                                addedDependencies.Add(referredStepName);

                                // create trigger for it:
                                var eventSourceStep = stepNames[referredStepName];
                                TriggerEventDef newEvent = new TriggerEventDef(sourceEventName, eventSourceStep, false);
                                FinishObj(newEvent, varIdentifier);
                                Trigger eventTargetStep = targetStep.Triggers[targetActionName];

                                eventTargetStep.EventExpression.AddEvent(newEvent); // add new event
                                
                                Log.Trace("Added event for trigger (implicit dependency): perform action '{0}.{1}' on event '{2}'", targetStep, eventTargetStep.ActionDef.Name, newEvent);
                            }
                        }
                    }
                }
            }
            _contextStack.Pop();
            Log.Debug("Stopped using parser context: {0}.", ctx.Name);

            return ctx;
        }

        private Scope StartScope(ScopeType scopeType)
        {
            if (!Settings.CollectScopeInformation)
                return null;

            Scope newScope = new Scope(scopeType);

            _scopeStack.Push(newScope);

#if SCOPE_TRACE
            Log.Trace("Entered code scope: {0}.",scopeType.ToString());
#endif

            return newScope;
        }

        private Scope FinishScope()
        {
            if (!Settings.CollectScopeInformation)
                return null;

            Scope scope = _scopeStack.Pop();

#if SCOPE_TRACE
            Log.Trace("Exited code scope: {0}.", scope);
#endif

            return scope;
        }

        /// <summary>
        ///   Возвращает начальную позицию токена.
        /// </summary>
        /// <param name = "token">Токен.</param>
        /// <returns>Координаты позиции в тексте.</returns>
        private TextPosition GetStart(IToken token)
        {
            return new TextPosition(token.Line, token.CharPositionInLine + 1);
        }

        /// <summary>
        ///   Возвращает конечную позицию токена.
        /// </summary>
        /// <param name = "token">Токен.</param>
        /// <returns>Координаты позиции в тексте.</returns>
        private TextPosition GetEnd(IToken token)
        {
            return new TextPosition(token.Line, token.CharPositionInLine + token.Text.Length);
        }

        private TextRange GetRange(IToken token)
        {
            return new TextRange(GetStart(token), GetEnd(token));
        }

        /// <summary>
        ///   Добавляет блок к текущему
        /// </summary>
        /// <param name = "block"></param>
        private void AddBlock(BlockBase block)
        {
            CurrentContext.Flow.Blocks.Add(block);

            Log.Trace("Block added: {0}.", block);
        }

        private void AddFlowAttribute(NamedParameter namedParameter)
        {
            if (namedParameter == null) throw new ArgumentNullException("namedParameter");
            CurrentContext.Flow.Attributes.Add(namedParameter);
        }

        private void AddRequirement(FileRequirement requirement)
        {
            CurrentContext.Requirements.Add(requirement);

            Log.Trace("Requirement added: {0}.", requirement);
        }

        /// <summary>
        /// Заканчивает заполнение объекта.
        /// </summary>
        /// <param name="parsedObj">Распарсенный объект.</param>
        /// <param name="scope">Область правила.</param>
        /// <param name="complete">Заполнение объекта успешно завершено?</param>
        private void FinishObj(IParsedObject parsedObj, ParserRuleReturnScope scope, bool complete = true)
        {
            if (parsedObj == null || !Settings.CollectTextInformation)
                return;
            
            parsedObj.TextBehind = input.ToString((IToken) scope.Start, input.LT(-1));
            parsedObj.TextRange = new TextRange
                (
                    GetStart((IToken) scope.Start),
                    GetEnd((IToken) scope.Stop)
                );

            parsedObj.IsComplete = complete;
            parsedObj.IsInvalid = false;

#if FINISHED_OBJ_TRACE
            Log.Trace("Finished obj as {0}: {1} = {2}.", complete ? "complete" : "NOT complete", parsedObj.GetType().Name, parsedObj.ToString());
#endif
        }

        /// <summary>
        /// Заканчивает заполнение объекта.
        /// </summary>
        /// <param name="parsedObj">Объект.</param>
        /// <param name="token">Токен, которому соответствует объект.</param>
        /// <param name="invalid">Ошибка?</param>
        /// <param name="complete">Закончен?</param>
        private void FinishObj(IParsedObject parsedObj, IToken token, bool invalid = false, bool complete = true)
        {
            if (parsedObj == null) throw new ArgumentNullException("parsedObj");

            if (Settings.CollectTextInformation)
            {
                parsedObj.TextBehind = token.Text;
                parsedObj.TextRange = GetRange(token);
            }

            parsedObj.IsComplete = complete;
            parsedObj.IsInvalid = invalid;

#if FINISHED_OBJ_TRACE
            Log.Trace("Finished obj as {0}: {1} = {2}.", complete ? "complete" : "NOT complete", parsedObj.GetType().Name, parsedObj.ToString());
#endif
        }

        private void FinishObj(IParsedObject parsedObj, IParsedObject otherObj)
        {
            if (parsedObj == null) throw new ArgumentNullException("parsedObj");

            if (Settings.CollectTextInformation)
            {
                parsedObj.TextBehind = otherObj.TextBehind;
                parsedObj.TextRange = otherObj.TextRange;
            }

            parsedObj.IsComplete = otherObj.IsComplete;
            parsedObj.IsInvalid = otherObj.IsInvalid;

#if FINISHED_OBJ_TRACE
            Log.Trace("Finished obj as {0}: {1} = {2}.", otherObj.IsComplete ? "complete" : "NOT complete", parsedObj.GetType().Name, parsedObj.ToString());
#endif
        }

        private bool IsKeyword(string id)
        {
            return Keywords.Contains(id);
        }

        private void AddFutureTrigger(TriggerForwardDefinition definition)
        {            
            CurrentContext.FutureTriggers.Add(definition);
            Log.Trace("Future trugger added: {0}.{1} on {2}.{3} ({4}).", definition.TriggerOwner.ToString(), definition.TriggerName, definition.EventSourceStepName, definition.EventName, definition.IsExplicit ? "explicit" : "implicit");
        }

        private Identifier VarId(string name)
        {
            return new Identifier(name, IdentifierType.VarName);
        }

        private Identifier StepId(string name)
        {
            return new Identifier(name, IdentifierType.StepName);
        }        

        /// <summary>
        /// Проверяет корректность определяющего появления идентификатора.
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        private bool CheckAddIdentifierDef(Identifier identifier)
        {
            if (CurrentContext.IdentifierTable.HasIdentifier(identifier.Name))
            {
                identifier.IsInvalid = true;
                Error(string.Format("Повторное объявление идентификатора {0}.", identifier.Name), identifier);
                return false;
            }           
            CurrentContext.IdentifierTable.Add(identifier);

            return true;
        }

        private void AddIdentifier(Identifier identifier)
        {
            CurrentContext.IdentifierTable.Add(identifier);
        }
    }

    /// <summary>
    ///   Аргументы событий, связанных с выдачей парсером сообщений.
    /// </summary>
    public class ParserMessageEventArgs: EventArgs
    {
        public ParserMessage Message { get; set; }

        public ParserMessageEventArgs(ParserMessage message)
        {
            Message = message;
        }
    }
}