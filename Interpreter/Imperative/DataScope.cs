using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Easis.Common.Exceptions;
using Easis.Wfs.EasyFlow.Model;
using Easis.Wfs.EasyFlow.Model.Expressions;

namespace Easis.Wfs.Interpreting
{
    public class DataScope : IValueGetterEcx, IValueSetterEcx, IValueGetter
    {
        private bool _isRoot;

        public virtual bool IsRoot
        {
            get { return _isRoot; }
            private set { _isRoot = value; }
        }

        private ICollection<Variable> _variables;
        public virtual ICollection<Variable> Variables
        {
            get { return _variables; }
            private set { _variables = value; }
        }

        private ICollection<DataScope> _childrenScopes;
        public virtual ICollection<DataScope> ChildrenScopes
        {
            get { return _childrenScopes; }
            private set { _childrenScopes = value; }
        }

        private DataScope _parentScope;
        public DataScope ParentScope
        {
            get { return _parentScope; }
        }

        public DataScope(DataScope parent)
        {
            if (parent == null)
                IsRoot = true;

            _parentScope = parent;
            Variables = new List<Variable>();
            ChildrenScopes = new List<DataScope>();

            if(_parentScope != null)
                _parentScope.ChildrenScopes.Add(this);
        }

        public virtual void DefineVariable(Variable variable)
        {
            bool exists = false;

            try
            {
                // определять переменные в этом контексте можно без вопросов к дочерним
                GetValue(new CompoundVarIdentifier(variable.Name));
                exists = true;
            }
            catch (InvalidOperationException e)
            {
                exists = false;
            }

            if (exists)
                // TODO: new Exception
                throw new InterpretionException(String.Format("Duplicate variable name {0}",variable.Name));
            else
            {
                Variables.Add(variable);
            }

        }

        public virtual ValueBase GetValue(CompoundVarIdentifier identifier, IEvaluationContext evaluationContext)
        {
            if (identifier == null) throw new ArgumentNullException("identifier");
            if (identifier.Count() == 0) throw new ArgumentException("identifier is empty");

            // !!! Внимательно !!!
            // Важен порядок поиска переменных в дереве

            string first = identifier.First().Name;

            ValueBase ret = null;

            // Ищем в переменных
            if (ret == null)
            {
                var namedVar = from variable in Variables where variable.Name == first select variable;
                if (namedVar.Count() > 0)
                {
                    System.Diagnostics.Debug.Assert(namedVar.Count() == 1);

                    // есть переменная с таким именем
                    ret = namedVar.First().GetValue(identifier,evaluationContext);
                }
            }
            
            // Ищем у родителя
            if (ret == null && !IsRoot)
            {
                // пока что без кэширования глобальных переменных
                ret = ParentScope.GetValue(identifier,evaluationContext);
            }

            if (ret != null) return ret;
            else throw new InvalidOperationException(String.Format("Undefined variable {0}", identifier.TextBehind));
        }

        /// <summary>
        /// Установка значения существующей переменной
        /// throws InvalidOperationException
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="value"></param>
        public virtual void SetValue(CompoundVarIdentifier identifier, ValueBase value, IEvaluationContext evaluationContext)
        {
            //Важно. При изменении - изменить BlockDataScope.setVar
            // Он отличается только тем, что не ищет у родителя.

            if (identifier == null) throw new ArgumentNullException("identifier");
            if (identifier.Count() == 0) throw new ArgumentException("identifier is empty");

            string first = identifier.First().Name;

            bool ret = false;

            // Ищем в переменных
            if (!ret)
            {
                var namedVar = from variable in Variables where variable.Name == first select variable;
                if (namedVar.Count() > 0)
                {
                    System.Diagnostics.Debug.Assert(namedVar.Count() == 1);

                    // есть переменная с таким именем/ привсаиваем новое значение
                    namedVar.First().SetValue(identifier, value,evaluationContext);

                    ret = true;
                }
            }

            // Ищем у родителя
            if (!ret && !IsRoot)
            {
                ParentScope.SetValue(identifier,value,evaluationContext);
                ret = true;
            }

            if (!ret) throw new InvalidOperationException(String.Format("Undefined variable {0}", identifier.TextBehind));
        }

        /// <summary>
        /// Класс описания найденной переменной с помощью метода FindVariable.
        /// </summary>
        public sealed class VariableDescriptor
        {
            private string _name;
            public string Name
            {
                get { return _name; }
                private set { _name = value; }
            }

            private VariableAccessMode _accessMode;
            public VariableAccessMode AccessMode
            {
                get { return _accessMode; }
                private set { _accessMode = value; }
            }

            private DataType _dataType;
            public DataType DataType
            {
                get { return _dataType; }
                private set { _dataType = value; }
            }

            private VariableModifier _modifier;
            public VariableModifier Modifier
            {
                get { return _modifier; }
                private set { _modifier = value; }
            }

            public VariableDescriptor(Variable variable, VarScopeLocation location)
            {
                Name = variable.Name;
                Variable = variable;
            }

            private VarScopeLocation _location;
            public VarScopeLocation Location
            {
                get { return _location; }
                set { _location = value; }
            }


            private Variable _variable;
            public Variable Variable
            {
                get { return _variable; }
                set { _variable = value; }
            }
        }

        /// <summary>
        /// Где найдена переменная
        /// </summary>
        public enum VarScopeLocation
        {
            CurrentBlockScope,
            GlobalScope,
            AnotherBlockScope
        }

        //TODO: протестить
        /// <summary>
        /// Поиск переменной
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public virtual VariableDescriptor FindVariable (CompoundVarIdentifier identifier)
        {
            string first = identifier.First().Name;

            VariableDescriptor ret = null;

            // Ищем в переменных
            if (ret == null)
            {
                var namedVar = from variable in Variables where variable.Name == first select variable;
                if (namedVar.Count() > 0)
                {
                    System.Diagnostics.Debug.Assert(namedVar.Count() == 1);

                    // есть переменная с таким именем
                    ret = new VariableDescriptor(namedVar.First(), VarScopeLocation.CurrentBlockScope);
                }
            }

            // Ищем у родителя
            if (ret == null && !IsRoot)
            {
                // пока что без кэширования глобальных переменных
                // Если не найдется, то выкинется исключение, которое пробросится наружу

                try
                {
                    ret = ParentScope.FindVariable(identifier);
                    // в DataScope ничего не делаем с location
                    switch (ret.Location)
                    {
                        case VarScopeLocation.CurrentBlockScope:
                            break;
                        case VarScopeLocation.GlobalScope:
                            break;
                        case VarScopeLocation.AnotherBlockScope:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                catch (ObjectNotFoundException e)
                {
                    ret = null;
                }
            }

            if (ret != null) return ret;
            else throw new ObjectNotFoundException();;
        }

        // только для current context
        // TODO: проверить отсутствие рекурсии
        public virtual ValueBase GetValue(CompoundVarIdentifier identifier)
        {
            IEvaluationContext evaluationContext = new SimpleEvaluationContext(this);
            return GetValue(identifier, evaluationContext);
        }
    }
}
