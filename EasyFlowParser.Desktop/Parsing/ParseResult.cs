using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Easis.Wfs.EasyFlow.Model;

namespace Easis.Wfs.EasyFlow.Parsing
{
    /// <summary>
    /// Результат разбора скрипта.
    /// </summary>
    public class ParseResult
    {
        private ScriptModel _scriptModel;
        private ParserMessageCollection _parserMessageCollection;
        private bool _succeeded;

        public bool Succeeded
        {
            get { return _succeeded; }
            internal set
            {
                _succeeded = value;
            }
        }

        public ParserMessageCollection ParserMessageCollection
        {
            get { return _parserMessageCollection; }
        }

        public ScriptModel ScriptModel
        {
            get { return _scriptModel; }            
        }

        public ParseResult(ScriptModel scriptModel, ParserMessageCollection parserMessageCollection)
        {
            if (scriptModel == null) throw new ArgumentNullException("scriptModel");
            if (parserMessageCollection == null) throw new ArgumentNullException("parserMessageCollection");
            _scriptModel = scriptModel;
            _parserMessageCollection = parserMessageCollection;
            _succeeded = true;
        }
    }
}
