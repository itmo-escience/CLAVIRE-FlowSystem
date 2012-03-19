using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Antlr.Runtime;
using Easis.Wfs.EasyFlow.Model;

namespace Easis.Wfs.EasyFlow.Parsing
{
    public static class EasyFlowParserHelper
    {        
        private static ParseResult Parse(ICharStream inputStream)
        {
            ScriptModel result = null;

            EasyFlowLexer lexer = new EasyFlowLexer(inputStream);
            CommonTokenStream tokenStream = new CommonTokenStream(lexer);

            EasyFlowParser parser = new EasyFlowParser(tokenStream);
            EasyFlowParser.program_return programReturn = parser.program();
            

            return  new ParseResult(null, null);
        }

        public static ParseResult Parse(Stream stream)
        {
            return Parse(new ANTLRInputStream(stream));
        }

        public static ParseResult Parse(string script)
        {
            return Parse(new ANTLRStringStream(script));
        }
    }
}
