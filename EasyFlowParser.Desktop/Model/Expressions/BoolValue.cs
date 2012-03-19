using System;

namespace Easis.Wfs.EasyFlow.Model
{
    public class BoolValue: ValueBase
    {
        public BoolValue(bool val): base(val)
        {
        }

        public override bool CanCastTo(DataType dataType)
        {
            return dataType == DataType.Bool;
        }

        public override ValueBase CastTo(DataType dataType)
        {
            return (BoolValue) this.Clone();
        }

        public bool AsBool
        {
            get
            {
                return (bool)Value;
            }
        }

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public BoolValue() : this (false)
        {
        }

        protected override object CreateClone()
        {
            return new BoolValue();
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
            return base.ToString().ToLowerInvariant();
        }
    }
}