using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

#if !SILVERLIGHT
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
#endif

namespace Easis.Common
{
    public static class ExtensionMethods
    {
        // returns the number of milliseconds since Jan 1, 1970 (useful for converting C# dates to JS dates)
        public static double ToUnixTicks(this DateTime dt)
        {
            DateTime d1 = new DateTime(1970, 1, 1);
            DateTime d2 = dt.ToUniversalTime();
            TimeSpan ts = new TimeSpan(d2.Ticks - d1.Ticks);
            return ts.TotalMilliseconds;
        }

#if !SILVERLIGHT

        public static string Sanitize(this string str)
        {
            return str.ToJson();//.Replace('\n', ' ').Replace('\r', ' ').Replace('\"', ' ').Replace('\'', ' ');
        }


        public static string ToXmlString(this object ob)
        {
            return "Serialized";
            XmlSerializer xs = new XmlSerializer(ob.GetType());
            MemoryStream ms = new MemoryStream();

            using (StreamWriter sw = new StreamWriter(ms, Encoding.UTF8))
            {
                xs.Serialize(sw, ob, new XmlSerializerNamespaces());
            }
            ms.Seek(0, SeekOrigin.Begin);
            string ret;
            using (StreamReader sr = new StreamReader(ms, Encoding.UTF8))
            {
                TextReader tr = sr;
                ret = tr.ReadToEnd();
            }
            return ret;
        }

        public static string ToJsonString(this object ob)
        {
            return ob.ToJson();
        }
#endif
    }
}
