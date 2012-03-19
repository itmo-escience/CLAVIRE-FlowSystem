using System;
using System.Collections.Generic;
using Easis.Wfs.EasyFlow.Model.Expressions;

namespace Easis.Wfs.EasyFlow.Model
{
    public class ConstValue : ValueBase
    {
        private string _constName;

        public ConstValue(string constName): base(constName)
        {
            _constName = constName;
        }

        public string AsConstName
        {
            get { return Value as string; }
        }

        /// <summary>
        /// Returns whether this value can be cast to the passed data type.
        /// </summary>
        /// <param name="dataType">Data type to check.</param>
        /// <returns>Can / can't.</returns>
        public override bool CanCastTo(DataType dataType)
        {
            return false;
        }

        /// <summary>
        /// Приводит данное значение к переданному типу.
        /// </summary>
        /// <param name="dataType">Тип, к которому осуществляется приведение.</param>
        /// <returns>Приведённое значение.</returns>
        public override ValueBase CastTo(DataType dataType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Интерфейс получения значения.
        ///  Соглашение следующее. В метод приходит compound "A.B", где A - это this.
        ///  throws InvalidOperationException
        /// </summary>
        /// <param name="identifier"/><param name="evaluationContext"/>
        /// <returns/>
        public override ValueBase GetValue(CompoundVarIdentifier identifier, IEvaluationContext evaluationContext)
        {
            // TODO: fix this
            if (ConstHolder.HasConst(_constName))
                return ConstHolder.GetConst(_constName);

            throw new KeyNotFoundException(string.Format("Constant '{0}' not found.", _constName));
        }

        /// <summary>
        /// Gets value.
        /// </summary>
        public override object Value
        {
            get { return GetValue(null, null) ; }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return "@" + base.ToString();
        }

        protected override object CreateClone()
        {
            return new ConstValue(_constName);
        }
    }
}
