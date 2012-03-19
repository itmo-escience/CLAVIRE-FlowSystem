using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Easis.Wfs.EasyFlow.Model.Expressions;

namespace Easis.Wfs.EasyFlow.Model
{
    /// <summary>
    /// Дробное значение.
    /// </summary>
    public class DoubleValue : ValueBase
    {
        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public DoubleValue() : base((double)0.0)
        {          
        }

        /// <summary>
        /// Конструктор с float.
        /// </summary>
        /// <param name="val">Значение float.</param>
        public DoubleValue(float val) : this((double)val) { }

        /// <summary>
        /// Конструктор с double.
        /// </summary>
        /// <param name="val">Значение double.</param>
        public DoubleValue(double val): base(val)
        {
            DataType = DataType.Double;
        }

        /// <summary>
        /// Возвращает значение в виде Double.
        /// </summary>
        public double AsDouble
        {
            get
            {              
                return (double)Value;
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
                    return new DoubleValue(AsDouble);
               /* case DataType.Int:
                    return new IntValue((int)AsDouble); // TODO: check whether it's true*/
                case DataType.String:
                    return new StringValue(AsDouble.ToString());
                default:
                    throw new NotSupportedException(String.Format("Can't cast Int to {0}.", dataType));
            }
        }

        protected override object CreateClone()
        {
            return new DoubleValue();
        }
    }
}
