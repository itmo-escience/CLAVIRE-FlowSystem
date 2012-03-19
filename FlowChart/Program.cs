using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

using Easis.Common.IdGenerators;
using Easis.Wfs.EasyFlow.Model;
using Easis.Wfs.EasyFlow.Parsing;
using Easis.Common.Collections;
using NLog;


namespace FlowChart
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger log = LogManager.GetLogger("Foo");
            log.Info("ItWorks!!");

            ParserSettings settings = new ParserSettings
            {
                CollectScopeInformation = true,
                CollectTextInformation = true,
                LogRuleTraces = true
            };

            ScriptParser parser = new ScriptParser(settings);
            Stream inStream = new FileStream("Test1.flow", FileMode.Open);
            ParseResult parseResult = parser.Parse(inStream);

            if (!parseResult.Succeeded)
            {
                Console.WriteLine("Parse error!");
                return;
            }

            Flow flow = parseResult.ScriptModel.MainFlow;

            using (StreamWriter dotStream = new StreamWriter("out.dot"))
            {
                dotStream.WriteLine("digraph flow {");
                dotStream.WriteLine("rankdir=\"LR\";");
                

                foreach (BlockBase block in flow.Blocks)
                {
                    StepBlock stepBlock = (StepBlock)block;

                    dotStream.WriteLine(@"{0}[shape=""none"", margin=0, label=<
                      <TABLE border=""0"" cellborder=""1"" cellspacing=""0"">
                        <TR><TD BGCOLOR=""gray"">{0} : {1}</TD></TR>;", stepBlock.Name, stepBlock.RunObjectName);

                    if (stepBlock.RunParameters.Parameters.Count > 0)
                    {
                        dotStream.WriteLine("<TR><TD balign=\"LEFT\">");
                        foreach (NamedParameter param in stepBlock.RunParameters.Parameters)
                        {
                            dotStream.WriteLine("{0}<BR/>", param);
                        }
                        dotStream.WriteLine("</TD></TR>");

                    }

                    dotStream.WriteLine("</TABLE>>];");


                    foreach (var trigger in block.Triggers.Values)
                    {
                        if (trigger.ActionDef.Name == TriggerActionDef.ACTION_START)
                        {
                            foreach (var triggerEvent in trigger.EventExpression.Events)
                            {
                                var srcName = ((StepBlock)triggerEvent.Source).Name;
                                dotStream.WriteLine("{0} -> {1}", srcName, stepBlock.Name);
                                Console.WriteLine(triggerEvent.TextBehind);
                                //Console.Write("{0}.{1}", ((StepBlock)triggerEvent.Source).Name, triggerEvent.Name);
                            }
                        }
                    }
                }

                dotStream.WriteLine("}");
            }

            Process p = new Process();
            p.StartInfo.FileName = "dot.exe";
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.StartInfo.Arguments = "out.dot -T png -o out.png";
            p.Start();
            p.WaitForExit();

            Console.WriteLine("ready");
            Console.ReadKey();
        }
    }
}
