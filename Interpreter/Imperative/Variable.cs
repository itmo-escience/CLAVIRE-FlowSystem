using System;
using Easis.Wfs.EasyFlow.Model;
using Easis.Wfs.EasyFlow.Model.Expressions;

namespace Easis.Wfs.Interpreting
{
    public enum VariableAccessMode
    {
        Public,
        Private
    }

    public enum VariableModifier
    {
        Readonly,
        Simple
    }

    public class Variable : IValueGetterEcx, IValueSetterEcx
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private VariableAccessMode _accessMode;
        public VariableAccessMode AccessMode
        {
            get { return _accessMode; }
            set { _accessMode = value; }
        }

        public DataType DataType
        {
            get { return Value.DataType; }
        }

        private VariableModifier _modifier;
        public VariableModifier Modifier
        {
            get { return _modifier; }
            set { _modifier = value; }
        }

        private ValueBase _value;
        public ValueBase Value
        {
            get { return _value; }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                _value = value;
            }
        }

        public Variable(string name, ValueBase value)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (value == null) throw new ArgumentNullException("value");

            _name = name;
            _value = value;
        }

        /// <summary>
        /// Для простых типов возможен вызов только с пустым идентификатором.
        /// Для составных типов.
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public ValueBase GetValue(CompoundVarIdentifier identifier,IEvaluationContext evaluationContext)
        {
            if (identifier == null) throw new ArgumentNullException("identifier");

            // если список пустой - отдаем свое значение
            if (identifier.NameParts.Count == 0)
            {
                throw new InvalidOperationException("Internal program logic error. Check setvalue logic of variable.");
            }

            // если список пустой - отдаем свое значение
            if (identifier.NameParts.Count == 1)
            {
                return Value;
            }

            // Если тип не комплексный - вернет исключение
            return Value.GetValue(identifier,evaluationContext);
        }

        public void SetValue(CompoundVarIdentifier identifier, ValueBase value,IEvaluationContext evaluationContext)
        {
            if (identifier == null) throw new ArgumentNullException("identifier");
            if (value == null) throw new ArgumentNullException("value");

            if (identifier.NameParts.Count == 0)
            {
                throw new InvalidOperationException("Internal program logic error. Check setvalue logic of variable.");
            }

            // если список пустой - привсваиваем значение себе
            if (identifier.NameParts.Count == 1)
            {
                Value = value;
                return;
            }

            // Если тип не комплексный - вернет исключение
            Value.SetValue(identifier, value,evaluationContext);
            return;
        }
    }
}
