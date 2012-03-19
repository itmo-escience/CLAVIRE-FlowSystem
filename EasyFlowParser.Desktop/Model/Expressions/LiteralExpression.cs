using System;

namespace Easis.Wfs.EasyFlow.Model
{
    ///<summary>
    /// Class representing literal expression.
    ///</summary>
    public class LiteralExpression: Expression
    {
        private ValueBase _value;

        /// <summary>
        /// Gets or sets literal value.
        /// </summary>
        public ValueBase Value
        {
            get { return _value; }
        }

        ///<summary>
        /// 
        ///</summary>
        ///<param name="value"></param>
        ///<exception cref="ArgumentNullException"></exception>
        public LiteralExpression(ValueBase value)
        {
            if (value == null) throw new ArgumentNullException("value");
            _value = value;
        }

        public override ValueBase Evaluate(IEvaluationContext ctx)
        {            
            return _value;
        }

        public override string ToString()
        {
            return string.Format("{0}", _value);
        }

        protected override object CreateClone()
        {
            return new LiteralExpression((ValueBase) Value.Clone());
        }
    }
}