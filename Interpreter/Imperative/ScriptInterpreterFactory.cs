using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easis.Wfs.Interpreting.Imperative
{
    class ScriptInterpreterFactory
    {
        public IScriptInterpreter GetScriptInterpreter(string lang)
        {
            if(lang.ToLower() == "ruby")
            {
                return new RubyInterpreter();
            }
            else
            {
                throw new ArgumentException("Unknown script language");
            }
        }
    }
}
