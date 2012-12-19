using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easis.Wfs.Interpreting.Imperative
{
    class ScriptInterpreterFactory
    {
        private readonly WfLog _log;

        public ScriptInterpreterFactory(WfLog log)
        {
            _log = log;
        }

        public IScriptInterpreter GetScriptInterpreter(string lang)
        {
            if(lang.ToLower() == "ruby")
            {
                return new RubyInterpreter(_log);
            }
            else
            {
                throw new ArgumentException("Unknown script language");
            }
        }
    }
}
