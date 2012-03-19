using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Easis.Wfs.EasyFlow.Model.Expressions
{
    public class HashDict : Dictionary<ValueBase, ValueBase>
    {
        public HashDict()
        {
        }

        public HashDict(int capacity)
            : base(capacity)
        {
        }

        public HashDict(IEqualityComparer<ValueBase> comparer)
            : base(comparer)
        {
        }

        public HashDict(IDictionary<ValueBase, ValueBase> dictionary)
            : base(dictionary)
        {
        }
    }

    public class HashValue : ValueBase
    {
        public HashDict AsHash
        {
            get
            {
                return Value as HashDict;
            }
        }

        public HashValue(HashDict dict)
            : base(new HashDict(dict))
        {
            DataType = DataType.Hash;
        }

        public HashValue()
            : base(new HashDict())
        {
            DataType = DataType.Hash;
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
                case DataType.Hash:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Приводит данное значение к переданному типу.
        /// </summary>
        /// <param name="dataType">Тип, к которому осуществляется приведение.</param>
        /// <returns>Приведённое значение.</returns>
        public override ValueBase CastTo(DataType dataType)
        {
            switch (dataType)
            {
                case DataType.Hash:
                    return new HashValue(AsHash);
                default:
                    throw new NotSupportedException(String.Format("Can't cast Int to {0}.", dataType));
            }
        }

        protected override object CreateClone()
        {
            return new HashValue();
        }

        public override ValueBase GetValue(CompoundVarIdentifier identifier, IEvaluationContext evaluationContext)
        {
            if (identifier == null) throw new ArgumentNullException("identifier");

            if (identifier.NameParts.Count == 0)
            {
                throw new Exception("If you see this please check getvalue logic of hash.");
            }

            var first = identifier.NameParts[0];

            if (first.HasIndexer)
            {
                ValueBase key = first.Indexer.Index.Evaluate(evaluationContext);
                if (AsHash.ContainsKey(key))
                    // TODO: check! !!!!!!!!!!!!!!!!!!!!!!
                    // create temporal fake compound for evaluated value
                    //return AsHash[key].GetValue(CompoundVarIdentifier.ComposeVarIdentifier(new CompoundVarIdentifier("_"),identifier.SubIdentifier(1)), evaluationContext);
                    return AsHash[key];
                else
                    throw new InvalidOperationException(String.Format("Hash {0} doesn't contain key {1}",first.Name,key));
            }
            else
            {
                if(identifier.NameParts.Count == 1)
                    return this;
                else
                {
                    throw new InvalidOperationException(String.Format("Using hash {0} as structure", first.Name));
                }
            }
        }

        public override void SetValue(CompoundVarIdentifier identifier, ValueBase value, IEvaluationContext evaluationContext)
        {
            if (identifier == null) throw new ArgumentNullException("identifier");
            if (value == null) throw new ArgumentNullException("value");

            if (identifier.NameParts.Count == 0)
            {
                throw new Exception("If you see this please check getvalue logic of hash.");
            }

            var first = identifier.NameParts[0];

            if (first.HasIndexer)
            {
                ValueBase key = first.Indexer.Index.Evaluate(evaluationContext);
                if (AsHash.ContainsKey(key))
                {
                    if (identifier.NameParts.Count == 1)
                    {
                        AsHash[key] = value;
                    }
                    else
                    {
                        // TODO: check! аккуратно с _
                        // create temporal fake compound for evaluated value
                        AsHash[key].SetValue(CompoundVarIdentifier.Concat(new CompoundVarIdentifier("_"), identifier.SubIdentifier(1)), value, evaluationContext);
                    }
                }
                else
                    AsHash.Add(key,value);

            }
            else
            {
                if (identifier.NameParts.Count == 1)
                    throw new InvalidOperationException(String.Format("Using value {0} as a reference type while setting new value", DataType));
                else
                {
                    throw new InvalidOperationException(String.Format("Using hash {0} as structure", first.Name));
                }
            }
        }
    }
}
