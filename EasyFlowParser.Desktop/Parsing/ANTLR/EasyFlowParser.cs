// $ANTLR 3.1.3 Mar 18, 2009 10:09:25 D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g 2011-12-18 18:54:46

// The variable 'variable' is assigned but its value is never used.
#pragma warning disable 168, 219
// Unreachable code detected.
#pragma warning disable 162


  using System.Collections.Generic; 
  using System.Linq;
  using Easis.Wfs.EasyFlow.Model;
  using Easis.Wfs.EasyFlow.Model.Collections;
  using System.Globalization;


using System;
using Antlr.Runtime;
using IList 		= System.Collections.IList;
using ArrayList 	= System.Collections.ArrayList;
using Stack 		= Antlr.Runtime.Collections.StackList;



using Antlr.Runtime.Tree;

namespace  Easis.Wfs.EasyFlow.Parsing 
{
public partial class EasyFlowParser : Parser
{
    public static readonly string[] tokenNames = new string[] 
	{
        "<invalid>", 
		"<EOR>", 
		"<DOWN>", 
		"<UP>", 
		"WS", 
		"NEWLINE", 
		"REQUIRE", 
		"SEMICOLON", 
		"LSQ", 
		"FLOW", 
		"COLON", 
		"ID", 
		"ASSIGN", 
		"RSQ", 
		"TILDA", 
		"STEP", 
		"RUNS", 
		"AFTER", 
		"LPAREN", 
		"APP", 
		"EXEC", 
		"RPAREN", 
		"CODE_BLOCK", 
		"COMMA", 
		"DOT", 
		"ARROW", 
		"SWEEP", 
		"STRING", 
		"INT", 
		"DOUBLE", 
		"BOOL", 
		"ID_CONST", 
		"ML_COMMENT", 
		"SL_COMMENT", 
		"POST", 
		"CODE", 
		"PRE", 
		"END", 
		"LCUR", 
		"RCUR", 
		"ON", 
		"ID_BASE", 
		"ESC_SEQ", 
		"DEC_DIGIT", 
		"EXPONENT", 
		"HEX_DIGIT", 
		"UNICODE_ESC", 
		"OCTAL_ESC", 
		"RULETTER"
    };

    public const int EXPONENT = 44;
    public const int LSQ = 8;
    public const int SWEEP = 26;
    public const int DEC_DIGIT = 43;
    public const int OCTAL_ESC = 47;
    public const int APP = 19;
    public const int STEP = 15;
    public const int PRE = 36;
    public const int ID = 11;
    public const int EOF = -1;
    public const int REQUIRE = 6;
    public const int RCUR = 39;
    public const int LPAREN = 18;
    public const int CODE = 35;
    public const int ML_COMMENT = 32;
    public const int RPAREN = 21;
    public const int TILDA = 14;
    public const int ESC_SEQ = 42;
    public const int COMMA = 23;
    public const int DOUBLE = 29;
    public const int DOT = 24;
    public const int RSQ = 13;
    public const int LCUR = 38;
    public const int UNICODE_ESC = 46;
    public const int BOOL = 30;
    public const int ON = 40;
    public const int HEX_DIGIT = 45;
    public const int SEMICOLON = 7;
    public const int EXEC = 20;
    public const int INT = 28;
    public const int AFTER = 17;
    public const int COLON = 10;
    public const int WS = 4;
    public const int RULETTER = 48;
    public const int NEWLINE = 5;
    public const int FLOW = 9;
    public const int RUNS = 16;
    public const int ID_BASE = 41;
    public const int SL_COMMENT = 33;
    public const int ASSIGN = 12;
    public const int CODE_BLOCK = 22;
    public const int ID_CONST = 31;
    public const int POST = 34;
    public const int ARROW = 25;
    public const int END = 37;
    public const int STRING = 27;

    // delegates
    // delegators



        public EasyFlowParser(ITokenStream input)
    		: this(input, new RecognizerSharedState()) {
        }

        public EasyFlowParser(ITokenStream input, RecognizerSharedState state)
    		: base(input, state) {
            InitializeCyclicDFAs();

             
        }
        
    protected ITreeAdaptor adaptor = new CommonTreeAdaptor();

    public ITreeAdaptor TreeAdaptor
    {
        get { return this.adaptor; }
        set {
    	this.adaptor = value;
    	}
    }

    override public string[] TokenNames {
		get { return EasyFlowParser.tokenNames; }
    }

    override public string GrammarFileName {
		get { return "D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g"; }
    }


    public class program_return : ParserRuleReturnScope
    {
        public ParseResult ParseResult;
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "program"
    // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:29:1: program returns [ParseResult ParseResult] : wsns ( topClause wsns )* ;
    public EasyFlowParser.program_return program() // throws RecognitionException [1]
    {   
        EasyFlowParser.program_return retval = new EasyFlowParser.program_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        EasyFlowParser.wsns_return wsns1 = default(EasyFlowParser.wsns_return);

        EasyFlowParser.topClause_return topClause2 = default(EasyFlowParser.topClause_return);

        EasyFlowParser.wsns_return wsns3 = default(EasyFlowParser.wsns_return);



          
          Init(); // Initialize parser
          
          StartRootContext(); // Start root context
          
          StartRootScope(); // Start root scope (scipt)

        try 
    	{
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:47:3: ( wsns ( topClause wsns )* )
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:47:5: wsns ( topClause wsns )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_wsns_in_program109);
            	wsns1 = wsns();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, wsns1.Tree);
            	// D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:47:10: ( topClause wsns )*
            	do 
            	{
            	    int alt1 = 2;
            	    int LA1_0 = input.LA(1);

            	    if ( (LA1_0 == REQUIRE || LA1_0 == LSQ || (LA1_0 >= TILDA && LA1_0 <= STEP)) )
            	    {
            	        alt1 = 1;
            	    }


            	    switch (alt1) 
            		{
            			case 1 :
            			    // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:47:12: topClause wsns
            			    {
            			    	PushFollow(FOLLOW_topClause_in_program113);
            			    	topClause2 = topClause();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, topClause2.Tree);
            			    	PushFollow(FOLLOW_wsns_in_program115);
            			    	wsns3 = wsns();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, wsns3.Tree);

            			    }
            			    break;

            			default:
            			    goto loop1;
            	    }
            	} while (true);

            	loop1:
            		;	// Stops C# compiler whining that label 'loop1' has no statements


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);

              FinishScope(); // Finish root scope
              
              FinishContext(); // Finish root context
              
              Finali(); // Finalize parser
              retval.ParseResult =  _parseResult; // Return parse result  

        }

          catch (RecognitionException re)
          {
            throw re;
          }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "program"

    public class wsn_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "wsn"
    // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:51:1: wsn : ( WS | NEWLINE );
    public EasyFlowParser.wsn_return wsn() // throws RecognitionException [1]
    {   
        EasyFlowParser.wsn_return retval = new EasyFlowParser.wsn_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken set4 = null;

        object set4_tree=null;

        try 
    	{
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:52:3: ( WS | NEWLINE )
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:
            {
            	root_0 = (object)adaptor.GetNilNode();

            	set4 = (IToken)input.LT(1);
            	if ( (input.LA(1) >= WS && input.LA(1) <= NEWLINE) ) 
            	{
            	    input.Consume();
            	    adaptor.AddChild(root_0, (object)adaptor.Create(set4));
            	    state.errorRecovery = false;
            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    throw mse;
            	}


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }

          catch (RecognitionException re)
          {
            throw re;
          }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "wsn"

    public class wsnp_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "wsnp"
    // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:57:1: wsnp : ( wsn )+ ;
    public EasyFlowParser.wsnp_return wsnp() // throws RecognitionException [1]
    {   
        EasyFlowParser.wsnp_return retval = new EasyFlowParser.wsnp_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        EasyFlowParser.wsn_return wsn5 = default(EasyFlowParser.wsn_return);



        try 
    	{
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:58:3: ( ( wsn )+ )
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:58:5: ( wsn )+
            {
            	root_0 = (object)adaptor.GetNilNode();

            	// D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:58:5: ( wsn )+
            	int cnt2 = 0;
            	do 
            	{
            	    int alt2 = 2;
            	    int LA2_0 = input.LA(1);

            	    if ( ((LA2_0 >= WS && LA2_0 <= NEWLINE)) )
            	    {
            	        alt2 = 1;
            	    }


            	    switch (alt2) 
            		{
            			case 1 :
            			    // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:58:5: wsn
            			    {
            			    	PushFollow(FOLLOW_wsn_in_wsnp160);
            			    	wsn5 = wsn();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, wsn5.Tree);

            			    }
            			    break;

            			default:
            			    if ( cnt2 >= 1 ) goto loop2;
            		            EarlyExitException eee2 =
            		                new EarlyExitException(2, input);
            		            throw eee2;
            	    }
            	    cnt2++;
            	} while (true);

            	loop2:
            		;	// Stops C# compiler whining that label 'loop2' has no statements


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }

          catch (RecognitionException re)
          {
            throw re;
          }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "wsnp"

    public class wsns_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "wsns"
    // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:62:1: wsns : ( wsn )* ;
    public EasyFlowParser.wsns_return wsns() // throws RecognitionException [1]
    {   
        EasyFlowParser.wsns_return retval = new EasyFlowParser.wsns_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        EasyFlowParser.wsn_return wsn6 = default(EasyFlowParser.wsn_return);



        try 
    	{
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:63:3: ( ( wsn )* )
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:63:5: ( wsn )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	// D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:63:5: ( wsn )*
            	do 
            	{
            	    int alt3 = 2;
            	    int LA3_0 = input.LA(1);

            	    if ( ((LA3_0 >= WS && LA3_0 <= NEWLINE)) )
            	    {
            	        alt3 = 1;
            	    }


            	    switch (alt3) 
            		{
            			case 1 :
            			    // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:63:5: wsn
            			    {
            			    	PushFollow(FOLLOW_wsn_in_wsns177);
            			    	wsn6 = wsn();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, wsn6.Tree);

            			    }
            			    break;

            			default:
            			    goto loop3;
            	    }
            	} while (true);

            	loop3:
            		;	// Stops C# compiler whining that label 'loop3' has no statements


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }

          catch (RecognitionException re)
          {
            throw re;
          }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "wsns"

    public class requireClause_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "requireClause"
    // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:66:1: requireClause : REQUIRE wsnp idList wsns SEMICOLON ;
    public EasyFlowParser.requireClause_return requireClause() // throws RecognitionException [1]
    {   
        EasyFlowParser.requireClause_return retval = new EasyFlowParser.requireClause_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken REQUIRE7 = null;
        IToken SEMICOLON11 = null;
        EasyFlowParser.wsnp_return wsnp8 = default(EasyFlowParser.wsnp_return);

        EasyFlowParser.idList_return idList9 = default(EasyFlowParser.idList_return);

        EasyFlowParser.wsns_return wsns10 = default(EasyFlowParser.wsns_return);


        object REQUIRE7_tree=null;
        object SEMICOLON11_tree=null;


          StartScope(ScopeType.Require);

        try 
    	{
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:75:3: ( REQUIRE wsnp idList wsns SEMICOLON )
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:75:5: REQUIRE wsnp idList wsns SEMICOLON
            {
            	root_0 = (object)adaptor.GetNilNode();

            	REQUIRE7=(IToken)Match(input,REQUIRE,FOLLOW_REQUIRE_in_requireClause207); 
            		REQUIRE7_tree = (object)adaptor.Create(REQUIRE7);
            		adaptor.AddChild(root_0, REQUIRE7_tree);

            	PushFollow(FOLLOW_wsnp_in_requireClause209);
            	wsnp8 = wsnp();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, wsnp8.Tree);
            	PushFollow(FOLLOW_idList_in_requireClause211);
            	idList9 = idList();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, idList9.Tree);
            	PushFollow(FOLLOW_wsns_in_requireClause213);
            	wsns10 = wsns();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, wsns10.Tree);
            	SEMICOLON11=(IToken)Match(input,SEMICOLON,FOLLOW_SEMICOLON_in_requireClause215); 
            		SEMICOLON11_tree = (object)adaptor.Create(SEMICOLON11);
            		adaptor.AddChild(root_0, SEMICOLON11_tree);


            	      foreach (var id in ((idList9 != null) ? idList9.Identifiers : default(IEnumerable<Identifier>)))      
            	      {
            	        FileRequirement requirement = new FileRequirement(id.Name);
            	        FinishObj(requirement, id);
            	      
            	        AddRequirement(new FileRequirement(id.Name));
            	      }      
            	    

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
              
              FinishScope();   

        }

          catch (RecognitionException re)
          {
            throw re;
          }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "requireClause"

    public class topClause_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "topClause"
    // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:87:1: topClause : ( flowAttributeClause | stepClause | requireClause );
    public EasyFlowParser.topClause_return topClause() // throws RecognitionException [1]
    {   
        EasyFlowParser.topClause_return retval = new EasyFlowParser.topClause_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        EasyFlowParser.flowAttributeClause_return flowAttributeClause12 = default(EasyFlowParser.flowAttributeClause_return);

        EasyFlowParser.stepClause_return stepClause13 = default(EasyFlowParser.stepClause_return);

        EasyFlowParser.requireClause_return requireClause14 = default(EasyFlowParser.requireClause_return);



        try 
    	{
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:88:3: ( flowAttributeClause | stepClause | requireClause )
            int alt4 = 3;
            alt4 = dfa4.Predict(input);
            switch (alt4) 
            {
                case 1 :
                    // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:89:5: flowAttributeClause
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_flowAttributeClause_in_topClause246);
                    	flowAttributeClause12 = flowAttributeClause();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, flowAttributeClause12.Tree);

                    }
                    break;
                case 2 :
                    // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:90:5: stepClause
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_stepClause_in_topClause252);
                    	stepClause13 = stepClause();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, stepClause13.Tree);

                    }
                    break;
                case 3 :
                    // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:91:5: requireClause
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_requireClause_in_topClause258);
                    	requireClause14 = requireClause();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, requireClause14.Tree);

                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }

          catch (RecognitionException re)
          {
            throw re;
          }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "topClause"

    public class flowAttributeClause_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "flowAttributeClause"
    // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:98:1: flowAttributeClause : LSQ wsns FLOW wsns COLON wsns ID wsns ASSIGN wsns expression RSQ ;
    public EasyFlowParser.flowAttributeClause_return flowAttributeClause() // throws RecognitionException [1]
    {   
        EasyFlowParser.flowAttributeClause_return retval = new EasyFlowParser.flowAttributeClause_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken LSQ15 = null;
        IToken FLOW17 = null;
        IToken COLON19 = null;
        IToken ID21 = null;
        IToken ASSIGN23 = null;
        IToken RSQ26 = null;
        EasyFlowParser.wsns_return wsns16 = default(EasyFlowParser.wsns_return);

        EasyFlowParser.wsns_return wsns18 = default(EasyFlowParser.wsns_return);

        EasyFlowParser.wsns_return wsns20 = default(EasyFlowParser.wsns_return);

        EasyFlowParser.wsns_return wsns22 = default(EasyFlowParser.wsns_return);

        EasyFlowParser.wsns_return wsns24 = default(EasyFlowParser.wsns_return);

        EasyFlowParser.expression_return expression25 = default(EasyFlowParser.expression_return);


        object LSQ15_tree=null;
        object FLOW17_tree=null;
        object COLON19_tree=null;
        object ID21_tree=null;
        object ASSIGN23_tree=null;
        object RSQ26_tree=null;

         
          NamedParameter namedParam = null;

        try 
    	{
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:108:3: ( LSQ wsns FLOW wsns COLON wsns ID wsns ASSIGN wsns expression RSQ )
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:108:5: LSQ wsns FLOW wsns COLON wsns ID wsns ASSIGN wsns expression RSQ
            {
            	root_0 = (object)adaptor.GetNilNode();

            	LSQ15=(IToken)Match(input,LSQ,FOLLOW_LSQ_in_flowAttributeClause295); 
            		LSQ15_tree = (object)adaptor.Create(LSQ15);
            		adaptor.AddChild(root_0, LSQ15_tree);

            	PushFollow(FOLLOW_wsns_in_flowAttributeClause297);
            	wsns16 = wsns();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, wsns16.Tree);
            	FLOW17=(IToken)Match(input,FLOW,FOLLOW_FLOW_in_flowAttributeClause299); 
            		FLOW17_tree = (object)adaptor.Create(FLOW17);
            		adaptor.AddChild(root_0, FLOW17_tree);

            	PushFollow(FOLLOW_wsns_in_flowAttributeClause301);
            	wsns18 = wsns();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, wsns18.Tree);
            	COLON19=(IToken)Match(input,COLON,FOLLOW_COLON_in_flowAttributeClause303); 
            		COLON19_tree = (object)adaptor.Create(COLON19);
            		adaptor.AddChild(root_0, COLON19_tree);

            	PushFollow(FOLLOW_wsns_in_flowAttributeClause305);
            	wsns20 = wsns();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, wsns20.Tree);
            	ID21=(IToken)Match(input,ID,FOLLOW_ID_in_flowAttributeClause307); 
            		ID21_tree = (object)adaptor.Create(ID21);
            		adaptor.AddChild(root_0, ID21_tree);

            	PushFollow(FOLLOW_wsns_in_flowAttributeClause309);
            	wsns22 = wsns();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, wsns22.Tree);
            	ASSIGN23=(IToken)Match(input,ASSIGN,FOLLOW_ASSIGN_in_flowAttributeClause311); 
            		ASSIGN23_tree = (object)adaptor.Create(ASSIGN23);
            		adaptor.AddChild(root_0, ASSIGN23_tree);

            	PushFollow(FOLLOW_wsns_in_flowAttributeClause313);
            	wsns24 = wsns();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, wsns24.Tree);
            	PushFollow(FOLLOW_expression_in_flowAttributeClause315);
            	expression25 = expression();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, expression25.Tree);
            	RSQ26=(IToken)Match(input,RSQ,FOLLOW_RSQ_in_flowAttributeClause317); 
            		RSQ26_tree = (object)adaptor.Create(RSQ26);
            		adaptor.AddChild(root_0, RSQ26_tree);


            	      namedParam = new NamedParameter(((ID21 != null) ? ID21.Text : null), ((expression25 != null) ? expression25.Expression : default(Expression)));
            	      AddFlowAttribute(namedParam); 
            	    

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);

              if (namedParam != null)
                FinishObj(namedParam, retval);

        }

          catch (RecognitionException re)
          {
            throw re;
          }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "flowAttributeClause"

    public class attribute_return : ParserRuleReturnScope
    {
        public NamedParameter NamedParameter;
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "attribute"
    // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:115:1: attribute returns [NamedParameter NamedParameter] : LSQ wsns ID wsns ASSIGN wsns expression wsns RSQ ;
    public EasyFlowParser.attribute_return attribute() // throws RecognitionException [1]
    {   
        EasyFlowParser.attribute_return retval = new EasyFlowParser.attribute_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken LSQ27 = null;
        IToken ID29 = null;
        IToken ASSIGN31 = null;
        IToken RSQ35 = null;
        EasyFlowParser.wsns_return wsns28 = default(EasyFlowParser.wsns_return);

        EasyFlowParser.wsns_return wsns30 = default(EasyFlowParser.wsns_return);

        EasyFlowParser.wsns_return wsns32 = default(EasyFlowParser.wsns_return);

        EasyFlowParser.expression_return expression33 = default(EasyFlowParser.expression_return);

        EasyFlowParser.wsns_return wsns34 = default(EasyFlowParser.wsns_return);


        object LSQ27_tree=null;
        object ID29_tree=null;
        object ASSIGN31_tree=null;
        object RSQ35_tree=null;



        try 
    	{
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:123:3: ( LSQ wsns ID wsns ASSIGN wsns expression wsns RSQ )
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:123:5: LSQ wsns ID wsns ASSIGN wsns expression wsns RSQ
            {
            	root_0 = (object)adaptor.GetNilNode();

            	LSQ27=(IToken)Match(input,LSQ,FOLLOW_LSQ_in_attribute352); 
            		LSQ27_tree = (object)adaptor.Create(LSQ27);
            		adaptor.AddChild(root_0, LSQ27_tree);

            	PushFollow(FOLLOW_wsns_in_attribute354);
            	wsns28 = wsns();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, wsns28.Tree);
            	ID29=(IToken)Match(input,ID,FOLLOW_ID_in_attribute356); 
            		ID29_tree = (object)adaptor.Create(ID29);
            		adaptor.AddChild(root_0, ID29_tree);

            	PushFollow(FOLLOW_wsns_in_attribute358);
            	wsns30 = wsns();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, wsns30.Tree);
            	ASSIGN31=(IToken)Match(input,ASSIGN,FOLLOW_ASSIGN_in_attribute360); 
            		ASSIGN31_tree = (object)adaptor.Create(ASSIGN31);
            		adaptor.AddChild(root_0, ASSIGN31_tree);

            	PushFollow(FOLLOW_wsns_in_attribute362);
            	wsns32 = wsns();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, wsns32.Tree);
            	PushFollow(FOLLOW_expression_in_attribute364);
            	expression33 = expression();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, expression33.Tree);
            	PushFollow(FOLLOW_wsns_in_attribute366);
            	wsns34 = wsns();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, wsns34.Tree);
            	RSQ35=(IToken)Match(input,RSQ,FOLLOW_RSQ_in_attribute368); 
            		RSQ35_tree = (object)adaptor.Create(RSQ35);
            		adaptor.AddChild(root_0, RSQ35_tree);


            	        retval.NamedParameter =  new NamedParameter(ID29.Text, ((expression33 != null) ? expression33.Expression : default(Expression)));       
            	        retval.NamedParameter.IsSweepParam = false;
            	        retval.NamedParameter.IsOntoParam = false;                  
            	    

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);

              FinishObj(retval.NamedParameter, retval);

        }

          catch (RecognitionException re)
          {
            throw re;
          }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "attribute"

    public class attributes_return : ParserRuleReturnScope
    {
        public NamedParameterCollection NamedParameters;
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "attributes"
    // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:131:1: attributes returns [NamedParameterCollection NamedParameters] : ( attribute wsns )* ;
    public EasyFlowParser.attributes_return attributes() // throws RecognitionException [1]
    {   
        EasyFlowParser.attributes_return retval = new EasyFlowParser.attributes_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        EasyFlowParser.attribute_return attribute36 = default(EasyFlowParser.attribute_return);

        EasyFlowParser.wsns_return wsns37 = default(EasyFlowParser.wsns_return);





        try 
    	{
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:140:3: ( ( attribute wsns )* )
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:140:5: ( attribute wsns )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	// D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:140:5: ( attribute wsns )*
            	do 
            	{
            	    int alt5 = 2;
            	    int LA5_0 = input.LA(1);

            	    if ( (LA5_0 == LSQ) )
            	    {
            	        alt5 = 1;
            	    }


            	    switch (alt5) 
            		{
            			case 1 :
            			    // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:140:6: attribute wsns
            			    {
            			    	PushFollow(FOLLOW_attribute_in_attributes406);
            			    	attribute36 = attribute();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, attribute36.Tree);

            			    		       if (retval.NamedParameters == null)
            			    		         retval.NamedParameters =  new NamedParameterCollection();
            			    		         
            			    		       retval.NamedParameters.Add(((attribute36 != null) ? attribute36.NamedParameter : default(NamedParameter)));
            			    		    
            			    	PushFollow(FOLLOW_wsns_in_attributes420);
            			    	wsns37 = wsns();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, wsns37.Tree);

            			    }
            			    break;

            			default:
            			    goto loop5;
            	    }
            	} while (true);

            	loop5:
            		;	// Stops C# compiler whining that label 'loop5' has no statements


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);

              if (retval.NamedParameters != null)  
                FinishObj(retval.NamedParameters, retval);

        }

          catch (RecognitionException re)
          {
            throw re;
          }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "attributes"

    public class stepClause_return : ParserRuleReturnScope
    {
        public StepBlock StepBlock;
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "stepClause"
    // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:152:1: stepClause returns [StepBlock StepBlock] : attributes ( TILDA )? STEP wsnp id wsnp RUNS wsnp packageName= compoundName ( wsnp AFTER wsnp idList )? wsns LPAREN wsns ( APP wsns COLON wsns )? (parameters= namedParametersList )? ( ( EXEC wsns COLON wsns ) (execParams= namedParametersList )? )? RPAREN ( wsns 'post' ( WS )+ CODE_BLOCK )? ( wsns SEMICOLON )? ;
    public EasyFlowParser.stepClause_return stepClause() // throws RecognitionException [1]
    {   
        EasyFlowParser.stepClause_return retval = new EasyFlowParser.stepClause_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken TILDA39 = null;
        IToken STEP40 = null;
        IToken RUNS44 = null;
        IToken AFTER47 = null;
        IToken LPAREN51 = null;
        IToken APP53 = null;
        IToken COLON55 = null;
        IToken EXEC57 = null;
        IToken COLON59 = null;
        IToken RPAREN61 = null;
        IToken string_literal63 = null;
        IToken WS64 = null;
        IToken CODE_BLOCK65 = null;
        IToken SEMICOLON67 = null;
        EasyFlowParser.compoundName_return packageName = default(EasyFlowParser.compoundName_return);

        EasyFlowParser.namedParametersList_return parameters = default(EasyFlowParser.namedParametersList_return);

        EasyFlowParser.namedParametersList_return execParams = default(EasyFlowParser.namedParametersList_return);

        EasyFlowParser.attributes_return attributes38 = default(EasyFlowParser.attributes_return);

        EasyFlowParser.wsnp_return wsnp41 = default(EasyFlowParser.wsnp_return);

        EasyFlowParser.id_return id42 = default(EasyFlowParser.id_return);

        EasyFlowParser.wsnp_return wsnp43 = default(EasyFlowParser.wsnp_return);

        EasyFlowParser.wsnp_return wsnp45 = default(EasyFlowParser.wsnp_return);

        EasyFlowParser.wsnp_return wsnp46 = default(EasyFlowParser.wsnp_return);

        EasyFlowParser.wsnp_return wsnp48 = default(EasyFlowParser.wsnp_return);

        EasyFlowParser.idList_return idList49 = default(EasyFlowParser.idList_return);

        EasyFlowParser.wsns_return wsns50 = default(EasyFlowParser.wsns_return);

        EasyFlowParser.wsns_return wsns52 = default(EasyFlowParser.wsns_return);

        EasyFlowParser.wsns_return wsns54 = default(EasyFlowParser.wsns_return);

        EasyFlowParser.wsns_return wsns56 = default(EasyFlowParser.wsns_return);

        EasyFlowParser.wsns_return wsns58 = default(EasyFlowParser.wsns_return);

        EasyFlowParser.wsns_return wsns60 = default(EasyFlowParser.wsns_return);

        EasyFlowParser.wsns_return wsns62 = default(EasyFlowParser.wsns_return);

        EasyFlowParser.wsns_return wsns66 = default(EasyFlowParser.wsns_return);


        object TILDA39_tree=null;
        object STEP40_tree=null;
        object RUNS44_tree=null;
        object AFTER47_tree=null;
        object LPAREN51_tree=null;
        object APP53_tree=null;
        object COLON55_tree=null;
        object EXEC57_tree=null;
        object COLON59_tree=null;
        object RPAREN61_tree=null;
        object string_literal63_tree=null;
        object WS64_tree=null;
        object CODE_BLOCK65_tree=null;
        object SEMICOLON67_tree=null;


          StartScope(ScopeType.Step); // Start Step scope
          
          retval.StepBlock =  new StepBlock(); // Create resulting Step block

        try 
    	{
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:183:3: ( attributes ( TILDA )? STEP wsnp id wsnp RUNS wsnp packageName= compoundName ( wsnp AFTER wsnp idList )? wsns LPAREN wsns ( APP wsns COLON wsns )? (parameters= namedParametersList )? ( ( EXEC wsns COLON wsns ) (execParams= namedParametersList )? )? RPAREN ( wsns 'post' ( WS )+ CODE_BLOCK )? ( wsns SEMICOLON )? )
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:184:5: attributes ( TILDA )? STEP wsnp id wsnp RUNS wsnp packageName= compoundName ( wsnp AFTER wsnp idList )? wsns LPAREN wsns ( APP wsns COLON wsns )? (parameters= namedParametersList )? ( ( EXEC wsns COLON wsns ) (execParams= namedParametersList )? )? RPAREN ( wsns 'post' ( WS )+ CODE_BLOCK )? ( wsns SEMICOLON )?
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_attributes_in_stepClause464);
            	attributes38 = attributes();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, attributes38.Tree);

            	      if (((attributes38 != null) ? attributes38.NamedParameters : default(NamedParameterCollection)) != null)
            	      {
            	        retval.StepBlock.ExecParameters.AddRange(((attributes38 != null) ? attributes38.NamedParameters : default(NamedParameterCollection)));
            	        FinishObj(retval.StepBlock.ExecParameters, ((attributes38 != null) ? attributes38.NamedParameters : default(NamedParameterCollection)));
            	      }
            	    
            	// D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:193:3: ( TILDA )?
            	int alt6 = 2;
            	int LA6_0 = input.LA(1);

            	if ( (LA6_0 == TILDA) )
            	{
            	    alt6 = 1;
            	}
            	switch (alt6) 
            	{
            	    case 1 :
            	        // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:193:4: TILDA
            	        {
            	        	TILDA39=(IToken)Match(input,TILDA,FOLLOW_TILDA_in_stepClause478); 
            	        		TILDA39_tree = (object)adaptor.Create(TILDA39);
            	        		adaptor.AddChild(root_0, TILDA39_tree);


            	        	      retval.StepBlock.IsLongRunning = true;
            	        	    

            	        }
            	        break;

            	}

            	STEP40=(IToken)Match(input,STEP,FOLLOW_STEP_in_stepClause494); 
            		STEP40_tree = (object)adaptor.Create(STEP40);
            		adaptor.AddChild(root_0, STEP40_tree);

            	 StartScope(ScopeType.StepHeader); 
            	PushFollow(FOLLOW_wsnp_in_stepClause498);
            	wsnp41 = wsnp();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, wsnp41.Tree);
            	PushFollow(FOLLOW_id_in_stepClause500);
            	id42 = id();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, id42.Tree);

            			  Identifier stepId = ((id42 != null) ? id42.Id : default(Identifier));
            			  stepId.IdentifierType = IdentifierType.StepName;                
            			  
            			  CheckAddIdentifierDef(stepId); // check and add first occurance of step name     
            			  
            			  // TODO: error check
            			  retval.StepBlock.Name = stepId.Name; // Assign step name     		 		  
            			
            	PushFollow(FOLLOW_wsnp_in_stepClause514);
            	wsnp43 = wsnp();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, wsnp43.Tree);
            	RUNS44=(IToken)Match(input,RUNS,FOLLOW_RUNS_in_stepClause516); 
            		RUNS44_tree = (object)adaptor.Create(RUNS44);
            		adaptor.AddChild(root_0, RUNS44_tree);

            	 StartScope(ScopeType.RunObjectName); 
            	PushFollow(FOLLOW_wsnp_in_stepClause520);
            	wsnp45 = wsnp();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, wsnp45.Tree);
            	PushFollow(FOLLOW_compoundName_in_stepClause524);
            	packageName = compoundName();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, packageName.Tree);

            			  retval.StepBlock.RunObjectName = new RunObjectName(((packageName != null) ? packageName.Locator : default(StringCompoundName)));
            			  
            			  FinishScope(); // Pop ScopeType.RunObjectName
            			  FinishScope(); // Pop ScopeType.StepHeader because here header ends
            			
            	// D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:218:4: ( wsnp AFTER wsnp idList )?
            	int alt7 = 2;
            	alt7 = dfa7.Predict(input);
            	switch (alt7) 
            	{
            	    case 1 :
            	        // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:218:5: wsnp AFTER wsnp idList
            	        {
            	        	PushFollow(FOLLOW_wsnp_in_stepClause540);
            	        	wsnp46 = wsnp();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, wsnp46.Tree);
            	        	AFTER47=(IToken)Match(input,AFTER,FOLLOW_AFTER_in_stepClause542); 
            	        		AFTER47_tree = (object)adaptor.Create(AFTER47);
            	        		adaptor.AddChild(root_0, AFTER47_tree);

            	        	 StartScope(ScopeType.StepAfterList); 
            	        	PushFollow(FOLLOW_wsnp_in_stepClause546);
            	        	wsnp48 = wsnp();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, wsnp48.Tree);
            	        	PushFollow(FOLLOW_idList_in_stepClause548);
            	        	idList49 = idList();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, idList49.Tree);

            	        				  foreach (var refStepId in ((idList49 != null) ? idList49.Identifiers : default(IEnumerable<Identifier>)))
            	        				  {
            	        				    TriggerForwardDefinition tfd = new TriggerForwardDefinition 
            	        				    {
            	        				      TriggerOwner = retval.StepBlock,
            	        				      TriggerName = TriggerActionDef.ACTION_START,
            	        				      EventSourceStepName = refStepId.Name,
            	        				      EventName = TriggerEventDef.FinishedEventName,
            	        				      IsExplicit = true
            	        				    };
            	        				    FinishObj(tfd, refStepId);
            	        				    AddFutureTrigger(tfd);
            	        				  }
            	        				  FinishScope(); // Pop ScopeType.StepAfterList
            	        				

            	        }
            	        break;

            	}

            	PushFollow(FOLLOW_wsns_in_stepClause575);
            	wsns50 = wsns();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, wsns50.Tree);
            	LPAREN51=(IToken)Match(input,LPAREN,FOLLOW_LPAREN_in_stepClause577); 
            		LPAREN51_tree = (object)adaptor.Create(LPAREN51);
            		adaptor.AddChild(root_0, LPAREN51_tree);

            	PushFollow(FOLLOW_wsns_in_stepClause579);
            	wsns52 = wsns();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, wsns52.Tree);
            	 
            			  StartScope(ScopeType.RunParameters);
            			  bool wasApp = false;
            			  bool wasExec = false;
            		  
            	// D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:243:5: ( APP wsns COLON wsns )?
            	int alt8 = 2;
            	int LA8_0 = input.LA(1);

            	if ( (LA8_0 == APP) )
            	{
            	    alt8 = 1;
            	}
            	switch (alt8) 
            	{
            	    case 1 :
            	        // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:243:6: APP wsns COLON wsns
            	        {
            	        	APP53=(IToken)Match(input,APP,FOLLOW_APP_in_stepClause590); 
            	        		APP53_tree = (object)adaptor.Create(APP53);
            	        		adaptor.AddChild(root_0, APP53_tree);

            	        	PushFollow(FOLLOW_wsns_in_stepClause592);
            	        	wsns54 = wsns();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, wsns54.Tree);
            	        	COLON55=(IToken)Match(input,COLON,FOLLOW_COLON_in_stepClause594); 
            	        		COLON55_tree = (object)adaptor.Create(COLON55);
            	        		adaptor.AddChild(root_0, COLON55_tree);

            	        	PushFollow(FOLLOW_wsns_in_stepClause596);
            	        	wsns56 = wsns();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, wsns56.Tree);

            	        }
            	        break;

            	}

            	// D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:243:38: (parameters= namedParametersList )?
            	int alt9 = 2;
            	int LA9_0 = input.LA(1);

            	if ( ((LA9_0 >= COLON && LA9_0 <= ID)) )
            	{
            	    alt9 = 1;
            	}
            	switch (alt9) 
            	{
            	    case 1 :
            	        // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:243:38: parameters= namedParametersList
            	        {
            	        	PushFollow(FOLLOW_namedParametersList_in_stepClause602);
            	        	parameters = namedParametersList();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, parameters.Tree);

            	        }
            	        break;

            	}


            			     wasApp = true;
            	         if (((parameters != null) ? parameters.NamedParameters : default(NamedParameterCollection)) != null)
            	           retval.StepBlock.RunParameters.Parameters = ((parameters != null) ? parameters.NamedParameters : default(NamedParameterCollection));         
            	      
            	// D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:249:4: ( ( EXEC wsns COLON wsns ) (execParams= namedParametersList )? )?
            	int alt11 = 2;
            	int LA11_0 = input.LA(1);

            	if ( (LA11_0 == EXEC) )
            	{
            	    alt11 = 1;
            	}
            	switch (alt11) 
            	{
            	    case 1 :
            	        // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:250:6: ( EXEC wsns COLON wsns ) (execParams= namedParametersList )?
            	        {
            	        	// D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:250:6: ( EXEC wsns COLON wsns )
            	        	// D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:250:7: EXEC wsns COLON wsns
            	        	{
            	        		EXEC57=(IToken)Match(input,EXEC,FOLLOW_EXEC_in_stepClause625); 
            	        			EXEC57_tree = (object)adaptor.Create(EXEC57);
            	        			adaptor.AddChild(root_0, EXEC57_tree);

            	        		PushFollow(FOLLOW_wsns_in_stepClause627);
            	        		wsns58 = wsns();
            	        		state.followingStackPointer--;

            	        		adaptor.AddChild(root_0, wsns58.Tree);
            	        		COLON59=(IToken)Match(input,COLON,FOLLOW_COLON_in_stepClause629); 
            	        			COLON59_tree = (object)adaptor.Create(COLON59);
            	        			adaptor.AddChild(root_0, COLON59_tree);

            	        		PushFollow(FOLLOW_wsns_in_stepClause631);
            	        		wsns60 = wsns();
            	        		state.followingStackPointer--;

            	        		adaptor.AddChild(root_0, wsns60.Tree);

            	        	}

            	        	// D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:250:39: (execParams= namedParametersList )?
            	        	int alt10 = 2;
            	        	int LA10_0 = input.LA(1);

            	        	if ( ((LA10_0 >= COLON && LA10_0 <= ID)) )
            	        	{
            	        	    alt10 = 1;
            	        	}
            	        	switch (alt10) 
            	        	{
            	        	    case 1 :
            	        	        // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:250:39: execParams= namedParametersList
            	        	        {
            	        	        	PushFollow(FOLLOW_namedParametersList_in_stepClause636);
            	        	        	execParams = namedParametersList();
            	        	        	state.followingStackPointer--;

            	        	        	adaptor.AddChild(root_0, execParams.Tree);

            	        	        }
            	        	        break;

            	        	}


            	        				     wasExec = true;
            	        				     if (((execParams != null) ? execParams.NamedParameters : default(NamedParameterCollection)) != null)
            	        				       retval.StepBlock.ExecParameters.Parameters.AddRange(((execParams != null) ? execParams.NamedParameters : default(NamedParameterCollection)));
            	        				  

            	        }
            	        break;

            	}

            	RPAREN61=(IToken)Match(input,RPAREN,FOLLOW_RPAREN_in_stepClause662); 
            		RPAREN61_tree = (object)adaptor.Create(RPAREN61);
            		adaptor.AddChild(root_0, RPAREN61_tree);


            	      FinishScope(); // Pop ScopeType.RunParameters
            	    
            	// D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:263:5: ( wsns 'post' ( WS )+ CODE_BLOCK )?
            	int alt13 = 2;
            	alt13 = dfa13.Predict(input);
            	switch (alt13) 
            	{
            	    case 1 :
            	        // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:263:6: wsns 'post' ( WS )+ CODE_BLOCK
            	        {
            	        	PushFollow(FOLLOW_wsns_in_stepClause681);
            	        	wsns62 = wsns();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, wsns62.Tree);
            	        	string_literal63=(IToken)Match(input,POST,FOLLOW_POST_in_stepClause683); 
            	        		string_literal63_tree = (object)adaptor.Create(string_literal63);
            	        		adaptor.AddChild(root_0, string_literal63_tree);

            	        	// D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:263:18: ( WS )+
            	        	int cnt12 = 0;
            	        	do 
            	        	{
            	        	    int alt12 = 2;
            	        	    int LA12_0 = input.LA(1);

            	        	    if ( (LA12_0 == WS) )
            	        	    {
            	        	        alt12 = 1;
            	        	    }


            	        	    switch (alt12) 
            	        		{
            	        			case 1 :
            	        			    // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:263:18: WS
            	        			    {
            	        			    	WS64=(IToken)Match(input,WS,FOLLOW_WS_in_stepClause685); 
            	        			    		WS64_tree = (object)adaptor.Create(WS64);
            	        			    		adaptor.AddChild(root_0, WS64_tree);


            	        			    }
            	        			    break;

            	        			default:
            	        			    if ( cnt12 >= 1 ) goto loop12;
            	        		            EarlyExitException eee12 =
            	        		                new EarlyExitException(12, input);
            	        		            throw eee12;
            	        	    }
            	        	    cnt12++;
            	        	} while (true);

            	        	loop12:
            	        		;	// Stops C# compiler whining that label 'loop12' has no statements

            	        	CODE_BLOCK65=(IToken)Match(input,CODE_BLOCK,FOLLOW_CODE_BLOCK_in_stepClause688); 
            	        		CODE_BLOCK65_tree = (object)adaptor.Create(CODE_BLOCK65);
            	        		adaptor.AddChild(root_0, CODE_BLOCK65_tree);

            	        		       
            	        		       retval.StepBlock.PostCodeSection = ((CODE_BLOCK65 != null) ? CODE_BLOCK65.Text : null);
            	        		    

            	        }
            	        break;

            	}

            	// D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:269:5: ( wsns SEMICOLON )?
            	int alt14 = 2;
            	alt14 = dfa14.Predict(input);
            	switch (alt14) 
            	{
            	    case 1 :
            	        // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:269:6: wsns SEMICOLON
            	        {
            	        	PushFollow(FOLLOW_wsns_in_stepClause714);
            	        	wsns66 = wsns();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, wsns66.Tree);
            	        	SEMICOLON67=(IToken)Match(input,SEMICOLON,FOLLOW_SEMICOLON_in_stepClause716); 
            	        		SEMICOLON67_tree = (object)adaptor.Create(SEMICOLON67);
            	        		adaptor.AddChild(root_0, SEMICOLON67_tree);


            	        }
            	        break;

            	}


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);


              bool isDisabled = false;
              var disabledParam = retval.StepBlock.ExecParameters.Parameters.FirstOrDefault(parameter => parameter.Name == "disabled");
              if (disabledParam != null)
              {
                 var expr = disabledParam.Value as LiteralExpression;

                 if (expr != null)
                 {
                    var boolValue = expr.Value as BoolValue;

                    if (boolValue != null)
                       isDisabled = boolValue.AsBool;
                 }
              }
              
              retval.StepBlock.IsDisabled = isDisabled;

              FinishObj(retval.StepBlock, retval); // Finish resulting Step block 
              AddBlock(retval.StepBlock);
              FinishScope(); // Finish ScopeType.Step scope

        }

          catch (RecognitionException re)
          {
            throw re;
          }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "stepClause"

    public class idList_return : ParserRuleReturnScope
    {
        public IEnumerable<Identifier> Identifiers;
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "idList"
    // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:287:1: idList returns [IEnumerable<Identifier> Identifiers] : first= ID ( wsns COMMA wsns next= ID )* ;
    public EasyFlowParser.idList_return idList() // throws RecognitionException [1]
    {   
        EasyFlowParser.idList_return retval = new EasyFlowParser.idList_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken first = null;
        IToken next = null;
        IToken COMMA69 = null;
        EasyFlowParser.wsns_return wsns68 = default(EasyFlowParser.wsns_return);

        EasyFlowParser.wsns_return wsns70 = default(EasyFlowParser.wsns_return);


        object first_tree=null;
        object next_tree=null;
        object COMMA69_tree=null;


          var list = new List<Identifier>();  

        try 
    	{
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:296:3: (first= ID ( wsns COMMA wsns next= ID )* )
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:296:5: first= ID ( wsns COMMA wsns next= ID )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	first=(IToken)Match(input,ID,FOLLOW_ID_in_idList797); 
            		first_tree = (object)adaptor.Create(first);
            		adaptor.AddChild(root_0, first_tree);


            	      list.Add(new Identifier(((first != null) ? first.Text : null)));      
            	    
            	// D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:300:5: ( wsns COMMA wsns next= ID )*
            	do 
            	{
            	    int alt15 = 2;
            	    alt15 = dfa15.Predict(input);
            	    switch (alt15) 
            		{
            			case 1 :
            			    // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:300:6: wsns COMMA wsns next= ID
            			    {
            			    	PushFollow(FOLLOW_wsns_in_idList810);
            			    	wsns68 = wsns();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, wsns68.Tree);
            			    	COMMA69=(IToken)Match(input,COMMA,FOLLOW_COMMA_in_idList812); 
            			    		COMMA69_tree = (object)adaptor.Create(COMMA69);
            			    		adaptor.AddChild(root_0, COMMA69_tree);

            			    	PushFollow(FOLLOW_wsns_in_idList814);
            			    	wsns70 = wsns();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, wsns70.Tree);
            			    	next=(IToken)Match(input,ID,FOLLOW_ID_in_idList818); 
            			    		next_tree = (object)adaptor.Create(next);
            			    		adaptor.AddChild(root_0, next_tree);


            			    	        list.Add(new Identifier(((next != null) ? next.Text : null)));       
            			    	      

            			    }
            			    break;

            			default:
            			    goto loop15;
            	    }
            	} while (true);

            	loop15:
            		;	// Stops C# compiler whining that label 'loop15' has no statements


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);

              retval.Identifiers =  list;

        }

          catch (RecognitionException re)
          {
            throw re;
          }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "idList"

    public class codeBlock_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "codeBlock"
    // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:310:1: codeBlock : ( statement wsns SEMICOLON wsns )+ ;
    public EasyFlowParser.codeBlock_return codeBlock() // throws RecognitionException [1]
    {   
        EasyFlowParser.codeBlock_return retval = new EasyFlowParser.codeBlock_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken SEMICOLON73 = null;
        EasyFlowParser.statement_return statement71 = default(EasyFlowParser.statement_return);

        EasyFlowParser.wsns_return wsns72 = default(EasyFlowParser.wsns_return);

        EasyFlowParser.wsns_return wsns74 = default(EasyFlowParser.wsns_return);


        object SEMICOLON73_tree=null;

        try 
    	{
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:311:3: ( ( statement wsns SEMICOLON wsns )+ )
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:311:5: ( statement wsns SEMICOLON wsns )+
            {
            	root_0 = (object)adaptor.GetNilNode();

            	// D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:311:5: ( statement wsns SEMICOLON wsns )+
            	int cnt16 = 0;
            	do 
            	{
            	    int alt16 = 2;
            	    int LA16_0 = input.LA(1);

            	    if ( (LA16_0 == ID) )
            	    {
            	        alt16 = 1;
            	    }


            	    switch (alt16) 
            		{
            			case 1 :
            			    // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:311:6: statement wsns SEMICOLON wsns
            			    {
            			    	PushFollow(FOLLOW_statement_in_codeBlock858);
            			    	statement71 = statement();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, statement71.Tree);
            			    	PushFollow(FOLLOW_wsns_in_codeBlock860);
            			    	wsns72 = wsns();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, wsns72.Tree);
            			    	SEMICOLON73=(IToken)Match(input,SEMICOLON,FOLLOW_SEMICOLON_in_codeBlock862); 
            			    		SEMICOLON73_tree = (object)adaptor.Create(SEMICOLON73);
            			    		adaptor.AddChild(root_0, SEMICOLON73_tree);

            			    	PushFollow(FOLLOW_wsns_in_codeBlock864);
            			    	wsns74 = wsns();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, wsns74.Tree);

            			    }
            			    break;

            			default:
            			    if ( cnt16 >= 1 ) goto loop16;
            		            EarlyExitException eee16 =
            		                new EarlyExitException(16, input);
            		            throw eee16;
            	    }
            	    cnt16++;
            	} while (true);

            	loop16:
            		;	// Stops C# compiler whining that label 'loop16' has no statements


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }

          catch (RecognitionException re)
          {
            throw re;
          }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "codeBlock"

    public class statement_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "statement"
    // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:314:1: statement : assignment ;
    public EasyFlowParser.statement_return statement() // throws RecognitionException [1]
    {   
        EasyFlowParser.statement_return retval = new EasyFlowParser.statement_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        EasyFlowParser.assignment_return assignment75 = default(EasyFlowParser.assignment_return);



        try 
    	{
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:315:3: ( assignment )
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:315:5: assignment
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_assignment_in_statement881);
            	assignment75 = assignment();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, assignment75.Tree);

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }

          catch (RecognitionException re)
          {
            throw re;
          }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "statement"

    public class assignment_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "assignment"
    // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:318:1: assignment : varIdentifier wsns ASSIGN wsns expression ;
    public EasyFlowParser.assignment_return assignment() // throws RecognitionException [1]
    {   
        EasyFlowParser.assignment_return retval = new EasyFlowParser.assignment_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken ASSIGN78 = null;
        EasyFlowParser.varIdentifier_return varIdentifier76 = default(EasyFlowParser.varIdentifier_return);

        EasyFlowParser.wsns_return wsns77 = default(EasyFlowParser.wsns_return);

        EasyFlowParser.wsns_return wsns79 = default(EasyFlowParser.wsns_return);

        EasyFlowParser.expression_return expression80 = default(EasyFlowParser.expression_return);


        object ASSIGN78_tree=null;

        try 
    	{
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:319:3: ( varIdentifier wsns ASSIGN wsns expression )
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:319:5: varIdentifier wsns ASSIGN wsns expression
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_varIdentifier_in_assignment896);
            	varIdentifier76 = varIdentifier();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, varIdentifier76.Tree);
            	PushFollow(FOLLOW_wsns_in_assignment898);
            	wsns77 = wsns();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, wsns77.Tree);
            	ASSIGN78=(IToken)Match(input,ASSIGN,FOLLOW_ASSIGN_in_assignment900); 
            		ASSIGN78_tree = (object)adaptor.Create(ASSIGN78);
            		adaptor.AddChild(root_0, ASSIGN78_tree);

            	PushFollow(FOLLOW_wsns_in_assignment902);
            	wsns79 = wsns();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, wsns79.Tree);
            	PushFollow(FOLLOW_expression_in_assignment904);
            	expression80 = expression();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, expression80.Tree);

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }

          catch (RecognitionException re)
          {
            throw re;
          }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "assignment"

    public class varIdentifierList_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "varIdentifierList"
    // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:324:1: varIdentifierList : varIdentifier ( wsns COMMA wsns varIdentifier )* ;
    public EasyFlowParser.varIdentifierList_return varIdentifierList() // throws RecognitionException [1]
    {   
        EasyFlowParser.varIdentifierList_return retval = new EasyFlowParser.varIdentifierList_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken COMMA83 = null;
        EasyFlowParser.varIdentifier_return varIdentifier81 = default(EasyFlowParser.varIdentifier_return);

        EasyFlowParser.wsns_return wsns82 = default(EasyFlowParser.wsns_return);

        EasyFlowParser.wsns_return wsns84 = default(EasyFlowParser.wsns_return);

        EasyFlowParser.varIdentifier_return varIdentifier85 = default(EasyFlowParser.varIdentifier_return);


        object COMMA83_tree=null;

        try 
    	{
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:325:3: ( varIdentifier ( wsns COMMA wsns varIdentifier )* )
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:325:5: varIdentifier ( wsns COMMA wsns varIdentifier )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_varIdentifier_in_varIdentifierList923);
            	varIdentifier81 = varIdentifier();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, varIdentifier81.Tree);
            	// D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:325:19: ( wsns COMMA wsns varIdentifier )*
            	do 
            	{
            	    int alt17 = 2;
            	    int LA17_0 = input.LA(1);

            	    if ( ((LA17_0 >= WS && LA17_0 <= NEWLINE) || LA17_0 == COMMA) )
            	    {
            	        alt17 = 1;
            	    }


            	    switch (alt17) 
            		{
            			case 1 :
            			    // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:325:20: wsns COMMA wsns varIdentifier
            			    {
            			    	PushFollow(FOLLOW_wsns_in_varIdentifierList926);
            			    	wsns82 = wsns();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, wsns82.Tree);
            			    	COMMA83=(IToken)Match(input,COMMA,FOLLOW_COMMA_in_varIdentifierList928); 
            			    		COMMA83_tree = (object)adaptor.Create(COMMA83);
            			    		adaptor.AddChild(root_0, COMMA83_tree);

            			    	PushFollow(FOLLOW_wsns_in_varIdentifierList930);
            			    	wsns84 = wsns();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, wsns84.Tree);
            			    	PushFollow(FOLLOW_varIdentifier_in_varIdentifierList932);
            			    	varIdentifier85 = varIdentifier();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, varIdentifier85.Tree);

            			    }
            			    break;

            			default:
            			    goto loop17;
            	    }
            	} while (true);

            	loop17:
            		;	// Stops C# compiler whining that label 'loop17' has no statements


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }

          catch (RecognitionException re)
          {
            throw re;
          }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "varIdentifierList"

    public class varIdentifier_return : ParserRuleReturnScope
    {
        public CompoundVarIdentifier VarIdentifier;
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "varIdentifier"
    // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:328:1: varIdentifier returns [CompoundVarIdentifier VarIdentifier] : first= simpleVarIdentifier ( wsns DOT wsns next= simpleVarIdentifier )* ;
    public EasyFlowParser.varIdentifier_return varIdentifier() // throws RecognitionException [1]
    {   
        EasyFlowParser.varIdentifier_return retval = new EasyFlowParser.varIdentifier_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken DOT87 = null;
        EasyFlowParser.simpleVarIdentifier_return first = default(EasyFlowParser.simpleVarIdentifier_return);

        EasyFlowParser.simpleVarIdentifier_return next = default(EasyFlowParser.simpleVarIdentifier_return);

        EasyFlowParser.wsns_return wsns86 = default(EasyFlowParser.wsns_return);

        EasyFlowParser.wsns_return wsns88 = default(EasyFlowParser.wsns_return);


        object DOT87_tree=null;


          StartScope(ScopeType.VarIdentifier);
          retval.VarIdentifier =  new CompoundVarIdentifier();

        try 
    	{
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:339:3: (first= simpleVarIdentifier ( wsns DOT wsns next= simpleVarIdentifier )* )
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:339:5: first= simpleVarIdentifier ( wsns DOT wsns next= simpleVarIdentifier )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_simpleVarIdentifier_in_varIdentifier971);
            	first = simpleVarIdentifier();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, first.Tree);

            	      retval.VarIdentifier.AddPart(((first != null) ? first.SimpleVarIdentifier : default(SimpleVarIdentifier)));
            	    
            	// D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:343:5: ( wsns DOT wsns next= simpleVarIdentifier )*
            	do 
            	{
            	    int alt18 = 2;
            	    alt18 = dfa18.Predict(input);
            	    switch (alt18) 
            		{
            			case 1 :
            			    // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:343:7: wsns DOT wsns next= simpleVarIdentifier
            			    {
            			    	PushFollow(FOLLOW_wsns_in_varIdentifier985);
            			    	wsns86 = wsns();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, wsns86.Tree);
            			    	DOT87=(IToken)Match(input,DOT,FOLLOW_DOT_in_varIdentifier987); 
            			    		DOT87_tree = (object)adaptor.Create(DOT87);
            			    		adaptor.AddChild(root_0, DOT87_tree);

            			    	PushFollow(FOLLOW_wsns_in_varIdentifier989);
            			    	wsns88 = wsns();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, wsns88.Tree);
            			    	PushFollow(FOLLOW_simpleVarIdentifier_in_varIdentifier993);
            			    	next = simpleVarIdentifier();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, next.Tree);

            			    	        retval.VarIdentifier.AddPart(((next != null) ? next.SimpleVarIdentifier : default(SimpleVarIdentifier)));
            			    	      

            			    }
            			    break;

            			default:
            			    goto loop18;
            	    }
            	} while (true);

            	loop18:
            		;	// Stops C# compiler whining that label 'loop18' has no statements


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);

              FinishObj(retval.VarIdentifier, retval);
              FinishScope();

        }

          catch (RecognitionException re)
          {
            throw re;
          }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "varIdentifier"

    public class simpleVarIdentifier_return : ParserRuleReturnScope
    {
        public SimpleVarIdentifier SimpleVarIdentifier;
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "simpleVarIdentifier"
    // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:351:1: simpleVarIdentifier returns [SimpleVarIdentifier SimpleVarIdentifier] : ID ( wsns varIndexer )? ;
    public EasyFlowParser.simpleVarIdentifier_return simpleVarIdentifier() // throws RecognitionException [1]
    {   
        EasyFlowParser.simpleVarIdentifier_return retval = new EasyFlowParser.simpleVarIdentifier_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken ID89 = null;
        EasyFlowParser.wsns_return wsns90 = default(EasyFlowParser.wsns_return);

        EasyFlowParser.varIndexer_return varIndexer91 = default(EasyFlowParser.varIndexer_return);


        object ID89_tree=null;

        try 
    	{
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:356:3: ( ID ( wsns varIndexer )? )
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:356:5: ID ( wsns varIndexer )?
            {
            	root_0 = (object)adaptor.GetNilNode();

            	ID89=(IToken)Match(input,ID,FOLLOW_ID_in_simpleVarIdentifier1037); 
            		ID89_tree = (object)adaptor.Create(ID89);
            		adaptor.AddChild(root_0, ID89_tree);

            	// D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:356:8: ( wsns varIndexer )?
            	int alt19 = 2;
            	alt19 = dfa19.Predict(input);
            	switch (alt19) 
            	{
            	    case 1 :
            	        // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:356:9: wsns varIndexer
            	        {
            	        	PushFollow(FOLLOW_wsns_in_simpleVarIdentifier1040);
            	        	wsns90 = wsns();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, wsns90.Tree);
            	        	PushFollow(FOLLOW_varIndexer_in_simpleVarIdentifier1042);
            	        	varIndexer91 = varIndexer();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, varIndexer91.Tree);

            	        }
            	        break;

            	}


            	      retval.SimpleVarIdentifier =  new SimpleVarIdentifier(((ID89 != null) ? ID89.Text : null), ((varIndexer91 != null) ? varIndexer91.Indexer : default(Indexer))); 
            	    

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);

              FinishObj(retval.SimpleVarIdentifier, retval);  

        }

          catch (RecognitionException re)
          {
            throw re;
          }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "simpleVarIdentifier"

    public class varName_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "varName"
    // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:362:1: varName : ID ;
    public EasyFlowParser.varName_return varName() // throws RecognitionException [1]
    {   
        EasyFlowParser.varName_return retval = new EasyFlowParser.varName_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken ID92 = null;

        object ID92_tree=null;

        try 
    	{
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:363:3: ( ID )
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:363:5: ID
            {
            	root_0 = (object)adaptor.GetNilNode();

            	ID92=(IToken)Match(input,ID,FOLLOW_ID_in_varName1071); 
            		ID92_tree = (object)adaptor.Create(ID92);
            		adaptor.AddChild(root_0, ID92_tree);


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }

          catch (RecognitionException re)
          {
            throw re;
          }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "varName"

    public class varIndexer_return : ParserRuleReturnScope
    {
        public Indexer Indexer;
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "varIndexer"
    // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:366:1: varIndexer returns [Indexer Indexer] : LSQ wsns expression wsns RSQ ;
    public EasyFlowParser.varIndexer_return varIndexer() // throws RecognitionException [1]
    {   
        EasyFlowParser.varIndexer_return retval = new EasyFlowParser.varIndexer_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken LSQ93 = null;
        IToken RSQ97 = null;
        EasyFlowParser.wsns_return wsns94 = default(EasyFlowParser.wsns_return);

        EasyFlowParser.expression_return expression95 = default(EasyFlowParser.expression_return);

        EasyFlowParser.wsns_return wsns96 = default(EasyFlowParser.wsns_return);


        object LSQ93_tree=null;
        object RSQ97_tree=null;

        try 
    	{
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:371:3: ( LSQ wsns expression wsns RSQ )
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:371:5: LSQ wsns expression wsns RSQ
            {
            	root_0 = (object)adaptor.GetNilNode();

            	LSQ93=(IToken)Match(input,LSQ,FOLLOW_LSQ_in_varIndexer1095); 
            		LSQ93_tree = (object)adaptor.Create(LSQ93);
            		adaptor.AddChild(root_0, LSQ93_tree);

            	PushFollow(FOLLOW_wsns_in_varIndexer1097);
            	wsns94 = wsns();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, wsns94.Tree);
            	PushFollow(FOLLOW_expression_in_varIndexer1099);
            	expression95 = expression();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, expression95.Tree);
            	PushFollow(FOLLOW_wsns_in_varIndexer1101);
            	wsns96 = wsns();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, wsns96.Tree);
            	RSQ97=(IToken)Match(input,RSQ,FOLLOW_RSQ_in_varIndexer1103); 
            		RSQ97_tree = (object)adaptor.Create(RSQ97);
            		adaptor.AddChild(root_0, RSQ97_tree);


            	      retval.Indexer =  new Indexer(((expression95 != null) ? expression95.Expression : default(Expression)));
            	    

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);

              FinishObj(retval.Indexer, retval);

        }

          catch (RecognitionException re)
          {
            throw re;
          }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "varIndexer"

    public class filePointer_return : ParserRuleReturnScope
    {
        public FileDescriptor FilePointer;
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "filePointer"
    // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:377:1: filePointer returns [FileDescriptor FilePointer] : ID ;
    public EasyFlowParser.filePointer_return filePointer() // throws RecognitionException [1]
    {   
        EasyFlowParser.filePointer_return retval = new EasyFlowParser.filePointer_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken ID98 = null;

        object ID98_tree=null;


          StartScope(ScopeType.FilePointer);

        try 
    	{
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:387:3: ( ID )
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:387:5: ID
            {
            	root_0 = (object)adaptor.GetNilNode();

            	ID98=(IToken)Match(input,ID,FOLLOW_ID_in_filePointer1140); 
            		ID98_tree = (object)adaptor.Create(ID98);
            		adaptor.AddChild(root_0, ID98_tree);


            	      retval.FilePointer =  new FileDescriptor(null, ((ID98 != null) ? ID98.Text : null));
            	    

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);

              FinishObj(retval.FilePointer, retval);
              FinishScope();

        }

          catch (RecognitionException re)
          {
            throw re;
          }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "filePointer"

    public class compoundNameList_return : ParserRuleReturnScope
    {
        public IEnumerable<StringCompoundName> LocatorList;
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "compoundNameList"
    // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:393:1: compoundNameList returns [IEnumerable<StringCompoundName> LocatorList] : compoundName ( wsns COMMA wsns compoundName ) ;
    public EasyFlowParser.compoundNameList_return compoundNameList() // throws RecognitionException [1]
    {   
        EasyFlowParser.compoundNameList_return retval = new EasyFlowParser.compoundNameList_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken COMMA101 = null;
        EasyFlowParser.compoundName_return compoundName99 = default(EasyFlowParser.compoundName_return);

        EasyFlowParser.wsns_return wsns100 = default(EasyFlowParser.wsns_return);

        EasyFlowParser.wsns_return wsns102 = default(EasyFlowParser.wsns_return);

        EasyFlowParser.compoundName_return compoundName103 = default(EasyFlowParser.compoundName_return);


        object COMMA101_tree=null;

        try 
    	{
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:394:3: ( compoundName ( wsns COMMA wsns compoundName ) )
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:394:5: compoundName ( wsns COMMA wsns compoundName )
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_compoundName_in_compoundNameList1171);
            	compoundName99 = compoundName();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, compoundName99.Tree);
            	// D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:394:18: ( wsns COMMA wsns compoundName )
            	// D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:394:19: wsns COMMA wsns compoundName
            	{
            		PushFollow(FOLLOW_wsns_in_compoundNameList1174);
            		wsns100 = wsns();
            		state.followingStackPointer--;

            		adaptor.AddChild(root_0, wsns100.Tree);
            		COMMA101=(IToken)Match(input,COMMA,FOLLOW_COMMA_in_compoundNameList1176); 
            			COMMA101_tree = (object)adaptor.Create(COMMA101);
            			adaptor.AddChild(root_0, COMMA101_tree);

            		PushFollow(FOLLOW_wsns_in_compoundNameList1178);
            		wsns102 = wsns();
            		state.followingStackPointer--;

            		adaptor.AddChild(root_0, wsns102.Tree);
            		PushFollow(FOLLOW_compoundName_in_compoundNameList1180);
            		compoundName103 = compoundName();
            		state.followingStackPointer--;

            		adaptor.AddChild(root_0, compoundName103.Tree);

            	}


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }

          catch (RecognitionException re)
          {
            throw re;
          }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "compoundNameList"

    public class compoundName_return : ParserRuleReturnScope
    {
        public StringCompoundName Locator;
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "compoundName"
    // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:397:1: compoundName returns [StringCompoundName Locator] : first= compoundNameComponent ( wsns DOT wsns next= compoundNameComponent )* ;
    public EasyFlowParser.compoundName_return compoundName() // throws RecognitionException [1]
    {   
        EasyFlowParser.compoundName_return retval = new EasyFlowParser.compoundName_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken DOT105 = null;
        EasyFlowParser.compoundNameComponent_return first = default(EasyFlowParser.compoundNameComponent_return);

        EasyFlowParser.compoundNameComponent_return next = default(EasyFlowParser.compoundNameComponent_return);

        EasyFlowParser.wsns_return wsns104 = default(EasyFlowParser.wsns_return);

        EasyFlowParser.wsns_return wsns106 = default(EasyFlowParser.wsns_return);


        object DOT105_tree=null;


          retval.Locator =  new StringCompoundName();  

        try 
    	{
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:406:3: (first= compoundNameComponent ( wsns DOT wsns next= compoundNameComponent )* )
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:406:5: first= compoundNameComponent ( wsns DOT wsns next= compoundNameComponent )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_compoundNameComponent_in_compoundName1215);
            	first = compoundNameComponent();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, first.Tree);
            	 
            	      retval.Locator.AddPart(((first != null) ? first.Identifier : default(Identifier)).Name);
            	    
            	// D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:410:5: ( wsns DOT wsns next= compoundNameComponent )*
            	do 
            	{
            	    int alt20 = 2;
            	    alt20 = dfa20.Predict(input);
            	    switch (alt20) 
            		{
            			case 1 :
            			    // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:410:7: wsns DOT wsns next= compoundNameComponent
            			    {
            			    	PushFollow(FOLLOW_wsns_in_compoundName1229);
            			    	wsns104 = wsns();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, wsns104.Tree);
            			    	DOT105=(IToken)Match(input,DOT,FOLLOW_DOT_in_compoundName1231); 
            			    		DOT105_tree = (object)adaptor.Create(DOT105);
            			    		adaptor.AddChild(root_0, DOT105_tree);

            			    	PushFollow(FOLLOW_wsns_in_compoundName1233);
            			    	wsns106 = wsns();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, wsns106.Tree);
            			    	PushFollow(FOLLOW_compoundNameComponent_in_compoundName1237);
            			    	next = compoundNameComponent();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, next.Tree);

            			    	        retval.Locator.AddPart(((next != null) ? next.Identifier : default(Identifier)).Name);
            			    	      

            			    }
            			    break;

            			default:
            			    goto loop20;
            	    }
            	} while (true);

            	loop20:
            		;	// Stops C# compiler whining that label 'loop20' has no statements


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);

              FinishObj(retval.Locator, retval);

        }

          catch (RecognitionException re)
          {
            throw re;
          }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "compoundName"

    public class compoundNameComponent_return : ParserRuleReturnScope
    {
        public Identifier Identifier;
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "compoundNameComponent"
    // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:417:1: compoundNameComponent returns [Identifier Identifier] : ID ;
    public EasyFlowParser.compoundNameComponent_return compoundNameComponent() // throws RecognitionException [1]
    {   
        EasyFlowParser.compoundNameComponent_return retval = new EasyFlowParser.compoundNameComponent_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken ID107 = null;

        object ID107_tree=null;

        try 
    	{
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:422:3: ( ID )
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:422:5: ID
            {
            	root_0 = (object)adaptor.GetNilNode();

            	ID107=(IToken)Match(input,ID,FOLLOW_ID_in_compoundNameComponent1276); 
            		ID107_tree = (object)adaptor.Create(ID107);
            		adaptor.AddChild(root_0, ID107_tree);


            	      retval.Identifier =  new Identifier(((ID107 != null) ? ID107.Text : null));
            	    

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);

              FinishObj(retval.Identifier, retval);

        }

          catch (RecognitionException re)
          {
            throw re;
          }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "compoundNameComponent"

    public class namedParametersList_return : ParserRuleReturnScope
    {
        public NamedParameterCollection NamedParameters;
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "namedParametersList"
    // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:428:1: namedParametersList returns [NamedParameterCollection NamedParameters] : first= namedParameter wsns ( ( ( COMMA wsns next= namedParameter wsns )+ ( COMMA wsns )? ) | ( COMMA wsns )? ) ;
    public EasyFlowParser.namedParametersList_return namedParametersList() // throws RecognitionException [1]
    {   
        EasyFlowParser.namedParametersList_return retval = new EasyFlowParser.namedParametersList_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken COMMA109 = null;
        IToken COMMA112 = null;
        IToken COMMA114 = null;
        EasyFlowParser.namedParameter_return first = default(EasyFlowParser.namedParameter_return);

        EasyFlowParser.namedParameter_return next = default(EasyFlowParser.namedParameter_return);

        EasyFlowParser.wsns_return wsns108 = default(EasyFlowParser.wsns_return);

        EasyFlowParser.wsns_return wsns110 = default(EasyFlowParser.wsns_return);

        EasyFlowParser.wsns_return wsns111 = default(EasyFlowParser.wsns_return);

        EasyFlowParser.wsns_return wsns113 = default(EasyFlowParser.wsns_return);

        EasyFlowParser.wsns_return wsns115 = default(EasyFlowParser.wsns_return);


        object COMMA109_tree=null;
        object COMMA112_tree=null;
        object COMMA114_tree=null;

          
          retval.NamedParameters =  new NamedParameterCollection();

        try 
    	{
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:437:3: (first= namedParameter wsns ( ( ( COMMA wsns next= namedParameter wsns )+ ( COMMA wsns )? ) | ( COMMA wsns )? ) )
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:437:5: first= namedParameter wsns ( ( ( COMMA wsns next= namedParameter wsns )+ ( COMMA wsns )? ) | ( COMMA wsns )? )
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_namedParameter_in_namedParametersList1314);
            	first = namedParameter();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, first.Tree);
            	PushFollow(FOLLOW_wsns_in_namedParametersList1316);
            	wsns108 = wsns();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, wsns108.Tree);
            	 
            	      retval.NamedParameters.Add(((first != null) ? first.NamedParameter : default(NamedParameter)));
            	    
            	// D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:441:5: ( ( ( COMMA wsns next= namedParameter wsns )+ ( COMMA wsns )? ) | ( COMMA wsns )? )
            	int alt24 = 2;
            	alt24 = dfa24.Predict(input);
            	switch (alt24) 
            	{
            	    case 1 :
            	        // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:441:6: ( ( COMMA wsns next= namedParameter wsns )+ ( COMMA wsns )? )
            	        {
            	        	// D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:441:6: ( ( COMMA wsns next= namedParameter wsns )+ ( COMMA wsns )? )
            	        	// D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:442:7: ( COMMA wsns next= namedParameter wsns )+ ( COMMA wsns )?
            	        	{
            	        		// D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:442:7: ( COMMA wsns next= namedParameter wsns )+
            	        		int cnt21 = 0;
            	        		do 
            	        		{
            	        		    int alt21 = 2;
            	        		    alt21 = dfa21.Predict(input);
            	        		    switch (alt21) 
            	        			{
            	        				case 1 :
            	        				    // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:443:9: COMMA wsns next= namedParameter wsns
            	        				    {
            	        				    	COMMA109=(IToken)Match(input,COMMA,FOLLOW_COMMA_in_namedParametersList1349); 
            	        				    		COMMA109_tree = (object)adaptor.Create(COMMA109);
            	        				    		adaptor.AddChild(root_0, COMMA109_tree);

            	        				    	PushFollow(FOLLOW_wsns_in_namedParametersList1351);
            	        				    	wsns110 = wsns();
            	        				    	state.followingStackPointer--;

            	        				    	adaptor.AddChild(root_0, wsns110.Tree);
            	        				    	PushFollow(FOLLOW_namedParameter_in_namedParametersList1355);
            	        				    	next = namedParameter();
            	        				    	state.followingStackPointer--;

            	        				    	adaptor.AddChild(root_0, next.Tree);
            	        				    	PushFollow(FOLLOW_wsns_in_namedParametersList1357);
            	        				    	wsns111 = wsns();
            	        				    	state.followingStackPointer--;

            	        				    	adaptor.AddChild(root_0, wsns111.Tree);

            	        				    	          retval.NamedParameters.Add(((next != null) ? next.NamedParameter : default(NamedParameter)));
            	        				    	        

            	        				    }
            	        				    break;

            	        				default:
            	        				    if ( cnt21 >= 1 ) goto loop21;
            	        			            EarlyExitException eee21 =
            	        			                new EarlyExitException(21, input);
            	        			            throw eee21;
            	        		    }
            	        		    cnt21++;
            	        		} while (true);

            	        		loop21:
            	        			;	// Stops C# compiler whining that label 'loop21' has no statements

            	        		// D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:448:7: ( COMMA wsns )?
            	        		int alt22 = 2;
            	        		int LA22_0 = input.LA(1);

            	        		if ( (LA22_0 == COMMA) )
            	        		{
            	        		    alt22 = 1;
            	        		}
            	        		switch (alt22) 
            	        		{
            	        		    case 1 :
            	        		        // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:448:8: COMMA wsns
            	        		        {
            	        		        	COMMA112=(IToken)Match(input,COMMA,FOLLOW_COMMA_in_namedParametersList1394); 
            	        		        		COMMA112_tree = (object)adaptor.Create(COMMA112);
            	        		        		adaptor.AddChild(root_0, COMMA112_tree);

            	        		        	PushFollow(FOLLOW_wsns_in_namedParametersList1396);
            	        		        	wsns113 = wsns();
            	        		        	state.followingStackPointer--;

            	        		        	adaptor.AddChild(root_0, wsns113.Tree);

            	        		        }
            	        		        break;

            	        		}


            	        	}


            	        }
            	        break;
            	    case 2 :
            	        // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:449:9: ( COMMA wsns )?
            	        {
            	        	// D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:449:9: ( COMMA wsns )?
            	        	int alt23 = 2;
            	        	int LA23_0 = input.LA(1);

            	        	if ( (LA23_0 == COMMA) )
            	        	{
            	        	    alt23 = 1;
            	        	}
            	        	switch (alt23) 
            	        	{
            	        	    case 1 :
            	        	        // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:449:10: COMMA wsns
            	        	        {
            	        	        	COMMA114=(IToken)Match(input,COMMA,FOLLOW_COMMA_in_namedParametersList1410); 
            	        	        		COMMA114_tree = (object)adaptor.Create(COMMA114);
            	        	        		adaptor.AddChild(root_0, COMMA114_tree);

            	        	        	PushFollow(FOLLOW_wsns_in_namedParametersList1412);
            	        	        	wsns115 = wsns();
            	        	        	state.followingStackPointer--;

            	        	        	adaptor.AddChild(root_0, wsns115.Tree);

            	        	        }
            	        	        break;

            	        	}


            	        }
            	        break;

            	}


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);

              FinishObj(retval.NamedParameters, retval); 

        }

          catch (RecognitionException re)
          {
            throw re;
          }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "namedParametersList"

    public class namedParameter_return : ParserRuleReturnScope
    {
        public NamedParameter NamedParameter;
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "namedParameter"
    // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:453:1: namedParameter returns [NamedParameter NamedParameter] : ( COLON )? ID wsns sign= ( ASSIGN | ARROW ) wsns ( SWEEP wsns )? expression ;
    public EasyFlowParser.namedParameter_return namedParameter() // throws RecognitionException [1]
    {   
        EasyFlowParser.namedParameter_return retval = new EasyFlowParser.namedParameter_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken sign = null;
        IToken COLON116 = null;
        IToken ID117 = null;
        IToken SWEEP120 = null;
        EasyFlowParser.wsns_return wsns118 = default(EasyFlowParser.wsns_return);

        EasyFlowParser.wsns_return wsns119 = default(EasyFlowParser.wsns_return);

        EasyFlowParser.wsns_return wsns121 = default(EasyFlowParser.wsns_return);

        EasyFlowParser.expression_return expression122 = default(EasyFlowParser.expression_return);


        object sign_tree=null;
        object COLON116_tree=null;
        object ID117_tree=null;
        object SWEEP120_tree=null;

        try 
    	{
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:458:3: ( ( COLON )? ID wsns sign= ( ASSIGN | ARROW ) wsns ( SWEEP wsns )? expression )
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:458:5: ( COLON )? ID wsns sign= ( ASSIGN | ARROW ) wsns ( SWEEP wsns )? expression
            {
            	root_0 = (object)adaptor.GetNilNode();

            	// D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:458:5: ( COLON )?
            	int alt25 = 2;
            	int LA25_0 = input.LA(1);

            	if ( (LA25_0 == COLON) )
            	{
            	    alt25 = 1;
            	}
            	switch (alt25) 
            	{
            	    case 1 :
            	        // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:458:5: COLON
            	        {
            	        	COLON116=(IToken)Match(input,COLON,FOLLOW_COLON_in_namedParameter1444); 
            	        		COLON116_tree = (object)adaptor.Create(COLON116);
            	        		adaptor.AddChild(root_0, COLON116_tree);


            	        }
            	        break;

            	}

            	ID117=(IToken)Match(input,ID,FOLLOW_ID_in_namedParameter1446); 
            		ID117_tree = (object)adaptor.Create(ID117);
            		adaptor.AddChild(root_0, ID117_tree);

            	PushFollow(FOLLOW_wsns_in_namedParameter1448);
            	wsns118 = wsns();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, wsns118.Tree);
            	sign = (IToken)input.LT(1);
            	if ( input.LA(1) == ASSIGN || input.LA(1) == ARROW ) 
            	{
            	    input.Consume();
            	    adaptor.AddChild(root_0, (object)adaptor.Create(sign));
            	    state.errorRecovery = false;
            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    throw mse;
            	}

            	PushFollow(FOLLOW_wsns_in_namedParameter1460);
            	wsns119 = wsns();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, wsns119.Tree);
            	// D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:458:46: ( SWEEP wsns )?
            	int alt26 = 2;
            	int LA26_0 = input.LA(1);

            	if ( (LA26_0 == SWEEP) )
            	{
            	    alt26 = 1;
            	}
            	switch (alt26) 
            	{
            	    case 1 :
            	        // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:458:47: SWEEP wsns
            	        {
            	        	SWEEP120=(IToken)Match(input,SWEEP,FOLLOW_SWEEP_in_namedParameter1463); 
            	        		SWEEP120_tree = (object)adaptor.Create(SWEEP120);
            	        		adaptor.AddChild(root_0, SWEEP120_tree);

            	        	PushFollow(FOLLOW_wsns_in_namedParameter1465);
            	        	wsns121 = wsns();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, wsns121.Tree);

            	        }
            	        break;

            	}

            	PushFollow(FOLLOW_expression_in_namedParameter1469);
            	expression122 = expression();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, expression122.Tree);
            	        
            		      retval.NamedParameter =  new NamedParameter(ID117.Text, ((expression122 != null) ? expression122.Expression : default(Expression)));	      
            		      retval.NamedParameter.IsSweepParam = SWEEP120 != null;
            		      retval.NamedParameter.IsOntoParam = COLON116 != null;
            		      retval.NamedParameter.IsSubscriptionParam = sign.Text != "=" ;	                 
            	    

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);

              FinishObj(retval.NamedParameter, retval);

        }

          catch (RecognitionException re)
          {
            throw re;
          }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "namedParameter"

    public class expression_return : ParserRuleReturnScope
    {
        public Expression Expression;
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "expression"
    // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:467:1: expression returns [Expression Expression] : ( STRING | INT | DOUBLE | BOOL | list | ID_CONST | varIdentifier );
    public EasyFlowParser.expression_return expression() // throws RecognitionException [1]
    {   
        EasyFlowParser.expression_return retval = new EasyFlowParser.expression_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken STRING123 = null;
        IToken INT124 = null;
        IToken DOUBLE125 = null;
        IToken BOOL126 = null;
        IToken ID_CONST128 = null;
        EasyFlowParser.list_return list127 = default(EasyFlowParser.list_return);

        EasyFlowParser.varIdentifier_return varIdentifier129 = default(EasyFlowParser.varIdentifier_return);


        object STRING123_tree=null;
        object INT124_tree=null;
        object DOUBLE125_tree=null;
        object BOOL126_tree=null;
        object ID_CONST128_tree=null;


          StartScope(ScopeType.Expression); 
          retval.Expression =  null;

        try 
    	{
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:479:3: ( STRING | INT | DOUBLE | BOOL | list | ID_CONST | varIdentifier )
            int alt27 = 7;
            switch ( input.LA(1) ) 
            {
            case STRING:
            	{
                alt27 = 1;
                }
                break;
            case INT:
            	{
                alt27 = 2;
                }
                break;
            case DOUBLE:
            	{
                alt27 = 3;
                }
                break;
            case BOOL:
            	{
                alt27 = 4;
                }
                break;
            case LSQ:
            	{
                alt27 = 5;
                }
                break;
            case ID_CONST:
            	{
                alt27 = 6;
                }
                break;
            case ID:
            	{
                alt27 = 7;
                }
                break;
            	default:
            	    NoViableAltException nvae_d27s0 =
            	        new NoViableAltException("", 27, 0, input);

            	    throw nvae_d27s0;
            }

            switch (alt27) 
            {
                case 1 :
                    // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:479:5: STRING
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	STRING123=(IToken)Match(input,STRING,FOLLOW_STRING_in_expression1506); 
                    		STRING123_tree = (object)adaptor.Create(STRING123);
                    		adaptor.AddChild(root_0, STRING123_tree);


                    	      retval.Expression =  new LiteralExpression(StringValue.CreateTrimmed(((STRING123 != null) ? STRING123.Text : null)));           
                    	    

                    }
                    break;
                case 2 :
                    // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:483:5: INT
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	INT124=(IToken)Match(input,INT,FOLLOW_INT_in_expression1519); 
                    		INT124_tree = (object)adaptor.Create(INT124);
                    		adaptor.AddChild(root_0, INT124_tree);


                    	      retval.Expression =  new LiteralExpression(new IntValue(int.Parse(((INT124 != null) ? INT124.Text : null))));
                    	    

                    }
                    break;
                case 3 :
                    // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:487:5: DOUBLE
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	DOUBLE125=(IToken)Match(input,DOUBLE,FOLLOW_DOUBLE_in_expression1531); 
                    		DOUBLE125_tree = (object)adaptor.Create(DOUBLE125);
                    		adaptor.AddChild(root_0, DOUBLE125_tree);


                    	      CultureInfo culture = (CultureInfo) CultureInfo.InvariantCulture.Clone();
                    	      culture.NumberFormat.NumberDecimalSeparator = ".";
                    	      retval.Expression =  new LiteralExpression(new DoubleValue(double.Parse(((DOUBLE125 != null) ? DOUBLE125.Text : null), culture)));
                    	    

                    }
                    break;
                case 4 :
                    // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:493:5: BOOL
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	BOOL126=(IToken)Match(input,BOOL,FOLLOW_BOOL_in_expression1543); 
                    		BOOL126_tree = (object)adaptor.Create(BOOL126);
                    		adaptor.AddChild(root_0, BOOL126_tree);


                    	      retval.Expression =  new LiteralExpression(new BoolValue(bool.Parse(((BOOL126 != null) ? BOOL126.Text : null))));
                    	    

                    }
                    break;
                case 5 :
                    // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:497:5: list
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_list_in_expression1555);
                    	list127 = list();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, list127.Tree);

                    	      retval.Expression =  new LiteralExpression(((list127 != null) ? list127.List : default(ListValue)));
                    	    

                    }
                    break;
                case 6 :
                    // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:505:5: ID_CONST
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	ID_CONST128=(IToken)Match(input,ID_CONST,FOLLOW_ID_CONST_in_expression1581); 
                    		ID_CONST128_tree = (object)adaptor.Create(ID_CONST128);
                    		adaptor.AddChild(root_0, ID_CONST128_tree);


                    	      retval.Expression =  new LiteralExpression(new ConstValue(((ID_CONST128 != null) ? ID_CONST128.Text : null).TrimStart(new[] { '@' })));
                    	    

                    }
                    break;
                case 7 :
                    // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:509:5: varIdentifier
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_varIdentifier_in_expression1593);
                    	varIdentifier129 = varIdentifier();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, varIdentifier129.Tree);

                    	      retval.Expression =  new CompoundIdentitfierExpression(((varIdentifier129 != null) ? varIdentifier129.VarIdentifier : default(CompoundVarIdentifier)));
                    	    

                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);

              FinishObj(retval.Expression, retval);
              
              FinishScope();

        }

          catch (RecognitionException re)
          {
            throw re;
          }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "expression"

    public class list_return : ParserRuleReturnScope
    {
        public ListValue List;
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "list"
    // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:515:1: list returns [ListValue List] : LSQ wsns ( expressionList )? RSQ ;
    public EasyFlowParser.list_return list() // throws RecognitionException [1]
    {   
        EasyFlowParser.list_return retval = new EasyFlowParser.list_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken LSQ130 = null;
        IToken RSQ133 = null;
        EasyFlowParser.wsns_return wsns131 = default(EasyFlowParser.wsns_return);

        EasyFlowParser.expressionList_return expressionList132 = default(EasyFlowParser.expressionList_return);


        object LSQ130_tree=null;
        object RSQ133_tree=null;

          
          ExpressionCollection Expressions = new ExpressionCollection();

        try 
    	{
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:524:3: ( LSQ wsns ( expressionList )? RSQ )
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:524:5: LSQ wsns ( expressionList )? RSQ
            {
            	root_0 = (object)adaptor.GetNilNode();

            	LSQ130=(IToken)Match(input,LSQ,FOLLOW_LSQ_in_list1628); 
            		LSQ130_tree = (object)adaptor.Create(LSQ130);
            		adaptor.AddChild(root_0, LSQ130_tree);

            	PushFollow(FOLLOW_wsns_in_list1630);
            	wsns131 = wsns();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, wsns131.Tree);
            	// D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:524:14: ( expressionList )?
            	int alt28 = 2;
            	int LA28_0 = input.LA(1);

            	if ( (LA28_0 == LSQ || LA28_0 == ID || (LA28_0 >= STRING && LA28_0 <= ID_CONST)) )
            	{
            	    alt28 = 1;
            	}
            	switch (alt28) 
            	{
            	    case 1 :
            	        // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:524:14: expressionList
            	        {
            	        	PushFollow(FOLLOW_expressionList_in_list1632);
            	        	expressionList132 = expressionList();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, expressionList132.Tree);

            	        }
            	        break;

            	}

            	RSQ133=(IToken)Match(input,RSQ,FOLLOW_RSQ_in_list1635); 
            		RSQ133_tree = (object)adaptor.Create(RSQ133);
            		adaptor.AddChild(root_0, RSQ133_tree);

            	    
            	    retval.List =  new ListValue(((expressionList132 != null) ? expressionList132.Expressions : default(ExpressionCollection)));       
            	  

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
              
              FinishObj(retval.List, retval); 

        }

          catch (RecognitionException re)
          {
            throw re;
          }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "list"

    public class expressionList_return : ParserRuleReturnScope
    {
        public ExpressionCollection Expressions;
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "expressionList"
    // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:530:1: expressionList returns [ExpressionCollection Expressions] : first= expression wsns ( COMMA wsns next= expression wsns )* ;
    public EasyFlowParser.expressionList_return expressionList() // throws RecognitionException [1]
    {   
        EasyFlowParser.expressionList_return retval = new EasyFlowParser.expressionList_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken COMMA135 = null;
        EasyFlowParser.expression_return first = default(EasyFlowParser.expression_return);

        EasyFlowParser.expression_return next = default(EasyFlowParser.expression_return);

        EasyFlowParser.wsns_return wsns134 = default(EasyFlowParser.wsns_return);

        EasyFlowParser.wsns_return wsns136 = default(EasyFlowParser.wsns_return);

        EasyFlowParser.wsns_return wsns137 = default(EasyFlowParser.wsns_return);


        object COMMA135_tree=null;

          
          retval.Expressions =  new ExpressionCollection();

        try 
    	{
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:539:3: (first= expression wsns ( COMMA wsns next= expression wsns )* )
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:539:5: first= expression wsns ( COMMA wsns next= expression wsns )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_expression_in_expressionList1670);
            	first = expression();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, first.Tree);
            	PushFollow(FOLLOW_wsns_in_expressionList1672);
            	wsns134 = wsns();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, wsns134.Tree);
            	 retval.Expressions.Add(((first != null) ? first.Expression : default(Expression))); 
            	// D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:540:3: ( COMMA wsns next= expression wsns )*
            	do 
            	{
            	    int alt29 = 2;
            	    int LA29_0 = input.LA(1);

            	    if ( (LA29_0 == COMMA) )
            	    {
            	        alt29 = 1;
            	    }


            	    switch (alt29) 
            		{
            			case 1 :
            			    // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:540:4: COMMA wsns next= expression wsns
            			    {
            			    	COMMA135=(IToken)Match(input,COMMA,FOLLOW_COMMA_in_expressionList1680); 
            			    		COMMA135_tree = (object)adaptor.Create(COMMA135);
            			    		adaptor.AddChild(root_0, COMMA135_tree);

            			    	PushFollow(FOLLOW_wsns_in_expressionList1682);
            			    	wsns136 = wsns();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, wsns136.Tree);
            			    	PushFollow(FOLLOW_expression_in_expressionList1686);
            			    	next = expression();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, next.Tree);
            			    	 retval.Expressions.Add(((next != null) ? next.Expression : default(Expression))); 
            			    	PushFollow(FOLLOW_wsns_in_expressionList1690);
            			    	wsns137 = wsns();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, wsns137.Tree);

            			    }
            			    break;

            			default:
            			    goto loop29;
            	    }
            	} while (true);

            	loop29:
            		;	// Stops C# compiler whining that label 'loop29' has no statements


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);

              FinishObj(retval.Expressions, retval); 

        }

          catch (RecognitionException re)
          {
            throw re;
          }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "expressionList"

    public class id_return : ParserRuleReturnScope
    {
        public Identifier Id;
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "id"
    // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:543:1: id returns [Identifier Id] : ID ;
    public EasyFlowParser.id_return id() // throws RecognitionException [1]
    {   
        EasyFlowParser.id_return retval = new EasyFlowParser.id_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken ID138 = null;

        object ID138_tree=null;

        try 
    	{
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:544:3: ( ID )
            // D:\\work\\code\\nano\\FlowSystem\\EasyFlowParser.Desktop\\Parsing\\ANTLR\\EasyFlow.g:544:5: ID
            {
            	root_0 = (object)adaptor.GetNilNode();

            	ID138=(IToken)Match(input,ID,FOLLOW_ID_in_id1712); 
            		ID138_tree = (object)adaptor.Create(ID138);
            		adaptor.AddChild(root_0, ID138_tree);


            	      bool isInvalid = false;
            	      if (IsKeyword(((ID138 != null) ? ID138.Text : null)))
            	      {
            	        Error(string.Format(GetTextRes(KeywordAsIdentifierError), ((ID138 != null) ? ID138.Text : null)));
            	        isInvalid = true;
            	      }
            	    
            	      retval.Id =  new Identifier(((ID138 != null) ? ID138.Text : null)); // create Identifier object
            	      FinishObj(retval.Id, ID138, isInvalid); // finish it      
            	    

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }

          catch (RecognitionException re)
          {
            throw re;
          }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "id"

    // Delegated rules


   	protected DFA4 dfa4;
   	protected DFA7 dfa7;
   	protected DFA13 dfa13;
   	protected DFA14 dfa14;
   	protected DFA15 dfa15;
   	protected DFA18 dfa18;
   	protected DFA19 dfa19;
   	protected DFA20 dfa20;
   	protected DFA24 dfa24;
   	protected DFA21 dfa21;
	private void InitializeCyclicDFAs()
	{
    	this.dfa4 = new DFA4(this);
    	this.dfa7 = new DFA7(this);
    	this.dfa13 = new DFA13(this);
    	this.dfa14 = new DFA14(this);
    	this.dfa15 = new DFA15(this);
    	this.dfa18 = new DFA18(this);
    	this.dfa19 = new DFA19(this);
    	this.dfa20 = new DFA20(this);
    	this.dfa24 = new DFA24(this);
    	this.dfa21 = new DFA21(this);










	}

    const string DFA4_eotS =
        "\x06\uffff";
    const string DFA4_eofS =
        "\x06\uffff";
    const string DFA4_minS =
        "\x01\x06\x01\x04\x02\uffff\x01\x04\x01\uffff";
    const string DFA4_maxS =
        "\x01\x0f\x01\x0b\x02\uffff\x01\x0b\x01\uffff";
    const string DFA4_acceptS =
        "\x02\uffff\x01\x02\x01\x03\x01\uffff\x01\x01";
    const string DFA4_specialS =
        "\x06\uffff}>";
    static readonly string[] DFA4_transitionS = {
            "\x01\x03\x01\uffff\x01\x01\x05\uffff\x02\x02",
            "\x02\x04\x03\uffff\x01\x05\x01\uffff\x01\x02",
            "",
            "",
            "\x02\x04\x03\uffff\x01\x05\x01\uffff\x01\x02",
            ""
    };

    static readonly short[] DFA4_eot = DFA.UnpackEncodedString(DFA4_eotS);
    static readonly short[] DFA4_eof = DFA.UnpackEncodedString(DFA4_eofS);
    static readonly char[] DFA4_min = DFA.UnpackEncodedStringToUnsignedChars(DFA4_minS);
    static readonly char[] DFA4_max = DFA.UnpackEncodedStringToUnsignedChars(DFA4_maxS);
    static readonly short[] DFA4_accept = DFA.UnpackEncodedString(DFA4_acceptS);
    static readonly short[] DFA4_special = DFA.UnpackEncodedString(DFA4_specialS);
    static readonly short[][] DFA4_transition = DFA.UnpackEncodedStringArray(DFA4_transitionS);

    protected class DFA4 : DFA
    {
        public DFA4(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 4;
            this.eot = DFA4_eot;
            this.eof = DFA4_eof;
            this.min = DFA4_min;
            this.max = DFA4_max;
            this.accept = DFA4_accept;
            this.special = DFA4_special;
            this.transition = DFA4_transition;

        }

        override public string Description
        {
            get { return "87:1: topClause : ( flowAttributeClause | stepClause | requireClause );"; }
        }

    }

    const string DFA7_eotS =
        "\x04\uffff";
    const string DFA7_eofS =
        "\x04\uffff";
    const string DFA7_minS =
        "\x02\x04\x02\uffff";
    const string DFA7_maxS =
        "\x02\x12\x02\uffff";
    const string DFA7_acceptS =
        "\x02\uffff\x01\x02\x01\x01";
    const string DFA7_specialS =
        "\x04\uffff}>";
    static readonly string[] DFA7_transitionS = {
            "\x02\x01\x0c\uffff\x01\x02",
            "\x02\x01\x0b\uffff\x01\x03\x01\x02",
            "",
            ""
    };

    static readonly short[] DFA7_eot = DFA.UnpackEncodedString(DFA7_eotS);
    static readonly short[] DFA7_eof = DFA.UnpackEncodedString(DFA7_eofS);
    static readonly char[] DFA7_min = DFA.UnpackEncodedStringToUnsignedChars(DFA7_minS);
    static readonly char[] DFA7_max = DFA.UnpackEncodedStringToUnsignedChars(DFA7_maxS);
    static readonly short[] DFA7_accept = DFA.UnpackEncodedString(DFA7_acceptS);
    static readonly short[] DFA7_special = DFA.UnpackEncodedString(DFA7_specialS);
    static readonly short[][] DFA7_transition = DFA.UnpackEncodedStringArray(DFA7_transitionS);

    protected class DFA7 : DFA
    {
        public DFA7(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 7;
            this.eot = DFA7_eot;
            this.eof = DFA7_eof;
            this.min = DFA7_min;
            this.max = DFA7_max;
            this.accept = DFA7_accept;
            this.special = DFA7_special;
            this.transition = DFA7_transition;

        }

        override public string Description
        {
            get { return "218:4: ( wsnp AFTER wsnp idList )?"; }
        }

    }

    const string DFA13_eotS =
        "\x04\uffff";
    const string DFA13_eofS =
        "\x02\x03\x02\uffff";
    const string DFA13_minS =
        "\x02\x04\x02\uffff";
    const string DFA13_maxS =
        "\x02\x22\x02\uffff";
    const string DFA13_acceptS =
        "\x02\uffff\x01\x01\x01\x02";
    const string DFA13_specialS =
        "\x04\uffff}>";
    static readonly string[] DFA13_transitionS = {
            "\x02\x01\x03\x03\x05\uffff\x02\x03\x12\uffff\x01\x02",
            "\x02\x01\x03\x03\x05\uffff\x02\x03\x12\uffff\x01\x02",
            "",
            ""
    };

    static readonly short[] DFA13_eot = DFA.UnpackEncodedString(DFA13_eotS);
    static readonly short[] DFA13_eof = DFA.UnpackEncodedString(DFA13_eofS);
    static readonly char[] DFA13_min = DFA.UnpackEncodedStringToUnsignedChars(DFA13_minS);
    static readonly char[] DFA13_max = DFA.UnpackEncodedStringToUnsignedChars(DFA13_maxS);
    static readonly short[] DFA13_accept = DFA.UnpackEncodedString(DFA13_acceptS);
    static readonly short[] DFA13_special = DFA.UnpackEncodedString(DFA13_specialS);
    static readonly short[][] DFA13_transition = DFA.UnpackEncodedStringArray(DFA13_transitionS);

    protected class DFA13 : DFA
    {
        public DFA13(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 13;
            this.eot = DFA13_eot;
            this.eof = DFA13_eof;
            this.min = DFA13_min;
            this.max = DFA13_max;
            this.accept = DFA13_accept;
            this.special = DFA13_special;
            this.transition = DFA13_transition;

        }

        override public string Description
        {
            get { return "263:5: ( wsns 'post' ( WS )+ CODE_BLOCK )?"; }
        }

    }

    const string DFA14_eotS =
        "\x04\uffff";
    const string DFA14_eofS =
        "\x02\x03\x02\uffff";
    const string DFA14_minS =
        "\x02\x04\x02\uffff";
    const string DFA14_maxS =
        "\x02\x0f\x02\uffff";
    const string DFA14_acceptS =
        "\x02\uffff\x01\x01\x01\x02";
    const string DFA14_specialS =
        "\x04\uffff}>";
    static readonly string[] DFA14_transitionS = {
            "\x02\x01\x01\x03\x01\x02\x01\x03\x05\uffff\x02\x03",
            "\x02\x01\x01\x03\x01\x02\x01\x03\x05\uffff\x02\x03",
            "",
            ""
    };

    static readonly short[] DFA14_eot = DFA.UnpackEncodedString(DFA14_eotS);
    static readonly short[] DFA14_eof = DFA.UnpackEncodedString(DFA14_eofS);
    static readonly char[] DFA14_min = DFA.UnpackEncodedStringToUnsignedChars(DFA14_minS);
    static readonly char[] DFA14_max = DFA.UnpackEncodedStringToUnsignedChars(DFA14_maxS);
    static readonly short[] DFA14_accept = DFA.UnpackEncodedString(DFA14_acceptS);
    static readonly short[] DFA14_special = DFA.UnpackEncodedString(DFA14_specialS);
    static readonly short[][] DFA14_transition = DFA.UnpackEncodedStringArray(DFA14_transitionS);

    protected class DFA14 : DFA
    {
        public DFA14(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 14;
            this.eot = DFA14_eot;
            this.eof = DFA14_eof;
            this.min = DFA14_min;
            this.max = DFA14_max;
            this.accept = DFA14_accept;
            this.special = DFA14_special;
            this.transition = DFA14_transition;

        }

        override public string Description
        {
            get { return "269:5: ( wsns SEMICOLON )?"; }
        }

    }

    const string DFA15_eotS =
        "\x04\uffff";
    const string DFA15_eofS =
        "\x04\uffff";
    const string DFA15_minS =
        "\x02\x04\x02\uffff";
    const string DFA15_maxS =
        "\x02\x17\x02\uffff";
    const string DFA15_acceptS =
        "\x02\uffff\x01\x02\x01\x01";
    const string DFA15_specialS =
        "\x04\uffff}>";
    static readonly string[] DFA15_transitionS = {
            "\x02\x01\x01\uffff\x01\x02\x0a\uffff\x01\x02\x04\uffff\x01"+
            "\x03",
            "\x02\x01\x01\uffff\x01\x02\x0a\uffff\x01\x02\x04\uffff\x01"+
            "\x03",
            "",
            ""
    };

    static readonly short[] DFA15_eot = DFA.UnpackEncodedString(DFA15_eotS);
    static readonly short[] DFA15_eof = DFA.UnpackEncodedString(DFA15_eofS);
    static readonly char[] DFA15_min = DFA.UnpackEncodedStringToUnsignedChars(DFA15_minS);
    static readonly char[] DFA15_max = DFA.UnpackEncodedStringToUnsignedChars(DFA15_maxS);
    static readonly short[] DFA15_accept = DFA.UnpackEncodedString(DFA15_acceptS);
    static readonly short[] DFA15_special = DFA.UnpackEncodedString(DFA15_specialS);
    static readonly short[][] DFA15_transition = DFA.UnpackEncodedStringArray(DFA15_transitionS);

    protected class DFA15 : DFA
    {
        public DFA15(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 15;
            this.eot = DFA15_eot;
            this.eof = DFA15_eof;
            this.min = DFA15_min;
            this.max = DFA15_max;
            this.accept = DFA15_accept;
            this.special = DFA15_special;
            this.transition = DFA15_transition;

        }

        override public string Description
        {
            get { return "()* loopback of 300:5: ( wsns COMMA wsns next= ID )*"; }
        }

    }

    const string DFA18_eotS =
        "\x04\uffff";
    const string DFA18_eofS =
        "\x01\x02\x03\uffff";
    const string DFA18_minS =
        "\x02\x04\x02\uffff";
    const string DFA18_maxS =
        "\x02\x18\x02\uffff";
    const string DFA18_acceptS =
        "\x02\uffff\x01\x02\x01\x01";
    const string DFA18_specialS =
        "\x04\uffff}>";
    static readonly string[] DFA18_transitionS = {
            "\x02\x01\x01\uffff\x01\x02\x04\uffff\x02\x02\x06\uffff\x02"+
            "\x02\x01\uffff\x01\x02\x01\x03",
            "\x02\x01\x01\uffff\x01\x02\x04\uffff\x02\x02\x06\uffff\x02"+
            "\x02\x01\uffff\x01\x02\x01\x03",
            "",
            ""
    };

    static readonly short[] DFA18_eot = DFA.UnpackEncodedString(DFA18_eotS);
    static readonly short[] DFA18_eof = DFA.UnpackEncodedString(DFA18_eofS);
    static readonly char[] DFA18_min = DFA.UnpackEncodedStringToUnsignedChars(DFA18_minS);
    static readonly char[] DFA18_max = DFA.UnpackEncodedStringToUnsignedChars(DFA18_maxS);
    static readonly short[] DFA18_accept = DFA.UnpackEncodedString(DFA18_acceptS);
    static readonly short[] DFA18_special = DFA.UnpackEncodedString(DFA18_specialS);
    static readonly short[][] DFA18_transition = DFA.UnpackEncodedStringArray(DFA18_transitionS);

    protected class DFA18 : DFA
    {
        public DFA18(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 18;
            this.eot = DFA18_eot;
            this.eof = DFA18_eof;
            this.min = DFA18_min;
            this.max = DFA18_max;
            this.accept = DFA18_accept;
            this.special = DFA18_special;
            this.transition = DFA18_transition;

        }

        override public string Description
        {
            get { return "()* loopback of 343:5: ( wsns DOT wsns next= simpleVarIdentifier )*"; }
        }

    }

    const string DFA19_eotS =
        "\x04\uffff";
    const string DFA19_eofS =
        "\x01\x03\x03\uffff";
    const string DFA19_minS =
        "\x02\x04\x02\uffff";
    const string DFA19_maxS =
        "\x02\x18\x02\uffff";
    const string DFA19_acceptS =
        "\x02\uffff\x01\x01\x01\x02";
    const string DFA19_specialS =
        "\x04\uffff}>";
    static readonly string[] DFA19_transitionS = {
            "\x02\x01\x01\uffff\x01\x03\x01\x02\x03\uffff\x02\x03\x06\uffff"+
            "\x02\x03\x01\uffff\x02\x03",
            "\x02\x01\x01\uffff\x01\x03\x01\x02\x03\uffff\x02\x03\x06\uffff"+
            "\x02\x03\x01\uffff\x02\x03",
            "",
            ""
    };

    static readonly short[] DFA19_eot = DFA.UnpackEncodedString(DFA19_eotS);
    static readonly short[] DFA19_eof = DFA.UnpackEncodedString(DFA19_eofS);
    static readonly char[] DFA19_min = DFA.UnpackEncodedStringToUnsignedChars(DFA19_minS);
    static readonly char[] DFA19_max = DFA.UnpackEncodedStringToUnsignedChars(DFA19_maxS);
    static readonly short[] DFA19_accept = DFA.UnpackEncodedString(DFA19_acceptS);
    static readonly short[] DFA19_special = DFA.UnpackEncodedString(DFA19_specialS);
    static readonly short[][] DFA19_transition = DFA.UnpackEncodedStringArray(DFA19_transitionS);

    protected class DFA19 : DFA
    {
        public DFA19(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 19;
            this.eot = DFA19_eot;
            this.eof = DFA19_eof;
            this.min = DFA19_min;
            this.max = DFA19_max;
            this.accept = DFA19_accept;
            this.special = DFA19_special;
            this.transition = DFA19_transition;

        }

        override public string Description
        {
            get { return "356:8: ( wsns varIndexer )?"; }
        }

    }

    const string DFA20_eotS =
        "\x04\uffff";
    const string DFA20_eofS =
        "\x01\x02\x03\uffff";
    const string DFA20_minS =
        "\x02\x04\x02\uffff";
    const string DFA20_maxS =
        "\x02\x18\x02\uffff";
    const string DFA20_acceptS =
        "\x02\uffff\x01\x02\x01\x01";
    const string DFA20_specialS =
        "\x04\uffff}>";
    static readonly string[] DFA20_transitionS = {
            "\x02\x01\x0c\uffff\x01\x02\x04\uffff\x01\x02\x01\x03",
            "\x02\x01\x0b\uffff\x02\x02\x04\uffff\x01\x02\x01\x03",
            "",
            ""
    };

    static readonly short[] DFA20_eot = DFA.UnpackEncodedString(DFA20_eotS);
    static readonly short[] DFA20_eof = DFA.UnpackEncodedString(DFA20_eofS);
    static readonly char[] DFA20_min = DFA.UnpackEncodedStringToUnsignedChars(DFA20_minS);
    static readonly char[] DFA20_max = DFA.UnpackEncodedStringToUnsignedChars(DFA20_maxS);
    static readonly short[] DFA20_accept = DFA.UnpackEncodedString(DFA20_acceptS);
    static readonly short[] DFA20_special = DFA.UnpackEncodedString(DFA20_specialS);
    static readonly short[][] DFA20_transition = DFA.UnpackEncodedStringArray(DFA20_transitionS);

    protected class DFA20 : DFA
    {
        public DFA20(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 20;
            this.eot = DFA20_eot;
            this.eof = DFA20_eof;
            this.min = DFA20_min;
            this.max = DFA20_max;
            this.accept = DFA20_accept;
            this.special = DFA20_special;
            this.transition = DFA20_transition;

        }

        override public string Description
        {
            get { return "()* loopback of 410:5: ( wsns DOT wsns next= compoundNameComponent )*"; }
        }

    }

    const string DFA24_eotS =
        "\x05\uffff";
    const string DFA24_eofS =
        "\x05\uffff";
    const string DFA24_minS =
        "\x01\x14\x01\x04\x01\uffff\x01\x04\x01\uffff";
    const string DFA24_maxS =
        "\x01\x17\x01\x15\x01\uffff\x01\x15\x01\uffff";
    const string DFA24_acceptS =
        "\x02\uffff\x01\x02\x01\uffff\x01\x01";
    const string DFA24_specialS =
        "\x05\uffff}>";
    static readonly string[] DFA24_transitionS = {
            "\x02\x02\x01\uffff\x01\x01",
            "\x02\x03\x04\uffff\x02\x04\x08\uffff\x02\x02",
            "",
            "\x02\x03\x04\uffff\x02\x04\x08\uffff\x02\x02",
            ""
    };

    static readonly short[] DFA24_eot = DFA.UnpackEncodedString(DFA24_eotS);
    static readonly short[] DFA24_eof = DFA.UnpackEncodedString(DFA24_eofS);
    static readonly char[] DFA24_min = DFA.UnpackEncodedStringToUnsignedChars(DFA24_minS);
    static readonly char[] DFA24_max = DFA.UnpackEncodedStringToUnsignedChars(DFA24_maxS);
    static readonly short[] DFA24_accept = DFA.UnpackEncodedString(DFA24_acceptS);
    static readonly short[] DFA24_special = DFA.UnpackEncodedString(DFA24_specialS);
    static readonly short[][] DFA24_transition = DFA.UnpackEncodedStringArray(DFA24_transitionS);

    protected class DFA24 : DFA
    {
        public DFA24(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 24;
            this.eot = DFA24_eot;
            this.eof = DFA24_eof;
            this.min = DFA24_min;
            this.max = DFA24_max;
            this.accept = DFA24_accept;
            this.special = DFA24_special;
            this.transition = DFA24_transition;

        }

        override public string Description
        {
            get { return "441:5: ( ( ( COMMA wsns next= namedParameter wsns )+ ( COMMA wsns )? ) | ( COMMA wsns )? )"; }
        }

    }

    const string DFA21_eotS =
        "\x05\uffff";
    const string DFA21_eofS =
        "\x05\uffff";
    const string DFA21_minS =
        "\x01\x14\x01\x04\x01\uffff\x01\x04\x01\uffff";
    const string DFA21_maxS =
        "\x01\x17\x01\x15\x01\uffff\x01\x15\x01\uffff";
    const string DFA21_acceptS =
        "\x02\uffff\x01\x02\x01\uffff\x01\x01";
    const string DFA21_specialS =
        "\x05\uffff}>";
    static readonly string[] DFA21_transitionS = {
            "\x02\x02\x01\uffff\x01\x01",
            "\x02\x03\x04\uffff\x02\x04\x08\uffff\x02\x02",
            "",
            "\x02\x03\x04\uffff\x02\x04\x08\uffff\x02\x02",
            ""
    };

    static readonly short[] DFA21_eot = DFA.UnpackEncodedString(DFA21_eotS);
    static readonly short[] DFA21_eof = DFA.UnpackEncodedString(DFA21_eofS);
    static readonly char[] DFA21_min = DFA.UnpackEncodedStringToUnsignedChars(DFA21_minS);
    static readonly char[] DFA21_max = DFA.UnpackEncodedStringToUnsignedChars(DFA21_maxS);
    static readonly short[] DFA21_accept = DFA.UnpackEncodedString(DFA21_acceptS);
    static readonly short[] DFA21_special = DFA.UnpackEncodedString(DFA21_specialS);
    static readonly short[][] DFA21_transition = DFA.UnpackEncodedStringArray(DFA21_transitionS);

    protected class DFA21 : DFA
    {
        public DFA21(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 21;
            this.eot = DFA21_eot;
            this.eof = DFA21_eof;
            this.min = DFA21_min;
            this.max = DFA21_max;
            this.accept = DFA21_accept;
            this.special = DFA21_special;
            this.transition = DFA21_transition;

        }

        override public string Description
        {
            get { return "()+ loopback of 442:7: ( COMMA wsns next= namedParameter wsns )+"; }
        }

    }

 

    public static readonly BitSet FOLLOW_wsns_in_program109 = new BitSet(new ulong[]{0x000000000000C142UL});
    public static readonly BitSet FOLLOW_topClause_in_program113 = new BitSet(new ulong[]{0x000000000000C170UL});
    public static readonly BitSet FOLLOW_wsns_in_program115 = new BitSet(new ulong[]{0x000000000000C142UL});
    public static readonly BitSet FOLLOW_set_in_wsn0 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_wsn_in_wsnp160 = new BitSet(new ulong[]{0x0000000000000032UL});
    public static readonly BitSet FOLLOW_wsn_in_wsns177 = new BitSet(new ulong[]{0x0000000000000032UL});
    public static readonly BitSet FOLLOW_REQUIRE_in_requireClause207 = new BitSet(new ulong[]{0x0000000000000030UL});
    public static readonly BitSet FOLLOW_wsnp_in_requireClause209 = new BitSet(new ulong[]{0x0000000000000800UL});
    public static readonly BitSet FOLLOW_idList_in_requireClause211 = new BitSet(new ulong[]{0x00000000000000B0UL});
    public static readonly BitSet FOLLOW_wsns_in_requireClause213 = new BitSet(new ulong[]{0x0000000000000080UL});
    public static readonly BitSet FOLLOW_SEMICOLON_in_requireClause215 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_flowAttributeClause_in_topClause246 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_stepClause_in_topClause252 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_requireClause_in_topClause258 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_LSQ_in_flowAttributeClause295 = new BitSet(new ulong[]{0x0000000000000230UL});
    public static readonly BitSet FOLLOW_wsns_in_flowAttributeClause297 = new BitSet(new ulong[]{0x0000000000000200UL});
    public static readonly BitSet FOLLOW_FLOW_in_flowAttributeClause299 = new BitSet(new ulong[]{0x0000000000000430UL});
    public static readonly BitSet FOLLOW_wsns_in_flowAttributeClause301 = new BitSet(new ulong[]{0x0000000000000400UL});
    public static readonly BitSet FOLLOW_COLON_in_flowAttributeClause303 = new BitSet(new ulong[]{0x0000000000000830UL});
    public static readonly BitSet FOLLOW_wsns_in_flowAttributeClause305 = new BitSet(new ulong[]{0x0000000000000800UL});
    public static readonly BitSet FOLLOW_ID_in_flowAttributeClause307 = new BitSet(new ulong[]{0x0000000000001030UL});
    public static readonly BitSet FOLLOW_wsns_in_flowAttributeClause309 = new BitSet(new ulong[]{0x0000000000001000UL});
    public static readonly BitSet FOLLOW_ASSIGN_in_flowAttributeClause311 = new BitSet(new ulong[]{0x00000000F8000930UL});
    public static readonly BitSet FOLLOW_wsns_in_flowAttributeClause313 = new BitSet(new ulong[]{0x00000000F8000930UL});
    public static readonly BitSet FOLLOW_expression_in_flowAttributeClause315 = new BitSet(new ulong[]{0x0000000000002000UL});
    public static readonly BitSet FOLLOW_RSQ_in_flowAttributeClause317 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_LSQ_in_attribute352 = new BitSet(new ulong[]{0x0000000000000830UL});
    public static readonly BitSet FOLLOW_wsns_in_attribute354 = new BitSet(new ulong[]{0x0000000000000800UL});
    public static readonly BitSet FOLLOW_ID_in_attribute356 = new BitSet(new ulong[]{0x0000000000001030UL});
    public static readonly BitSet FOLLOW_wsns_in_attribute358 = new BitSet(new ulong[]{0x0000000000001000UL});
    public static readonly BitSet FOLLOW_ASSIGN_in_attribute360 = new BitSet(new ulong[]{0x00000000F8000930UL});
    public static readonly BitSet FOLLOW_wsns_in_attribute362 = new BitSet(new ulong[]{0x00000000F8000930UL});
    public static readonly BitSet FOLLOW_expression_in_attribute364 = new BitSet(new ulong[]{0x0000000000002030UL});
    public static readonly BitSet FOLLOW_wsns_in_attribute366 = new BitSet(new ulong[]{0x0000000000002000UL});
    public static readonly BitSet FOLLOW_RSQ_in_attribute368 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_attribute_in_attributes406 = new BitSet(new ulong[]{0x0000000000000130UL});
    public static readonly BitSet FOLLOW_wsns_in_attributes420 = new BitSet(new ulong[]{0x0000000000000102UL});
    public static readonly BitSet FOLLOW_attributes_in_stepClause464 = new BitSet(new ulong[]{0x000000000000C000UL});
    public static readonly BitSet FOLLOW_TILDA_in_stepClause478 = new BitSet(new ulong[]{0x0000000000008000UL});
    public static readonly BitSet FOLLOW_STEP_in_stepClause494 = new BitSet(new ulong[]{0x0000000000000030UL});
    public static readonly BitSet FOLLOW_wsnp_in_stepClause498 = new BitSet(new ulong[]{0x0000000000000800UL});
    public static readonly BitSet FOLLOW_id_in_stepClause500 = new BitSet(new ulong[]{0x0000000000000030UL});
    public static readonly BitSet FOLLOW_wsnp_in_stepClause514 = new BitSet(new ulong[]{0x0000000000010000UL});
    public static readonly BitSet FOLLOW_RUNS_in_stepClause516 = new BitSet(new ulong[]{0x0000000000000030UL});
    public static readonly BitSet FOLLOW_wsnp_in_stepClause520 = new BitSet(new ulong[]{0x0000000000000800UL});
    public static readonly BitSet FOLLOW_compoundName_in_stepClause524 = new BitSet(new ulong[]{0x0000000000040030UL});
    public static readonly BitSet FOLLOW_wsnp_in_stepClause540 = new BitSet(new ulong[]{0x0000000000020000UL});
    public static readonly BitSet FOLLOW_AFTER_in_stepClause542 = new BitSet(new ulong[]{0x0000000000000030UL});
    public static readonly BitSet FOLLOW_wsnp_in_stepClause546 = new BitSet(new ulong[]{0x0000000000000800UL});
    public static readonly BitSet FOLLOW_idList_in_stepClause548 = new BitSet(new ulong[]{0x0000000000040030UL});
    public static readonly BitSet FOLLOW_wsns_in_stepClause575 = new BitSet(new ulong[]{0x0000000000040000UL});
    public static readonly BitSet FOLLOW_LPAREN_in_stepClause577 = new BitSet(new ulong[]{0x0000000000380C30UL});
    public static readonly BitSet FOLLOW_wsns_in_stepClause579 = new BitSet(new ulong[]{0x0000000000380C00UL});
    public static readonly BitSet FOLLOW_APP_in_stepClause590 = new BitSet(new ulong[]{0x0000000000000430UL});
    public static readonly BitSet FOLLOW_wsns_in_stepClause592 = new BitSet(new ulong[]{0x0000000000000400UL});
    public static readonly BitSet FOLLOW_COLON_in_stepClause594 = new BitSet(new ulong[]{0x0000000000300C30UL});
    public static readonly BitSet FOLLOW_wsns_in_stepClause596 = new BitSet(new ulong[]{0x0000000000300C00UL});
    public static readonly BitSet FOLLOW_namedParametersList_in_stepClause602 = new BitSet(new ulong[]{0x0000000000300000UL});
    public static readonly BitSet FOLLOW_EXEC_in_stepClause625 = new BitSet(new ulong[]{0x0000000000000430UL});
    public static readonly BitSet FOLLOW_wsns_in_stepClause627 = new BitSet(new ulong[]{0x0000000000000400UL});
    public static readonly BitSet FOLLOW_COLON_in_stepClause629 = new BitSet(new ulong[]{0x0000000000200C30UL});
    public static readonly BitSet FOLLOW_wsns_in_stepClause631 = new BitSet(new ulong[]{0x0000000000200C00UL});
    public static readonly BitSet FOLLOW_namedParametersList_in_stepClause636 = new BitSet(new ulong[]{0x0000000000200000UL});
    public static readonly BitSet FOLLOW_RPAREN_in_stepClause662 = new BitSet(new ulong[]{0x00000004000000B2UL});
    public static readonly BitSet FOLLOW_wsns_in_stepClause681 = new BitSet(new ulong[]{0x0000000400000000UL});
    public static readonly BitSet FOLLOW_POST_in_stepClause683 = new BitSet(new ulong[]{0x0000000000000010UL});
    public static readonly BitSet FOLLOW_WS_in_stepClause685 = new BitSet(new ulong[]{0x0000000000400010UL});
    public static readonly BitSet FOLLOW_CODE_BLOCK_in_stepClause688 = new BitSet(new ulong[]{0x00000000000000B2UL});
    public static readonly BitSet FOLLOW_wsns_in_stepClause714 = new BitSet(new ulong[]{0x0000000000000080UL});
    public static readonly BitSet FOLLOW_SEMICOLON_in_stepClause716 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_ID_in_idList797 = new BitSet(new ulong[]{0x0000000000800032UL});
    public static readonly BitSet FOLLOW_wsns_in_idList810 = new BitSet(new ulong[]{0x0000000000800000UL});
    public static readonly BitSet FOLLOW_COMMA_in_idList812 = new BitSet(new ulong[]{0x0000000000000830UL});
    public static readonly BitSet FOLLOW_wsns_in_idList814 = new BitSet(new ulong[]{0x0000000000000800UL});
    public static readonly BitSet FOLLOW_ID_in_idList818 = new BitSet(new ulong[]{0x0000000000800032UL});
    public static readonly BitSet FOLLOW_statement_in_codeBlock858 = new BitSet(new ulong[]{0x00000000000000B0UL});
    public static readonly BitSet FOLLOW_wsns_in_codeBlock860 = new BitSet(new ulong[]{0x0000000000000080UL});
    public static readonly BitSet FOLLOW_SEMICOLON_in_codeBlock862 = new BitSet(new ulong[]{0x00000000F8000930UL});
    public static readonly BitSet FOLLOW_wsns_in_codeBlock864 = new BitSet(new ulong[]{0x00000000F8000932UL});
    public static readonly BitSet FOLLOW_assignment_in_statement881 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_varIdentifier_in_assignment896 = new BitSet(new ulong[]{0x0000000000001030UL});
    public static readonly BitSet FOLLOW_wsns_in_assignment898 = new BitSet(new ulong[]{0x0000000000001000UL});
    public static readonly BitSet FOLLOW_ASSIGN_in_assignment900 = new BitSet(new ulong[]{0x00000000F8000930UL});
    public static readonly BitSet FOLLOW_wsns_in_assignment902 = new BitSet(new ulong[]{0x00000000F8000930UL});
    public static readonly BitSet FOLLOW_expression_in_assignment904 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_varIdentifier_in_varIdentifierList923 = new BitSet(new ulong[]{0x0000000000800032UL});
    public static readonly BitSet FOLLOW_wsns_in_varIdentifierList926 = new BitSet(new ulong[]{0x0000000000800000UL});
    public static readonly BitSet FOLLOW_COMMA_in_varIdentifierList928 = new BitSet(new ulong[]{0x00000000F8000930UL});
    public static readonly BitSet FOLLOW_wsns_in_varIdentifierList930 = new BitSet(new ulong[]{0x00000000F8000930UL});
    public static readonly BitSet FOLLOW_varIdentifier_in_varIdentifierList932 = new BitSet(new ulong[]{0x0000000000800032UL});
    public static readonly BitSet FOLLOW_simpleVarIdentifier_in_varIdentifier971 = new BitSet(new ulong[]{0x0000000001000032UL});
    public static readonly BitSet FOLLOW_wsns_in_varIdentifier985 = new BitSet(new ulong[]{0x0000000001000000UL});
    public static readonly BitSet FOLLOW_DOT_in_varIdentifier987 = new BitSet(new ulong[]{0x00000000F8000930UL});
    public static readonly BitSet FOLLOW_wsns_in_varIdentifier989 = new BitSet(new ulong[]{0x00000000F8000930UL});
    public static readonly BitSet FOLLOW_simpleVarIdentifier_in_varIdentifier993 = new BitSet(new ulong[]{0x0000000001000032UL});
    public static readonly BitSet FOLLOW_ID_in_simpleVarIdentifier1037 = new BitSet(new ulong[]{0x0000000000000132UL});
    public static readonly BitSet FOLLOW_wsns_in_simpleVarIdentifier1040 = new BitSet(new ulong[]{0x0000000000000130UL});
    public static readonly BitSet FOLLOW_varIndexer_in_simpleVarIdentifier1042 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_ID_in_varName1071 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_LSQ_in_varIndexer1095 = new BitSet(new ulong[]{0x00000000F8000930UL});
    public static readonly BitSet FOLLOW_wsns_in_varIndexer1097 = new BitSet(new ulong[]{0x00000000F8000930UL});
    public static readonly BitSet FOLLOW_expression_in_varIndexer1099 = new BitSet(new ulong[]{0x0000000000002030UL});
    public static readonly BitSet FOLLOW_wsns_in_varIndexer1101 = new BitSet(new ulong[]{0x0000000000002000UL});
    public static readonly BitSet FOLLOW_RSQ_in_varIndexer1103 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_ID_in_filePointer1140 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_compoundName_in_compoundNameList1171 = new BitSet(new ulong[]{0x0000000000800030UL});
    public static readonly BitSet FOLLOW_wsns_in_compoundNameList1174 = new BitSet(new ulong[]{0x0000000000800000UL});
    public static readonly BitSet FOLLOW_COMMA_in_compoundNameList1176 = new BitSet(new ulong[]{0x0000000000000830UL});
    public static readonly BitSet FOLLOW_wsns_in_compoundNameList1178 = new BitSet(new ulong[]{0x0000000000000800UL});
    public static readonly BitSet FOLLOW_compoundName_in_compoundNameList1180 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_compoundNameComponent_in_compoundName1215 = new BitSet(new ulong[]{0x0000000001000032UL});
    public static readonly BitSet FOLLOW_wsns_in_compoundName1229 = new BitSet(new ulong[]{0x0000000001000000UL});
    public static readonly BitSet FOLLOW_DOT_in_compoundName1231 = new BitSet(new ulong[]{0x0000000000000830UL});
    public static readonly BitSet FOLLOW_wsns_in_compoundName1233 = new BitSet(new ulong[]{0x0000000000000800UL});
    public static readonly BitSet FOLLOW_compoundNameComponent_in_compoundName1237 = new BitSet(new ulong[]{0x0000000001000032UL});
    public static readonly BitSet FOLLOW_ID_in_compoundNameComponent1276 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_namedParameter_in_namedParametersList1314 = new BitSet(new ulong[]{0x0000000000800030UL});
    public static readonly BitSet FOLLOW_wsns_in_namedParametersList1316 = new BitSet(new ulong[]{0x0000000000800002UL});
    public static readonly BitSet FOLLOW_COMMA_in_namedParametersList1349 = new BitSet(new ulong[]{0x0000000000000C30UL});
    public static readonly BitSet FOLLOW_wsns_in_namedParametersList1351 = new BitSet(new ulong[]{0x0000000000000C00UL});
    public static readonly BitSet FOLLOW_namedParameter_in_namedParametersList1355 = new BitSet(new ulong[]{0x0000000000800030UL});
    public static readonly BitSet FOLLOW_wsns_in_namedParametersList1357 = new BitSet(new ulong[]{0x0000000000800002UL});
    public static readonly BitSet FOLLOW_COMMA_in_namedParametersList1394 = new BitSet(new ulong[]{0x0000000000000030UL});
    public static readonly BitSet FOLLOW_wsns_in_namedParametersList1396 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_COMMA_in_namedParametersList1410 = new BitSet(new ulong[]{0x0000000000000030UL});
    public static readonly BitSet FOLLOW_wsns_in_namedParametersList1412 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_COLON_in_namedParameter1444 = new BitSet(new ulong[]{0x0000000000000800UL});
    public static readonly BitSet FOLLOW_ID_in_namedParameter1446 = new BitSet(new ulong[]{0x0000000002001030UL});
    public static readonly BitSet FOLLOW_wsns_in_namedParameter1448 = new BitSet(new ulong[]{0x0000000002001000UL});
    public static readonly BitSet FOLLOW_set_in_namedParameter1452 = new BitSet(new ulong[]{0x00000000FC000930UL});
    public static readonly BitSet FOLLOW_wsns_in_namedParameter1460 = new BitSet(new ulong[]{0x00000000FC000930UL});
    public static readonly BitSet FOLLOW_SWEEP_in_namedParameter1463 = new BitSet(new ulong[]{0x00000000F8000930UL});
    public static readonly BitSet FOLLOW_wsns_in_namedParameter1465 = new BitSet(new ulong[]{0x00000000F8000930UL});
    public static readonly BitSet FOLLOW_expression_in_namedParameter1469 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_STRING_in_expression1506 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_INT_in_expression1519 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_DOUBLE_in_expression1531 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_BOOL_in_expression1543 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_list_in_expression1555 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_ID_CONST_in_expression1581 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_varIdentifier_in_expression1593 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_LSQ_in_list1628 = new BitSet(new ulong[]{0x00000000F8002930UL});
    public static readonly BitSet FOLLOW_wsns_in_list1630 = new BitSet(new ulong[]{0x00000000F8002930UL});
    public static readonly BitSet FOLLOW_expressionList_in_list1632 = new BitSet(new ulong[]{0x0000000000002000UL});
    public static readonly BitSet FOLLOW_RSQ_in_list1635 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_expression_in_expressionList1670 = new BitSet(new ulong[]{0x0000000000800030UL});
    public static readonly BitSet FOLLOW_wsns_in_expressionList1672 = new BitSet(new ulong[]{0x0000000000800002UL});
    public static readonly BitSet FOLLOW_COMMA_in_expressionList1680 = new BitSet(new ulong[]{0x00000000F8000930UL});
    public static readonly BitSet FOLLOW_wsns_in_expressionList1682 = new BitSet(new ulong[]{0x00000000F8000930UL});
    public static readonly BitSet FOLLOW_expression_in_expressionList1686 = new BitSet(new ulong[]{0x0000000000800030UL});
    public static readonly BitSet FOLLOW_wsns_in_expressionList1690 = new BitSet(new ulong[]{0x0000000000800002UL});
    public static readonly BitSet FOLLOW_ID_in_id1712 = new BitSet(new ulong[]{0x0000000000000002UL});

}
}