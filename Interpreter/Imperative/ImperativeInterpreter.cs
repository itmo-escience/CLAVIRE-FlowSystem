using System;
using System.Collections.Generic;
using Easis.Common.Exceptions;
using Easis.Wfs.EasyFlow.Model;


namespace Easis.Wfs.Interpreting
{
    /// <summary>
    /// Интерпретатор императивного кода. Свой для каждого степа.
    /// not ts
    /// </summary>
    public class ImperativeInterpreter : ICodeInterpreter, IValueSetter
    {
        private readonly BlockDataScope _blockDataScope;
        private readonly GlobalDataScope _globalDataScope;
        protected DataScope CurrentDataScope = null;

        
        /// <summary>
        /// Контекст интерпретации
        /// </summary>
        protected IEvaluationContext EvaluationContext
        {
            get { return new SimpleEvaluationContext(CurrentDataScope); }
        }

        /// <summary>
        /// Делает запрос глобальному скопу на создание структуры с именем блока.
        /// </summary>
        /// <param name="globalDataScope"></param>
        public ImperativeInterpreter(GlobalDataScope globalDataScope,string blockName = null)
        {
            _globalDataScope = globalDataScope;
            _blockDataScope = new BlockDataScope(globalDataScope,blockName);
            CurrentDataScope = _blockDataScope;

            if(blockName != null)
                // создаем переменную, соответствующую шагу в глобальном контексте
                _globalDataScope.CreateNamedBlockStructure(_blockDataScope.Name);
        }

        public GlobalDataScope GetGlobalDataScope()
        {
            return _globalDataScope;
        }
        public BlockDataScope GetBlockDataScope()
        {
            return _blockDataScope;
        }

        public void InterpreteCodeSection(CodeSection codeSection)
        {
            throw new NotImplementedException();
        }

        public ValueBase EvaluateExpression(Expression expr)
        {
            return expr.Evaluate(EvaluationContext);
        }

        public IDictionary<string, ValueBase> InterpreteRunParameters(RunParameters runParameters)
        {
            IDictionary<string, ValueBase> ret = new Dictionary<string, ValueBase>();
            
            foreach (var param in runParameters.Parameters)
            {
                if (param.IsSubscriptionParam)
                {
                    // TODO: make compound string
                    ret.Add(param.Name, new StringValue(((CompoundIdentitfierExpression)param.Value).Identifier.ToString()));
                }
                else
                {
                    //TODO: Check Exception
                    ret.Add(param.Name, param.Value.Evaluate(EvaluationContext));
                }
            }

            return ret;
        }

        // TODO: сделать копирование переменной
        public void ShareVariables(IEnumerable<Variable> variables)
        {
            _globalDataScope.ShareVariables(_blockDataScope.Name, variables);
        }

        // Helper
        public void ShareVariable(Variable variable)
        {
            ICollection<Variable> vs = new List<Variable>();
            vs.Add(variable);
            ShareVariables(vs);
        }

        public void ShareVariables(IEnumerable<string> identifier)
        {
            ICollection<Variable> vars = new List<Variable>();
            
            foreach (var ids in identifier)
            {
                DataScope.VariableDescriptor vd = CurrentDataScope.FindVariable(new CompoundVarIdentifier(ids));
                vars.Add(vd.Variable);
            }

            ShareVariables(vars);
        }

        public void ShareVariable(string identifier)
        {
            ICollection<string> vs = new List<string>();
            vs.Add(identifier);
            ShareVariables(vs);
        }

        /// <summary>
        /// В отличие от SetValue гарантируется что переменной не было до этого
        /// </summary>
        /// <param name="variable"></param>
        public void DefineVariable(Variable variable)
        {
            DataScope.VariableDescriptor vd;

            bool exists = false;

            try
            {
                vd = CurrentDataScope.FindVariable(new CompoundVarIdentifier(variable.Name));
                if(vd.Location == DataScope.VarScopeLocation.CurrentBlockScope)
                    exists = true;
            }
            catch (ObjectNotFoundException e)
            {
                exists = false;

            }

            if (exists)
            {
                throw new InterpretionException(String.Format("Duplicate definition of variable {0}", variable.Name));
            }
            else
            {
                CurrentDataScope.DefineVariable(variable);
            }
        }

        private class SimpleEvaluationContext : IEvaluationContext
        {
            private readonly IValueGetter _valueGetter;
            public SimpleEvaluationContext(IValueGetter valueGetter)
            {
                _valueGetter = valueGetter;
            }

            public IValueGetter ValueGetter
            {
                get { return _valueGetter; }
            }
        }

        /// <summary>
        /// Новая переменная либо изменение значения старой.
        /// Оператор присваивания.
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="value"></param>
        public void SetValue(CompoundVarIdentifier identifier, ValueBase value)
        {
            if (identifier == null) throw new ArgumentNullException("identifier");
            if (value == null) throw new ArgumentNullException("value");

            if (identifier.NameParts.Count == 0) throw new ArgumentNullException("identifier");

            // ищем, есть ли такая переменная
            DataScope.VariableDescriptor vd;

            try
            {
                vd = CurrentDataScope.FindVariable(identifier);

                // проверяем, можем ли ее переопределить

                // глобальная?
                if (vd.Location != DataScope.VarScopeLocation.CurrentBlockScope)
                {
                    throw new InterpretionException("Unable write to external variable.");
                }

                // TODO: раскомментировать !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                // readonly?
                //if (vd.Modifier == VariableModifier.Readonly)
                //{
                //    throw new InterpretionException("Unable write to readonly variable.");
                //}

                // переустанавливаем значение
                CurrentDataScope.SetValue(identifier, value, EvaluationContext);

            }
            catch (ObjectNotFoundException e)
            {
                // создаем новую переменную

                // если указано составное имя - исключение
                if(identifier.NameParts.Count > 1) throw new InterpretionException("Unable to set field to undefined structure");

                string first = identifier.NameParts[0].Name;

                Variable newVar = new Variable(first,value);

                newVar.AccessMode = VariableAccessMode.Private;
                newVar.Modifier = VariableModifier.Simple;

                CurrentDataScope.DefineVariable(newVar);
            }
        }

        public ValueBase GetValue(CompoundVarIdentifier identifier)
        {
            return CurrentDataScope.GetValue(identifier);
        }
    }
}
