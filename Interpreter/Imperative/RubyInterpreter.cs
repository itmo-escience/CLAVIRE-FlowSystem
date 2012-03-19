using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Easis.Wfs.EasyFlow.Model;
using Easis.Wfs.EasyFlow.Model.Expressions;
using IronRuby;
using IronRuby.Builtins;
using IronRuby.Runtime;
using Microsoft.Scripting.Hosting;
using NLog;

namespace Easis.Wfs.Interpreting.Imperative
{
    class RubyInterpreter : IScriptInterpreter
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private string _initScript = "";


        protected ValueBase ConvertRubyTypesToEf(object val)
        {
            if (val == null) throw new ArgumentNullException("val");

            if (val is Boolean)
            {
                return new BoolValue((Boolean)val);
            }
            else if (val is Int32)
            {
                return new IntValue((Int32)val);
            }
            else if (val is Int64)
            {
                return new IntValue((Int64)val);
            }
            else if (val is String)
            {
                return new StringValue((String)val);
            }
            else if (val is MutableString)
            {
                return new StringValue(((MutableString)val).ToString());
            }
            else if (val is Double)
            {
                return new DoubleValue((Double)val);
            }
            else if (val is Integer)
            {
                int a = Int32.Parse(((Integer)val).ToString());
                return new IntValue(a);
            }
            else if (val is Hash)
            {
                HashValue ret = new HashValue(new HashDict(new Dictionary<ValueBase, ValueBase>()));
                var ra = val as Hash;
                foreach (var r in ra)
                {
                    ValueBase key = ConvertRubyTypesToEf(r.Key);
                    ValueBase value = ConvertRubyTypesToEf(r.Value);
                    if (key != null && value != null)
                        ret.AsHash.Add(key, value);
                }
                return ret;
            }
            else if (val is RubyArray)
            {
                ListValue ret = new ListValue(new List<Expression>());
                var ra = val as RubyArray;
                foreach (var r in ra)
                {
                    ValueBase vb = ConvertRubyTypesToEf(r);
                    if (vb != null)
                        ret.AsList.Add(new LiteralExpression(vb));
                }
                return ret;
            }
            else
            {
                Log.Warn("Type of returned ruby object '{0}' is unknown. Ignoring.", val.GetType());
                throw new InterpretionException(String.Format("Type of returned ruby object '{0}' is unknown. Ignoring.", val.GetType()));
            }
        }

        protected object ConvertEfTypesToRuby(ValueBase val)
        {
            if (val == null) throw new ArgumentNullException("val");

            if(val is UndefinedValue)
            {
                Log.Trace("{0}", val);
                return null;
            }
            else if (val is BoolValue)
            {
                Log.Trace("{0}", val.Value);
                return val.Value;
            }
            if (val is IntValue)
            {
                Log.Trace("{0}", val.Value);
                return val.Value;
            }
            if (val is StringValue)
            {
                Log.Trace("{0}", val.Value);
                var ret = new MutableString();
                ret.Append((String)val.Value);
                return ret;
            }
            else if (val is DoubleValue)
            {
                Log.Trace("{0}", val.Value);
                return val.Value;
            }
            else if (val is HashValue)
            {
                string str = "{";
                Hash ret = new Hash(new Dictionary<object, object>());
                foreach (var r in ((HashValue)val).AsHash)
                {
                    object key = ConvertEfTypesToRuby(r.Key);
                    object value = ConvertEfTypesToRuby(r.Value);
                    if (key != null && value != null)
                    {
                        ret.Add(key, value);
                        str += String.Format("{0}: {1},", key, value);
                    }
                }
                str += "}";
                Log.Trace("{0}", str);
                return ret;
            }
            else if (val is ListValue)
            {
                string str = "[";
                RubyArray ret = new RubyArray();
                foreach (var r in ((ListValue)val).AsList)
                {
                    object key = ConvertEfTypesToRuby(r.Evaluate(null));
                    if (key != null)
                    {
                        ret.Add(key);
                        str += String.Format("{0},", key);
                    }
                }
                str += "]";
                return ret;
            }
            else if (val is StructureValue)
            {
                string str = "{";
                Hash ret = new Hash(new Dictionary<object, object>());
                foreach (var r in ((StructureValue)val).AsDict)
                {
                    var ms = new MutableString();
                    ms.Append(r.Key);
                    object key = ms;
                    object value = ConvertEfTypesToRuby(r.Value);
                    if (key != null && value != null)
                    {
                        ret.Add(key, value);
                        str += String.Format("{0}: {1},", key, value);
                    }
                }
                str += "}";
                Log.Trace("{0}", str);
                return ret;
            }
            else if (val is FileValue)
            {
                var ret = new MutableString();
                var fileDescriptor = ((FileValue)val).AsFileDescriptor;
                ret.Append(String.Format("{0}|{1}", fileDescriptor.FileIdentifier, fileDescriptor.NStorageId));
                return ret;
            }
            else
            {
                Log.Warn("Type of returned ruby object '{0}' is unknown. Ignoring.", val.GetType());
                throw new InterpretionException(String.Format("Type of ef object '{0}' is unknown. Ignoring.", val.GetType()));
            }
        }

        public HashValue ExecuteScript(GlobalDataScope globalDataScope, BlockDataScope currentDataScope, string scriptText)
        {
            ScriptEngine engine = Ruby.CreateEngine();
            engine.Runtime.LoadAssembly(typeof(IronRuby.Ruby).Assembly);
            ScriptScope scope = engine.CreateScope();

            try
            {
                scope.SetVariable("log", Log);
                engine.Execute("$log = log", scope);
            }
            catch (Exception e)
            {
                Log.ErrorException(e.Message, e);
            }

            try
            {
                engine.Execute(_initScript, scope);
            }
            catch (Exception ex)
            {
                RubyExceptionData red = RubyExceptionData.GetInstance(ex);
                StringBuilder sb = new StringBuilder();

                var scriptException = new InterpretionException("Exception while executing script: " + ex.Message, ex);
                Log.ErrorException(scriptException.Message, scriptException);
                throw scriptException;
            }


            //----------------------------------------------
            // Converting scopes
            //----------------------------------------------
            IDictionary<string, object> vars = new Dictionary<string, object>();
            // current scope
            foreach (var variable in currentDataScope.Variables)
            {
                object v = ConvertEfTypesToRuby(variable.Value);
                if (v != null)
                {
                    vars.Add(variable.Name.ToLower(), v);
                }
                else
                {
                    Log.Warn("Variable '{0}' from current scope didn't convert", variable.Name);
                }
            }

            // global scope
            foreach (var ds in globalDataScope.ChildrenScopes)
            {
                var bds = (BlockDataScope)ds;

                var bsv = new Hash(new Dictionary<object, object>());

                foreach (var variable in bds.Variables)
                {
                    object v = ConvertEfTypesToRuby(variable.Value);
                    if (v != null)
                    {
                        var ms = new MutableString();
                        ms.Append(variable.Name);
                        bsv.Add(ms, v);
                    }
                    else
                    {
                        Log.Warn("Variable '{0}' from current scope didn't convert", variable.Name);
                    }
                }
                vars.Add(bds.Name, bsv);
            }
            try
            {
                foreach (var @var in vars)
                {
                    scope.SetVariable(@var.Key.ToLower(), @var.Value);
//                    scope.Engine.Runtime.Globals.
                }
            }
            catch (Exception e)
            {
                Log.ErrorException(e.Message, e);
            }
            //----------------------------------------------

            try
            {
                Hash h = engine.Execute<Hash>(scriptText, scope);

                Log.Trace("Handling script results");
                HashValue ret = new HashValue(new HashDict(new Dictionary<ValueBase, ValueBase>()));
                foreach (var kvp in h)
                {
                    Log.Trace("Found result '{0}'", kvp.Key);
                    ValueBase key = ConvertRubyTypesToEf(kvp.Key);
                    ValueBase value = ConvertRubyTypesToEf(kvp.Value);
                    if (key != null && value != null)
                    {
                        ret.AsHash.Add(key, value);
                    }
                }
                return ret;
            }
            catch (Exception ex)
            {
                RubyExceptionData red = RubyExceptionData.GetInstance(ex);
                StringBuilder sb = new StringBuilder();

                var scriptException = new InterpretionException("Exception while executing script: " + ex.Message, ex);
                Log.ErrorException(scriptException.Message, scriptException);
                throw scriptException;
            }
        }

    }
}
