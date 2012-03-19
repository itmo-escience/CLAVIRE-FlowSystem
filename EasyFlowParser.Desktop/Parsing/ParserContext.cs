using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Easis.Wfs.EasyFlow.Model;

namespace Easis.Wfs.EasyFlow.Parsing
{
    internal class TriggerForwardDefinition : ParsedObject
    {
        public string TriggerName { get; set; }
        public BlockBase TriggerOwner { get; set; }
        public string EventName { get; set; }
        public string EventSourceStepName { get; set; }
        public bool IsExplicit { get; set; }
        protected override object CreateClone()
        {
            throw new NotImplementedException();
        }
    }

    internal class ParserContext
    {
        private Flow _flow;
        private FileRequirementCollection _requirements = new FileRequirementCollection();
        private string _name = Guid.NewGuid().ToString();
        private Scope _rootScope = new Scope(ScopeType.Script);
        private IdentifierTable _identifierTable = new IdentifierTable();

        private List<TriggerForwardDefinition> _futureTriggers = new List<TriggerForwardDefinition>();

        public ParserContext()
        {
            _flow = new Flow();
        }

        public IdentifierTable IdentifierTable
        {
            get { return _identifierTable; }
        }

        public List<TriggerForwardDefinition> FutureTriggers
        {
            get { return _futureTriggers; }
        }

        public FileRequirementCollection Requirements
        {
            get { return _requirements; }
        }

        public Flow Flow
        {
            get { return _flow; }
        }

        public Scope RootScope
        {
            get { return _rootScope; }
        }

        /// <summary>
        /// Возвращает имя контекста.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public ParserContext(string name) : this()
        {
            if (name == null) throw new ArgumentNullException("name");
            _name = name;
        }

        public ParserContext(string name, Scope rootScope) : this(name)
        {
            if (rootScope == null) throw new ArgumentNullException("rootScope");
            _rootScope = rootScope;
        }

        public override string ToString()
        {
            return string.Format("{0}", _name);
        }
    }
}
