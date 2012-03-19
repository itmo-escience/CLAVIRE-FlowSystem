using System;
using Easis.Common;

namespace Easis.Wfs.EasyFlow.Model
{
    /// <summary>
    /// Класс узла дерева выражений для бинарного оператора.
    /// </summary>
    public abstract class BinaryOpeartorExpression: Expression
    {
        private Expression _left;
        private Expression _right;

        /// <summary>
        /// Левый операнд.
        /// </summary>
        public Expression Left
        {
            get { return _left; }
        }

        /// <summary>
        /// Правый операнд.
        /// </summary>
        public Expression Right
        {
            get { return _right; }
        }

        public class ValuePair
        {
            public ValueBase Left { get; set; }
            public ValueBase Right { get; set; }

            public ValuePair(ValueBase left, ValueBase right)
            {
                Left = left;
                Right = right;
            }
        }

        public override ValueBase Evaluate(IEvaluationContext ctx)
        {
            ValueBase leftValue = Left.Evaluate(ctx);
            ValueBase rightValue = Right.Evaluate(ctx);            

            ValuePair castedValues;

            if (!TryCast(new ValuePair(leftValue, rightValue), out castedValues))
                throw new Exception(); // TODO: real exception

            return ApplyOperator(castedValues.Left, castedValues.Right);
        }

        protected abstract bool TryCast(ValuePair source, out ValuePair casted);

        protected abstract ValueBase ApplyOperator(ValueBase leftValue, ValueBase rightValue);


        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="left">Левый операнд.</param>
        /// <param name="right">Правый операнд.</param>
        protected BinaryOpeartorExpression(Expression left, Expression right)
        {
            if (left == null) throw new ArgumentNullException("left");
            if (right == null) throw new ArgumentNullException("right");

            _left = left;
            _right = right;
        }

        protected abstract string OperationSign
        {
            get;
        }

        public override string ToString()
        {
            return String.Format("{0} {1} {2}", _left, OperationSign, _right);
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override object Clone()
        {
            var clone = (BinaryOpeartorExpression)base.Clone();

            clone._left = (Expression) _left.Clone();
            clone._right = (Expression)_right.Clone();

            return clone;
        }
    }
}