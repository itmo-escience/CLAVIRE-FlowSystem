using System;
using System.Linq;
using System.Text;
using Easis.Wfs.EasyFlow.Model;
using MongoDB.Bson;
using NLog;

namespace Easis.Wfs.Interpreting.Imperative
{
    class JsonDataFormatter
    {
        private WfLog Log;

        public JsonDataFormatter(WfLog log)
        {
            Log = new WfLog(log, LogManager.GetCurrentClassLogger());
        }

        public ValueBase ConvertValueFromJson(string val)
        {
            BsonValue bv;
            try
            {
                bv = BsonDocument.Parse(val);
            }
            catch(InvalidOperationException ex)
            {
                //TODO: zn hack bson array
                // not document - value
                string wrappedVal = "{\"value\": " + val +" }";
                BsonDocument bd = BsonDocument.Parse(wrappedVal);
                bv = bd["value"];
            }

            BsonDataFormatter bdf = new BsonDataFormatter(Log);
            return bdf.ConvertValueFromBson(bv);
        }
    }
}
