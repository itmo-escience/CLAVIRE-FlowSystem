using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easis.Wfs.EasyFlow.Model.Expressions
{
    public class PlusOperatorExpression : ArithmeticOperatorExpression
    {
        public PlusOperatorExpression(Expression left, Expression right): base(left, right)
        {
        }

        protected override ValueBase ApplyOperator(ValueBase leftValue, ValueBase rightValue)
        {
            if (leftValue.DataType == DataType.Int && rightValue.DataType == DataType.Int)
                return new IntValue(((IntValue)leftValue).AsInt + ((IntValue)rightValue).AsInt);

            if (leftValue.DataType == DataType.Double && rightValue.DataType == DataType.Double)
                return new DoubleValue(((DoubleValue)leftValue).AsDouble + ((DoubleValue)rightValue).AsDouble);

            throw new Exception(); // TODO: real exception
        }

        protected override string OperationSign
        {
            get { return "+"; }
        }

        protected override object CreateClone()
        {
            return new PlusOperatorExpression((Expression)Left.Clone(), (Expression)Right.Clone());
        }

    }
}
