using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Easis.Wfs.EasyFlow.Model;

namespace Easis.Wfs.Interpreting
{
    public class SimpleEvaluationContext : IEvaluationContext
    {
        private readonly IValueGetter _valueGetter;
        public SimpleEvaluationContext(IValueGetter valueGetter)
        {
            _valueGetter = valueGetter;
        }

        public IValueGetter ValueGetter
        {
            get { return _valueGetter; }
        }
    }
}
