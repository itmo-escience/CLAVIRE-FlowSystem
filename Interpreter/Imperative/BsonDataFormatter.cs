using System;
using System.Collections.Generic;
using Easis.Wfs.EasyFlow.Model;
using Easis.Wfs.EasyFlow.Model.Expressions;
using MongoDB.Bson;
using NLog;

namespace Easis.Wfs.Interpreting.Imperative
{
    class BsonDataFormatter
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public ValueBase ConvertValueFromBson(BsonValue val)
        {
            if (val == null) throw new ArgumentNullException("val");

            if (val is BsonBoolean)
            {
                return new BoolValue((val).AsBoolean);
            }
            else if (val is BsonInt32)
            {
                return new IntValue(val.AsInt32);
            }
            else if (val is BsonInt64)
            {
                return new IntValue(val.AsInt64);
            }
            else if (val is BsonString)
            {
                return new StringValue(val.AsString);
            }
            else if (val is BsonDouble)
            {
                return new DoubleValue(val.AsDouble);
            }
            else if (val is BsonDocument)
            {

                // complex type
                if (val.AsBsonDocument.Contains("_t"))
                {
                    if (val.AsBsonDocument["_t"].AsString == "ExternalFileDef")
                    {
                        return
                            new FileValue(new FileDescriptor(null, val.AsBsonDocument["FileName"].AsString,
                                                             val.AsBsonDocument["Locator"].AsString));
                    }
                    else
                    {
                        Log.Warn("Unknown type '{0}' using as hash.", val.AsBsonDocument["_t"].AsString);
                    }
                }

                HashValue ret = new HashValue(new HashDict(new Dictionary<ValueBase, ValueBase>()));
                var ra = val.AsBsonDocument;
                foreach (var r in ra)
                {
                    ValueBase key = new StringValue(r.Name);
                    ValueBase value = ConvertValueFromBson(r.Value);
                    if (key != null && value != null)
                        ret.AsHash.Add(key, value);
                }
                return ret;
            }
            else if (val is BsonArray)
            {
                ListValue ret = new ListValue(new List<Expression>());
                var ra = val.AsBsonArray;
                foreach (var r in ra)
                {
                    ValueBase vb = ConvertValueFromBson(r);
                    if (vb != null)
                        ret.AsList.Add(new LiteralExpression(vb));
                }
                return ret;
            }
            else
            {
                Log.Warn("Type of returned bson object '{0}' is unknown. Ignoring.", val.GetType());
                throw new InterpretionException(String.Format("Type of returned bson object '{0}' is unknown. Ignoring.", val.GetType()));
            }

        }
    }
}