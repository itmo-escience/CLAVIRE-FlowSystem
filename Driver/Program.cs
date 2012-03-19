using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Antlr.Runtime;
using Easis.Common.IdGenerators;
using Easis.Wfs.EasyFlow.Model;
using Easis.Wfs.EasyFlow.Parsing;
using Easis.Common.Collections;
using Easis.Wfs.Interpreting;
using NLog;

namespace Driver
{
    internal class Program
    {
        class ParentMan
        {
            
        }

        struct Child
        {
            public ParentMan Parent { get; internal set; }
            public long Id { get; internal set; }
        }

        class MyOwnedCollection : OwnedCollection<ParentMan, Child>
        {
            IIdGenerator<long> _idGen = new LongIdGenerator();

            /// <summary>
            /// Initializes a new instance of the <see cref="T:System.Object"/> class.
            /// </summary>
            public MyOwnedCollection(ParentMan owner): base(owner)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="T:System.Object"/> class.
            /// </summary>
            public MyOwnedCollection(ParentMan owner, IEnumerable<Child> items): base(owner, items)
            {
            }

            protected override void AdoptItem(Child item)
            {
                item.Parent = Owner;
                item.Id = _idGen.GetId();
            }

            protected override void DeadoptItem(Child item)
            {
                item.Parent = null;
                item.Id = -1;
            }
        }

        private static void Main(string [] args)
        {
            DeclarativeInterpreter di = new DeclarativeInterpreter(new GlobalContext());
            Console.WriteLine(di.WfId);

            Logger log = LogManager.GetLogger("Foo");
            //log.Info("ItWorks!!");

            //log.ErrorException("blah", new Exception("hooo"));
            
            ParserSettings settings = new ParserSettings
                                          {
                                              CollectScopeInformation = true,
                                              CollectTextInformation = true,
                                              LogRuleTraces = true
                                          };


            ScriptParser parser = new ScriptParser(settings);


            Stream inStream = new FileStream("Test1.flow", FileMode.Open);

            ParseResult parseResult = null;
            try
            {
                parseResult = parser.Parse(inStream);
            }
            catch (RecognitionException recExc)
            {
                Console.WriteLine("Line: {0}, LinePosition: {1}, Index: {2}, Token: {3}", recExc.Line, recExc.CharPositionInLine, recExc.Index, recExc.Token);
            }
            

            if (parseResult != null && parseResult.Succeeded)
            {
                Console.WriteLine("Parsing succeeded.\n\n");
                Flow flow = parseResult.ScriptModel.MainFlow;

                if (parseResult.ScriptModel.Requirements.Count > 0)
                {
                    Console.Write("require " +
                                  parseResult.ScriptModel.Requirements.Aggregate("",
                                                                                 (s, requirement) =>
                                                                                 s + (s == "" ? s : ", ") +
                                                                                 requirement.Name));
                    Console.Write(";\n\n");
                }

                

                foreach (BlockBase block in flow.Blocks)
                {
                    StepBlock stepBlock = (StepBlock) block;

                    foreach (var execParam in stepBlock.ExecParameters.Parameters)
                    {
                        Console.WriteLine("[{0}]", execParam);
                    }

                    Console.Write(string.Format("step {0}", stepBlock.Name));

                    bool isFirstTrigger = true;
                    foreach (var trigger in block.Triggers.Values)
                    {
                        if (trigger.ActionDef.Name == TriggerActionDef.ACTION_START)
                        {
                            bool isFirstEvent = true;
                            foreach (var triggerEvent in trigger.EventExpression.Events)
                            {
                                if (isFirstTrigger)
                                {
                                    Console.Write(" on ");
                                    isFirstTrigger = false;
                                }

                                if (isFirstEvent)
                                    isFirstEvent = false;
                                else
                                    Console.Write(", ");

                                Console.Write("{0}.{1}", ((StepBlock)triggerEvent.Source).Name, triggerEvent.Name);
                            }                            
                        }
                    }

                    Console.Write(string.Format(" runs {0}\n(\n", stepBlock.RunObjectName));

                    foreach (NamedParameter param in stepBlock.RunParameters.Parameters)
                    {
                        Console.Write("  {0}\n", param);
                    }

                    Console.WriteLine(");\n");
                }
            }
            else
            {
                Console.WriteLine("During parsing some errors occured:");

                foreach (ParserMessage message in parseResult.ParserMessageCollection)
                {
                    Console.WriteLine(string.Format("  {0} ({1}): {2}", message.MessageType,
                                                    message.MessageObject.TextRange.Start, message.Message));
                }
            }
            Console.ReadKey();
        }
    }
}