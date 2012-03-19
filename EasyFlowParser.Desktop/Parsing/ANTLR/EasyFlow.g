grammar EasyFlow;
   
options { // define options for ANTLR
  language = CSharp2; 
  output = AST;    
}
  
@parser::namespace { Easis.Wfs.EasyFlow.Parsing }    // define parser namespace
@lexer::namespace { Easis.Wfs.EasyFlow.Parsing }     // define lexer namespace
    
@parser::header // make usages of the needed namespaces
{
  using System.Collections.Generic; 
  using System.Linq;
  using Easis.Wfs.EasyFlow.Model;
  using Easis.Wfs.EasyFlow.Model.Collections;
  using System.Globalization;
}

// catch exceptions manually and just rethrow it 
@rulecatch  
{
  catch (RecognitionException re)
  {
    throw re;
  }
}

program returns [ParseResult ParseResult]
@init
{  
  Init(); // Initialize parser
  
  StartRootContext(); // Start root context
  
  StartRootScope(); // Start root scope (scipt)
}
@after
{
  FinishScope(); // Finish root scope
  
  FinishContext(); // Finish root context
  
  Finali(); // Finalize parser
  $ParseResult = _parseResult; // Return parse result  
}
  : wsns ( topClause wsns )*    
  ;
  
// White space rule
wsn
  : WS
  | NEWLINE
  ;  

// wsn+
wsnp
  : wsn+
  ;
  
// wsn*
wsns
  : wsn*
  ;    
  
requireClause
@init
{
  StartScope(ScopeType.Require);
}
@after
{  
  FinishScope();   
}
  : REQUIRE wsnp idList wsns SEMICOLON
    {
      foreach (var id in $idList.Identifiers)      
      {
        FileRequirement requirement = new FileRequirement(id.Name);
        FinishObj(requirement, id);
      
        AddRequirement(new FileRequirement(id.Name));
      }      
    }    
  ;  
  
topClause
  :
    flowAttributeClause
  | stepClause
  | requireClause
  
  //| ifClause
  //  {
  //  }
  ;
  
flowAttributeClause
@init
{ 
  NamedParameter namedParam = null;
}
@after
{
  if (namedParam != null)
    FinishObj(namedParam, retval);
}
  : LSQ wsns FLOW wsns COLON wsns ID wsns ASSIGN wsns expression RSQ
    {
      namedParam = new NamedParameter($ID.text, $expression.Expression);
      AddFlowAttribute(namedParam); 
    }
  ;
  
attribute returns [NamedParameter NamedParameter]
@init
{
}
@after
{
  FinishObj($NamedParameter, retval);
}
  : LSQ wsns ID wsns ASSIGN wsns expression wsns RSQ
      {
        $NamedParameter = new NamedParameter($ID.Text, $expression.Expression);       
        $NamedParameter.IsSweepParam = false;
        $NamedParameter.IsOntoParam = false;                  
    }
  ;
  
attributes returns [NamedParameterCollection NamedParameters]
@init
{
}
@after
{
  if ($NamedParameters != null)  
    FinishObj($NamedParameters, retval);
}
  : (attribute
	    {
	       if ($NamedParameters == null)
	         $NamedParameters = new NamedParameterCollection();
	         
	       $NamedParameters.Add($attribute.NamedParameter);
	    }
     wsns
    )*
  
  ;
  
stepClause returns [StepBlock StepBlock]
@init
{
  StartScope(ScopeType.Step); // Start Step scope
  
  $StepBlock = new StepBlock(); // Create resulting Step block
}
@after
{
  bool isDisabled = false;
  var disabledParam = $StepBlock.ExecParameters.Parameters.FirstOrDefault(parameter => parameter.Name == "disabled");
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
  
  $StepBlock.IsDisabled = isDisabled;

  FinishObj($StepBlock, retval); // Finish resulting Step block 
  AddBlock($StepBlock);
  FinishScope(); // Finish ScopeType.Step scope
}
  : 
    attributes
    {
      if ($attributes.NamedParameters != null)
      {
        $StepBlock.ExecParameters.AddRange($attributes.NamedParameters);
        FinishObj($StepBlock.ExecParameters, $attributes.NamedParameters);
      }
    }
		//(DISABLED wsnp)? 
		STEP { StartScope(ScopeType.StepHeader); } wsnp id
		{
		  Identifier stepId = $id.Id;
		  stepId.IdentifierType = IdentifierType.StepName;                
		  
		  CheckAddIdentifierDef(stepId); // check and add first occurance of step name     
		  
		  // TODO: error check
		  $StepBlock.Name = stepId.Name; // Assign step name     		 		  
		}     

		wsnp RUNS { StartScope(ScopeType.RunObjectName); } wsnp packageName=compoundName
		{
		  $StepBlock.RunObjectName = new RunObjectName($packageName.Locator);
		  
		  FinishScope(); // Pop ScopeType.RunObjectName
		  FinishScope(); // Pop ScopeType.StepHeader because here header ends
		}
				 
	  (wsnp AFTER { StartScope(ScopeType.StepAfterList); } wsnp idList // if there is AFTER keyword
			{
			  foreach (var refStepId in $idList.Identifiers)
			  {
			    TriggerForwardDefinition tfd = new TriggerForwardDefinition 
			    {
			      TriggerOwner = $StepBlock,
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
    )?  // Run conditions end	            
		
		wsns LPAREN wsns
		{ 
		  StartScope(ScopeType.RunParameters);
		  bool wasApp = false;
		  bool wasExec = false;
	  }
		  (APP wsns COLON wsns)? parameters=namedParametersList?
		  {
		     wasApp = true;
         if ($parameters.NamedParameters != null)
           $StepBlock.RunParameters.Parameters = $parameters.NamedParameters;         
      }			
			(
			  (EXEC wsns COLON wsns) execParams=namedParametersList?
			  {
			     wasExec = true;
			     if ($execParams.NamedParameters != null)
			       $StepBlock.ExecParameters.Parameters.AddRange($execParams.NamedParameters);
			  }
			)?
		   
    RPAREN 
    {
      FinishScope(); // Pop ScopeType.RunParameters
    }
    wsns SEMICOLON
 ;			  
						
	//					|
	//					(ON { StartScope(ScopeType.StepEventList); } wsnp eventList=compoundNameList) // if there is ON keyword
	//					{
	//					  // TODO: check event list (Step.Event only)
	//					  
	//					  FinishScope(); // Pop ScopeType.StepEventList
	//					}
					
			 

//runClause [StepBlock StepBlock]
  // run Some.Package.Name (Param1 = 1, Param2 = "string");
//  : RUN wsnp compoundName wsns LPAREN wsns namedParametersList? RPAREN wsns SEMICOLON
//  ;  

idList returns [IEnumerable<Identifier> Identifiers]
@init
{
  var list = new List<Identifier>();  
}
@after
{
  $Identifiers = list;
}
  : first=ID
    {
      list.Add(new Identifier($first.text));      
    }
    (wsns COMMA wsns next=ID
      {
        list.Add(new Identifier($next.text));       
      }
    )*
    
  ;
  

  
codeBlock
  : (statement wsns SEMICOLON wsns)+
  ;
  
statement
  : assignment
  ;
  
assignment
  : varIdentifier wsns ASSIGN wsns expression
  ;  
  


varIdentifierList
  : varIdentifier (wsns COMMA wsns varIdentifier)*    
  ;   

varIdentifier returns [CompoundVarIdentifier VarIdentifier]
@init 
{
  StartScope(ScopeType.VarIdentifier);
  $VarIdentifier = new CompoundVarIdentifier();
}
@after
{
  FinishObj($VarIdentifier, retval);
  FinishScope();
}
  : first=simpleVarIdentifier
    {
      $VarIdentifier.AddPart($first.SimpleVarIdentifier);
    }
    ( wsns DOT wsns next=simpleVarIdentifier
      {
        $VarIdentifier.AddPart($next.SimpleVarIdentifier);
      }
    )*
    
  ;
  
simpleVarIdentifier returns [SimpleVarIdentifier SimpleVarIdentifier]
@after
{
  FinishObj($SimpleVarIdentifier, retval);  
}
  : ID (wsns varIndexer)?
    {
      $SimpleVarIdentifier = new SimpleVarIdentifier($ID.text, $varIndexer.Indexer); 
    }       
  ;
 
varName
  : ID
  ; 
 
varIndexer returns [Indexer Indexer]
@after
{
  FinishObj($Indexer, retval);
}
  : LSQ wsns expression wsns RSQ
    {
      $Indexer = new Indexer($expression.Expression);
    }
  ;
    
filePointer returns [FileDescriptor FilePointer]
@init
{
  StartScope(ScopeType.FilePointer);
}
@after
{
  FinishObj($FilePointer, retval);
  FinishScope();
}
  : ID
    {
      $FilePointer = new FileDescriptor(null, $ID.text);
    }
  ;      
  
compoundNameList returns [IEnumerable<StringCompoundName> LocatorList]
  : compoundName (wsns COMMA wsns compoundName)
  ;  
  
compoundName  returns [StringCompoundName Locator]
@init
{
  $Locator = new StringCompoundName();  
}
@after
{
  FinishObj($Locator, retval);
}
  : first=compoundNameComponent
    { 
      $Locator.AddPart($first.Identifier.Name);
    }
    ( wsns DOT wsns next=compoundNameComponent
      {
        $Locator.AddPart($next.Identifier.Name);
      }
    )*
  ;
  
compoundNameComponent returns [Identifier Identifier]
@after
{
  FinishObj($Identifier, retval);
}
  : ID 
    {
      $Identifier = new Identifier($ID.text);
    }
  ;
  
namedParametersList returns [NamedParameterCollection NamedParameters]
@init
{  
  $NamedParameters = new NamedParameterCollection();
}
@after
{
  FinishObj($NamedParameters, retval); 
}
  : first=namedParameter wsns  
    { 
      $NamedParameters.Add($first.NamedParameter);
    }
    ((
      (
        COMMA wsns next=namedParameter wsns
        {
          $NamedParameters.Add($next.NamedParameter);
        }         
      )+
      (COMMA wsns)? // comma at the end
    ) | (COMMA wsns)?
    ) // comma at the end
  ;
 
namedParameter returns [NamedParameter NamedParameter]
@after
{
  FinishObj($NamedParameter, retval);
}
  : COLON?ID wsns ASSIGN wsns (SWEEP wsns)? expression
      {
	      $NamedParameter = new NamedParameter($ID.Text, $expression.Expression);	      
	      $NamedParameter.IsSweepParam = $SWEEP != null;
	      $NamedParameter.IsOntoParam = $COLON != null;	                 
    }
  ; 
 
expression returns [Expression Expression]
@init
{
  StartScope(ScopeType.Expression); 
  $Expression = null;
}
@after
{
  FinishObj($Expression, retval);
  
  FinishScope();
}
  : STRING
    {
      $Expression = new LiteralExpression(StringValue.CreateTrimmed($STRING.text));           
    } 
  | INT
    {
      $Expression = new LiteralExpression(new IntValue(int.Parse($INT.text)));
    }
  | DOUBLE
    {
      CultureInfo culture = (CultureInfo) CultureInfo.InvariantCulture.Clone();
      culture.NumberFormat.NumberDecimalSeparator = ".";
      $Expression = new LiteralExpression(new DoubleValue(double.Parse($DOUBLE.text, culture)));
    }
  | BOOL
    {
      $Expression = new LiteralExpression(new BoolValue(bool.Parse($BOOL.text)));
    }
  | list
    {
      $Expression = new LiteralExpression($list.List);
    }
//  | filePointer
  //  {
    //  $Expression = new LiteralExpression(new FileValue($filePointer.FilePointer));
    //}
  | ID_CONST
    {
      $Expression = new LiteralExpression(new ConstValue($ID_CONST.text.TrimStart(new[] { '@' })));
    }
  | varIdentifier
    {
      $Expression = new CompoundIdentitfierExpression($varIdentifier.VarIdentifier);
    }
  ;
  
list returns [ListValue List]
@init
{  
  ExpressionCollection Expressions = new ExpressionCollection();
}
@after
{  
  FinishObj($List, retval); 
}
  : LSQ wsns expressionList? RSQ
  {    
    $List = new ListValue($expressionList.Expressions);       
  }
  ;
  
expressionList returns [ExpressionCollection Expressions]
@init
{  
  $Expressions = new ExpressionCollection();
}
@after
{
  FinishObj($Expressions, retval); 
}
  : first=expression wsns  { $Expressions.Add($first.Expression); }
  (COMMA wsns next=expression { $Expressions.Add($next.Expression); } wsns)*
  ; 
  
id returns [Identifier Id]
  : ID
    {
      bool isInvalid = false;
      if (IsKeyword($ID.text))
      {
        Error(string.Format(GetTextRes(KeywordAsIdentifierError), $ID.text));
        isInvalid = true;
      }
    
      $Id = new Identifier($ID.text); // create Identifier object
      FinishObj($Id, $ID, isInvalid); // finish it      
    }
  ;  
 
ML_COMMENT
    :   '/*' (options {greedy=false;} : .)* '*/' {$channel=HIDDEN;}
    ;  
    
SL_COMMENT
    : '//' .* NEWLINE {$channel=HIDDEN;}
    ;    
  
WS  
    : ' '
    | '\t'               
    ;
    
LCUR
    : '{'
    ;
    
RCUR
    : '}'
    ;  
    
LSQ
    : '['
    ;
RSQ
    : ']'
    ;
    
LPAREN
    : '('
    ;

RPAREN
    : ')'
    ;

ASSIGN
    : '='
    ;    
    
SEMICOLON
    : ';'
    ;

COLON
    : ':'
    ;
    
FLOW
    : 'flow'
    ;  
      
//RUN
//    : 'run'
//   ;
    
//OBTAIN
//    : 'obtain'
//    ;
    
//FILE
//    : 'file'
//    ;

APP
    : 'app'
    ;

EXEC
    : 'exec'
    ;    
    
REQUIRE
    : 'require'
    ;
    
//DISABLED
//    : 'disabled'
//    ;    
    
STEP
    : 'step'
    ;
    
SWEEP
    : 'sweep'
    ;    
    
RUNS
    : 'runs'
    ;
    
AFTER
    : 'after'
    ;
   
//FOR
//    : 'for'
//    ;

//IF
//    : 'if'
//    ;
    
ON
    : 'on'
    ;
        
BOOL
    :  ('true') | ('false') 
    ;

ID_CONST
    : '@' ID_BASE
    ;    
    
ID
    : ID_BASE
    ;
    
fragment ID_BASE
    : ('a'..'z'|'A'..'Z'|'_' ) ('a'..'z'|'A'..'Z'|'0'..'9'|'_')*
    ;
    
STRING
    :  '"' ( (~('\\' | '"'))| ESC_SEQ)* '"'
    ;

//CHAR:  '\'' ( ESC_SEQ | (~('\'' | '\\')) ) '\''
//    ;

DOUBLE
    :   ('+' | '-')? DEC_DIGIT+ '.' DEC_DIGIT* EXPONENT?
    |   ('+' | '-')? '.' DEC_DIGIT+ EXPONENT?
    |   ('+' | '-')? DEC_DIGIT+ EXPONENT
    ;

INT : ('+' | '-')? DEC_DIGIT+
    ;



fragment DEC_DIGIT : ('0'..'9') ;

fragment EXPONENT : ('e'|'E') ('+'|'-')? ('0'..'9')+ ;

fragment HEX_DIGIT : ('0'..'9'|'a'..'f'|'A'..'F') ;

fragment ESC_SEQ
    :   '\\' ('b'|'t'|'n'|'f'|'r'|'\"'|'\''|'\\')
    |   UNICODE_ESC
    |   OCTAL_ESC
    ;

fragment OCTAL_ESC
    :   '\\' ('0'..'3') ('0'..'7') ('0'..'7')
    |   '\\' ('0'..'7') ('0'..'7')
    |   '\\' ('0'..'7')    
    ;            

fragment UNICODE_ESC
  : '\\' 'u' HEX_DIGIT HEX_DIGIT HEX_DIGIT HEX_DIGIT
  ;
    
fragment RULETTER
  : '\u0400'..'\u04FF'
  ;         
        
DOT
    : '.'
    ;  

COMMA
    : ','
    ;
    
NEWLINE
  : '\r'? '\n'
  ;   