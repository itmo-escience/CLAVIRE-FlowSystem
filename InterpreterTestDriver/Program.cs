using System;
using System.IO;
using System.Linq;
using Easis.Common.DataContracts;
using Easis.Wfs.EasyFlow.Model;
using Easis.Wfs.EasyFlow.Parsing;
using Easis.Wfs.Interpreting;
using NLog;

namespace InterpreterTestDriver
{
    class Program
    {
        static Flow Parse(string script)
        {
            Logger log = LogManager.GetLogger("Parser");
            log.Info("ItWorks!!");

            ParserSettings settings = new ParserSettings
                                          {
                                              CollectScopeInformation = true,
                                              CollectTextInformation = true,
                                              LogRuleTraces = true
                                          };

            ScriptParser parser = new ScriptParser(settings);
            //            Stream inStream = new FileStream("Test1.flow", FileMode.Open);
            ParseResult parseResult = parser.Parse(script);

            if (parseResult.Succeeded)
            {
                //Console.WriteLine("Parsing succeeded.");

                Flow flow = parseResult.ScriptModel.MainFlow;

                if (parseResult.ScriptModel.Requirements.Count > 0)
                {
                    //Console.Write("require " +
                    //              parseResult.ScriptModel.Requirements.Aggregate("",
                    //                                                             (s, requirement) =>
                    //                                                             s + (s == "" ? s : ", ") +
                    //                                                             requirement.Name));
                    //Console.Write(";\n\n");
                }



                foreach (BlockBase block in flow.Blocks)
                {
                    StepBlock stepBlock = (StepBlock)block;

                    //Console.Write(string.Format("Step {0}", stepBlock.Name));

                    bool isFirstTrigger = true;
                    foreach (var trigger in block.Triggers.Values)
                    {
                        if (trigger.ActionDef.Name == TriggerActionDef.ACTION_START)
                        {
                            if (isFirstTrigger)
                            {
                                //Console.Write(" on ");
                                isFirstTrigger = false;
                            }


                            bool isFirstEvent = true;
                            foreach (var triggerEvent in trigger.EventExpression.Events)
                            {
                                //if (isFirstEvent)
                                //    isFirstEvent = false;
                                //else 
                                //Console.Write(", ");

                                //Console.Write("{0}.{1}", ((StepBlock)triggerEvent.Source).Name, triggerEvent.Name);
                            }
                        }
                    }

                    //Console.Write(string.Format(" as {0}\n(\n", stepBlock.RunObjectName));

                    foreach (NamedParameter param in stepBlock.RunParameters.Parameters)
                    {
                        //Console.Write("\t{0}\n", param);
                    }

                    //Console.WriteLine(");\n");
                }

                return flow;
            }
            else
            {
                Console.WriteLine("During parsing some errors occured:");
                return null;
            }
        }


        static void Interprete(string fname)
        {
            Logger log = LogManager.GetLogger("Interpreter");

            FlowSystemContext flowSystemContext = new FlowSystemContext();
            StepStarterSimulator stepStarter = new StepStarterSimulator();

            string script = File.ReadAllText(fname);

            // Создаем контекст
            IGlobalContext gc = new GlobalContext();
            gc.FileRegistry = new DictBasedFileRegistry();
            gc.FlowSystemContext = flowSystemContext;
            gc.PackageRegistry = new ListBasedPackageRegistry();
            gc.StepStarter = stepStarter;

            log.Info("Creating Interpreter");
            DeclarativeInterpreter declarativeInterpreter = new DeclarativeInterpreter(gc);
            stepStarter.SetEventConsumer(declarativeInterpreter);

            log.Info("Parsing");
            Flow flow = Parse(script);

            DateTime startTime = DateTime.Now;

            FlowDataContext flowDataContext = new FlowDataContext();
            flowDataContext.InputFiles.Add("file1", "000000000000000");

            ExecutionContext flowExecutionProperties = new ExecutionContext();
            declarativeInterpreter.ExecuteFlow(Guid.NewGuid(), flow, flowDataContext, flowExecutionProperties);

            Console.ReadKey();

        }

        /// <summary>
        /// Test1.flow interpretion
        /// </summary>
        static void TestEasyFlowInterpretation()
        {
            Interprete("Test1.flow");
        }

        /// <summary>
        /// Imp.flow interpretion
        /// </summary>
        static void TestImperative()
        {
            Interprete("Imp.flow");
        }

        static void Main(string[] args)
        {
            TestEasyFlowInterpretation();
        }

        /// <summary>
        /// Код интерпретации в рамках эксперимента с интерпретатором.
        /// </summary>
        static void Experiments()
        {
            Logger log = LogManager.GetLogger("Interpreter");

            const int MIN_N = 10;
            const int STEP = 50;
            const int MAX_N = 500 + 1;
            const int REPEAT = 50;

            //const int MIN_N = 1000;
            //const int STEP = 50;
            //const int MAX_N = 1001 + 1;
            //const int REPEAT = 1;

            double[][] perf = new double[MAX_N][];
            for (int i = 0; i < MAX_N; i++)
            {
                perf[i] = new double[REPEAT];
            }

            StreamWriter sw = File.CreateText(String.Format("Results_{0}x{1}_{2}", MAX_N, REPEAT, DateTime.Now.Ticks));

            //string horiz_graph = GraphSerializer.SerializeGraph(GraphGenerator.GenerateHorizontalLineGraph(1000),"file1");
            int curstep = 2;

            for (int i = MIN_N; i < MAX_N; i += curstep)
            {
                string graph = GraphSerializer.SerializeGraph(GraphGenerator.GenerateHorizontalLineGraph(i), "file1");
                //                string graph = GraphSerializer.SerializeGraph(GraphGenerator.GenerateVerticalLineGraph(i), "file1");

                //                Console.WriteLine(graph);

                double min = 10000, max = -10;

                for (int j = 0; j < REPEAT; j++)
                {

                    FlowSystemContext flowSystemContext = new FlowSystemContext();
                    StepStarterSimulator stepStarter = new StepStarterSimulator();

                    // Создаем контекст
                    IGlobalContext gc = new GlobalContext();
                    gc.FileRegistry = new DictBasedFileRegistry();
                    gc.FlowSystemContext = flowSystemContext;
                    gc.PackageRegistry = new ListBasedPackageRegistry();
                    gc.StepStarter = stepStarter;

                    log.Info("Creating Interpreter");
                    DeclarativeInterpreter declarativeInterpreter = new DeclarativeInterpreter(gc);
                    stepStarter.SetEventConsumer(declarativeInterpreter);

                    log.Info("Parsing");
                    Flow flow = Parse(graph);

                    DateTime startTime = DateTime.Now;

                    FlowDataContext flowDataContext = new FlowDataContext();
                    flowDataContext.InputFiles.Add("file1", "000000000000000");

                    ExecutionContext flowExecutionProperties = new ExecutionContext();
                    declarativeInterpreter.ExecuteFlow(Guid.NewGuid(), flow, flowDataContext, flowExecutionProperties);

                    DateTime stopTime = DateTime.Now;


                    TimeSpan duration = stopTime - startTime;

                    perf[i][j] = duration.TotalMilliseconds;
                    min = perf[i][j] < min ? perf[i][j] : min;
                    max = perf[i][j] > max ? perf[i][j] : max;
                    //Console.WriteLine(String.Format("{0}: {1} repeat from {2} finished [{3}]", i, j, REPEAT, perf[i][j]));
                }
                Console.WriteLine("{0} {1} {2}", i, min, max);

                if (curstep < STEP)
                    curstep = curstep * 2;
                if (curstep > STEP)
                    curstep = STEP;
            }

            sw.WriteLine("Graph type: {0}\nMAX_N: {1}\nMIN_N: {2}\nSTEP: {3}\nREPEATS: {4}\n", "vertical", MAX_N, MIN_N, STEP, REPEAT);

            for (int i = MIN_N; i < MAX_N; i++)
            {
                sw.Write("{0}: ", i);

                for (int j = 0; j < REPEAT; j++)
                {
                    sw.Write("{0}  ", perf[i][j].ToString("F4"));
                }
                sw.WriteLine("");
            }

            sw.Close();

            //Console.ReadKey();
        }
    }
}
