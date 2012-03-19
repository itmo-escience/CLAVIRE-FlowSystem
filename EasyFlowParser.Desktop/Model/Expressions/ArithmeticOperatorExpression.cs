namespace Easis.Wfs.EasyFlow.Model
{    
    /// <summary>
    /// Класс узла дерева выражений для арифметического оператора.
    /// </summary>
    public abstract class ArithmeticOperatorExpression : BinaryOpeartorExpression
    {        
        private bool TryCastRightToLeft(ValuePair source, out ValuePair casted)
        {
            ValueBase left = source.Left;
            ValueBase right = source.Right;

            casted = null;

            switch (left.DataType)
            {
                case DataType.Double:
                    switch (right.DataType)
                    {
                        case DataType.Double:
                            break;
                        case DataType.Int:
                            right = new DoubleValue(((IntValue)right).AsInt);
                            break;
                        default:
                            return false;
                    }
                    break;
                case DataType.Int:
                    switch (right.DataType)
                    {
                        case DataType.Int:
                            break;
                        default:
                            return false;
                    }
                    break;

            }

            casted = new ValuePair(left, right);

            return true;
        }

        protected override bool TryCast(ValuePair source, out ValuePair casted)
        {
            if (!TryCastRightToLeft(source, out casted))
                if (!TryCastRightToLeft(new ValuePair(source.Right, source.Left), out casted))
                    return false;

            return true;
        }

        protected ArithmeticOperatorExpression(Expression left, Expression right): base(left, right)
        {
        }
    }
}
