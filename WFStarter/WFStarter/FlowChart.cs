using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Windows.Media.Imaging;

using Easis.Wfs.EasyFlow.Model;
using Easis.Wfs.EasyFlow.Parsing;

namespace FlowChartEditor
{
    class FlowChart
    {
        private string _parseLog;
        private Bitmap _chartImage;

        public string ParseLog
        {
            get { return _parseLog; }
        }

        public Bitmap ChartImage
        {
            get { return _chartImage; }
        }

        public FlowChart()
        {
            _parseLog = "";
        }

        public bool Parse(string code)
        {
            _parseLog = "";

            ParserSettings settings = new ParserSettings
            {
                CollectScopeInformation = true,
                CollectTextInformation = true,
                LogRuleTraces = true
            };

            ScriptParser parser = new ScriptParser(settings);
            ParseResult parseResult = parser.Parse(code);

            if (!parseResult.Succeeded)
            {
                _parseLog += "PARSING FAILURE:\n";

                return false;
            }

            _parseLog += "Parse OK\n";

            Flow flow = parseResult.ScriptModel.MainFlow;
            string dotCode = GenDotCode(flow);

            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.FileName = "dot.exe";
            p.StartInfo.Arguments = "-T png";
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();

            p.StandardInput.Write(dotCode);
            p.StandardInput.Flush();
            p.StandardInput.Close();

            Bitmap bmp = new Bitmap(p.StandardOutput.BaseStream);

            p.WaitForExit();

            _chartImage = bmp;

            return true;
        }

        public void SetDotCode(string code)
        {
            string dotCode = code;

            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.FileName = "dot.exe";
            p.StartInfo.Arguments = "-T png";
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();

            p.StandardInput.Write(dotCode);
            p.StandardInput.Flush();
            p.StandardInput.Close();

            Bitmap bmp = new Bitmap(p.StandardOutput.BaseStream);

            p.WaitForExit();

            _chartImage = bmp;

            return;
        }


        private string GenDotCode(Flow flow)
        {
            StreamWriter dotStream = new StreamWriter(new MemoryStream());

            dotStream.WriteLine("digraph flow {");
            dotStream.WriteLine("rankdir=\"BT\";");

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
                        dotStream.WriteLine("{0}<BR/>", param.TextBehind);
                    }
                    dotStream.WriteLine("</TD></TR>");

                }

                dotStream.WriteLine("</TABLE>>];");


                foreach (var trigger in block.Triggers.Values)
                {
                    //if (trigger.ActionDef.Name == TriggerActionDef.ACTION_START)
                    {
                        foreach (var triggerEvent in trigger.EventExpression.Events)
                        {
                            var srcName = ((StepBlock)triggerEvent.Source).Name;
                            dotStream.WriteLine("{0} -> {1}", srcName, stepBlock.Name);
                            //Console.WriteLine(triggerEvent.TextBehind);
                            //Console.Write("{0}.{1}", ((StepBlock)triggerEvent.Source).Name, triggerEvent.Name);
                        }
                    }
                }
            }

            dotStream.WriteLine("label=\"Workflow structure\";");

            dotStream.WriteLine("}");
            dotStream.Flush();

            dotStream.BaseStream.Seek(0, SeekOrigin.Begin);
            StreamReader reader = new StreamReader(dotStream.BaseStream);
            string dotCode = reader.ReadToEnd();

            return dotCode;
        }

    }
}
