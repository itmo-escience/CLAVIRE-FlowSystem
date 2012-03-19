using System;
using NLog;

namespace Easis.Wfs.EasyFlow.Model
{
    public class CompoundIdentitfierExpression: Expression
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();
        private CompoundVarIdentifier _identifier;

        public CompoundIdentitfierExpression(CompoundVarIdentifier identifier)
        {
            if (identifier == null) throw new ArgumentNullException("identifier");
            _identifier = identifier;
        }

        public CompoundVarIdentifier Identifier
        {
            get { return _identifier; }
        }

        public override ValueBase Evaluate(IEvaluationContext ctx)
        {
            if (ctx == null) throw new ArgumentNullException("ctx");

            _log.Trace("Compound identifier evaluation for '{0}'", _identifier.ToString());

            return ctx.ValueGetter.GetValue(_identifier);
        }

        public override string ToString()
        {
            return _identifier.ToString();
        }

        protected override object CreateClone()
        {
            return new CompoundIdentitfierExpression((CompoundVarIdentifier) Identifier.Clone());
        }
    }
}