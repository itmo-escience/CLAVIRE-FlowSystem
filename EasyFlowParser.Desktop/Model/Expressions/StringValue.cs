using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Easis.Wfs.EasyFlow.Model.Expressions;

namespace Easis.Wfs.EasyFlow.Model
{
    /// <summary>
    /// Класс, хранящий строковое значение.
    /// </summary>
    public class StringValue : ValueBase
    {    
        internal static StringValue CreateTrimmed(string str)
        {
            return new StringValue(str.Trim('"').Replace("\\\"", "\"")
                .Replace("\\\"", "\"")
                .Replace("\\\'", "\'")
                .Replace("\\\n", "\n")
                .Replace("\\\r", "\r")
                .Replace("\\\t", "\t")
                .Replace("\\\b", "\b")
                .Replace("\\\f", "\f")
                .Replace("\\\\", "\\"));
        }

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public StringValue() : this("")
        {
        }

        public StringValue(string val): base(val)
        {
            DataType = DataType.String;
        }        

        public string AsString
        {
            get
            {
                return Value as string;
            }

        }

        public override string ToString()
        {
            return string.Format("\"{0}\"", AsString);
        }

        /// <summary>
        /// Возвращает, может ли данное значение быть приведено к переданному типу.
        /// </summary>
        /// <param name="dataType">Тип.</param>
        /// <returns>Может / не может.</returns>
        public override bool CanCastTo(DataType dataType)
        {
            switch (dataType)
            {                
                case DataType.String:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Приводит данное значение к переданному типу.
        /// </summary>
        /// <param name="dataType">Тип, к которому осуществляется приведение.</param>
        /// <returns>Приведённое значение.</returns>
        public override ValueBase CastTo(DataType dataType)
        {
            switch (dataType)
            {             
                case DataType.String:
                    return new StringValue(AsString);
                default:
                    throw new NotSupportedException(String.Format("Can't cast Int to {0}.", dataType));
            }
        }

        protected override object CreateClone()
        {
            return new StringValue(AsString);
        }
    }
}
