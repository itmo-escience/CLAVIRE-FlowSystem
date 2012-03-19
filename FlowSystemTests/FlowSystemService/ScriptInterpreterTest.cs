﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Easis.Wfs.EasyFlow.Model;
using Easis.Wfs.Interpreting;
using Easis.Wfs.Interpreting.Imperative;

namespace FlowSystemTests.FlowSystemService
{
    using NUnit.Framework;

    [TestFixture]
    class ScriptInterpreterTest
    {
        [Test]
        public void BaseTestRuby()
        {
            ScriptInterpreterFactory fact = new ScriptInterpreterFactory();
            IScriptInterpreter si = fact.GetScriptInterpreter("ruby");
            GlobalDataScope gds = new GlobalDataScope();
            BlockDataScope bds = new BlockDataScope(gds, "step1");
            bds.Variables.Add(new Variable("a", new IntValue(44)));
            bds.Variables.Add(new Variable("b", new DoubleValue(4.6)));
            bds.Variables.Add(new Variable("c", new BoolValue(true)));
            bds.Variables.Add(new Variable("d", new StringValue("str")));
            bds.Variables.Add(new Variable("e", new ListValue(new List<Expression>(){new LiteralExpression(new IntValue(5))})));

            string script = @"
$log.Trace('try it')
S = ''
S = ""#{step1.Keys.Count}""
{'int' => 6, 'str' => S, 'list' => [4,5], 'hash' => {5 => 7}}
";

            var ret = si.ExecuteScript(gds, bds, script);

        }
    }
}
