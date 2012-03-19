using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Easis.Wfs.EasyFlow.Model.Expressions;
using NLog;

namespace Easis.Wfs.EasyFlow.Model
{
    public class ListValue : ValueBase
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();



        /// <summary>
        /// Magic indexing behavior for lists
        /// to remove this just delete override
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="evaluationContext"></param>
        /// <returns></returns>
        public override ValueBase GetValue(CompoundVarIdentifier identifier, IEvaluationContext evaluationContext)
        {
            if (identifier == null) throw new ArgumentNullException("identifier");

            if (identifier.NameParts.Count == 0)
            {
                throw new Exception("If you see this please check getvalue logic of list.");
            }

            var first = identifier.NameParts[0];

            if (first.HasIndexer)
            {
                _log.Trace("Found indexer in list. Probably it is filter. Trying to evaluate...");
                ListValue ret = new ListValue(new List<Expression>());

                ValueBase key = first.Indexer.Index.Evaluate(evaluationContext);

                foreach (var expression in (List<Expression>)Value)
                {
                    ValueBase vb = expression.Evaluate(evaluationContext);
                    if(vb is HashValue)
                    {
                        HashDict hd = ((HashValue) vb).AsHash;
                        if(hd.ContainsKey(key))
                        {
                            ret.AsList.Add(new LiteralExpression(hd[key]));
                        }
                    }
                }

                return ret;
            }
            else
            {
                if (identifier.NameParts.Count == 1)
                    return this;
                else
                {
                    throw new InvalidOperationException(String.Format("Using hash {0} as structure", first.Name));
                }
            }
        }

        public IList<Expression> AsList
        {
            get
            {
                return Value as List<Expression>;
            }
        }

        public ListValue(IEnumerable<Expression> collection)
            : base(new List<Expression>(collection))
        {
            DataType = DataType.List;
        }

        public ListValue()
        {
            DataType = DataType.List;
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
                case DataType.List:
                    return true;
                default:
                    return false;
            }
        }

        //private HashDict MakeHash()
        //{
        //    HashDict dict = new HashDict();

        //    int i = 0;
        //    foreach (Expression value in AsList)
        //        dict.Add(new IntValue(i), value);

        //    return dict;
        //}

        /// <summary>
        /// Приводит данное значение к переданному типу.
        /// </summary>
        /// <param name="dataType">Тип, к которому осуществляется приведение.</param>
        /// <returns>Приведённое значение.</returns>
        public override ValueBase CastTo(DataType dataType)
        {
            switch (dataType)
            {
                //case DataType.Hash:
                //    return new HashValue(MakeHash());
                case DataType.List:
                    return new ListValue(AsList);
                default:
                    throw new NotSupportedException(String.Format("Can't cast Int to {0}.", dataType));
            }
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
            StringBuilder builder = new StringBuilder();

            builder.Append("[");

            bool first = true;

            foreach (var expression in AsList)
            {
                if (first)
                    first = false;
                else
                {
                    builder.Append(", ");
                }
                builder.Append(expression.ToString());
                
            }

            builder.Append("]");

            return builder.ToString();
        }

        protected override object CreateClone()
        {
            return new ListValue();
        }

        protected override object CloneValue()
        {
            return AsList.Select(value => (Expression)value.Clone());
        }
    }
}
