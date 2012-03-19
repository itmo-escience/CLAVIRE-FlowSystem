using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Easis.Wfs.EasyFlow.Model.Expressions;

namespace Easis.Wfs.EasyFlow.Model
{
    /// <summary>
    /// Класс, хранящий целое значение.
    /// </summary>
    public class IntValue : ValueBase
    {
        public IntValue(int value) : base(value)
        {
            DataType =  DataType.Int;
        }

        public IntValue(short val)
            : this((int)val)
        {
        }

        public IntValue(Int64 val)
            : this((int)val)
        {
        }

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public IntValue() : this (0)
        {
        }

        /// <summary>
        /// Возвращает значение в виде целого.
        /// </summary>
        public int AsInt
        {
            get
            {              
                return (int)Value;
            }
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
                case DataType.Double:
                case DataType.Int:
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
                case DataType.Double:
                    return new DoubleValue(AsInt);                    
                case DataType.Int:
                    return new IntValue(AsInt);
                case DataType.String:
                    return new StringValue(AsInt.ToString());
                default:
                    throw new NotSupportedException(String.Format("Can't cast Int to {0}.", dataType));
            }
        }

        protected override object CreateClone()
        {
            return new IntValue();
        }
    }
}
