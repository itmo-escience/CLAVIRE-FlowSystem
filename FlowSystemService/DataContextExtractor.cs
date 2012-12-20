using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Easis.Common.DataContracts;
using Easis.Common.Exceptions;
using Easis.Wfs.Interpreting;
using NLog;

namespace Easis.Wfs.FlowSystemService
{
    public interface IDataContextExtractor
    {
        FlowDataContext CreateDataContext(string scriptDataContext);
        ExecutionContext CreateExecutionContext(IDictionary<string, string> scriptEC);
    }

    /// <summary>
    /// Работает с текстом в виде:
    /// key0 = value0\n
    /// ...
    /// keyn = valuen
    /// </summary>
    public class DataContextExtractor : IDataContextExtractor
    {
        static readonly Logger _log = LogManager.GetCurrentClassLogger();

        public FlowDataContext CreateDataContext(string scriptDataContext)
        {
            // TODO: подумать, оставить или выбросить ошибку
            if (String.IsNullOrEmpty(scriptDataContext))
                return new FlowDataContext();

            Regex r = new Regex("^\\s*(\\w+)\\s*=\\s*([^\\s]+)\\s*$", RegexOptions.IgnoreCase);

            FlowDataContext ret = new FlowDataContext();

            foreach (string str in scriptDataContext.Split(new char[] { '\n', ';' }))
            {
                Match m = r.Match(str);

                // считаем, что в строке только один матч
                if (m.Success)
                {
                    string key = m.Groups[1].Value;
                    string val = m.Groups[2].Value;

                    _log.Trace("Parsed: {0} -> {1}", key, val);

                    // проверяем, файл или параметер
                    if (val.ToUpper().StartsWith("FSID#"))
                    {
                        ret.InputFiles.Add(key, val.Substring(5));
                    }
                    else
                    {
                        ret.InputParameters.Add(key, val);
                    }
                }
                else
                {
                    _log.Trace("Parsing of {0} failed. Throwing exception.", str);
                    throw new InvalidFormatException();
                }
            }

            //// получаем имя файла из супер хранилища
            //StorageService.StorageServiceClient ssc = new StorageServiceClient();

            //var ds = ssc.GetDataByIdBatch(ret.InputFiles.Values.ToArray());
            //foreach (var kvp in ret.InputFiles)
            //{
            //    DataEntry dataEntry = ds.First(entry => entry.Id == kvp.Value);
                
            //    string newv = String.Format("{0}|{1}", dataEntry.Id, dataEntry.);
            //    ret.InputFiles[dataEntry.Id] = String.Format("{0}|{1}");
            //}


            return ret;
        }

        public ExecutionContext CreateExecutionContext(IDictionary<string, string> scriptEC)
        {
            // TODO: подумать, оставить или выбросить ошибку
            if (scriptEC == null)
                return new ExecutionContext();
            
            var ret = new ExecutionContext();

            foreach (var el in scriptEC)
            {
                _log.Trace("Extracting exec param {0} {1}",el.Key, el.Value);
                if (el.Key != null && el.Value != null)
                switch (el.Key.ToLower())
                {
                    case "priority":
                        ret.Priority = el.Value;
                        break;
                    case "tempusercertid":
                        ret.TempUserCert = el.Value;
                        break;
                    case "userid":
                        ret.UserId = el.Value;
                        break;
                    default:
                        _log.Warn("Unknown execution property {0}", el.Key);
                        break;
                }
            }

            return ret;
        }
    }
}