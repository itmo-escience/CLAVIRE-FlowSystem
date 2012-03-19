using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Easis.Common.Exceptions;

namespace Easis.Wfs.EasyFlow.Model.Expressions
{

    //---- DRYRUN ----
    public class UndefinedValue : ValueBase
    {
        public override string ToString()
        {
            return "undefined";
        }

        public override bool CanCastTo(DataType dataType)
        {
            return false;
        }

        public UndefinedValue()
        {
            DataType = DataType.Undefined;
        }

        public override bool Equals(object obj)
        {
            return false;
        }

        public override ValueBase GetValue(CompoundVarIdentifier identifier, IEvaluationContext evaluationContext)
        {
            return new UndefinedValue();
        }

        public override ValueBase CastTo(DataType dataType)
        {
            throw new InvalidFormatException("Undefined value cannot be casted to any type");
        }

        protected override object CreateClone()
        {
            return new UndefinedValue();
        }
    }
}
