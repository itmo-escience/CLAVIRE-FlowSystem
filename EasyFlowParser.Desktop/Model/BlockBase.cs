using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Easis.Wfs.EasyFlow.Utils;

namespace Easis.Wfs.EasyFlow.Model
{    
    /// <summary>
    /// Base class for flow block.
    /// </summary>
    public abstract class BlockBase : ParsedObject
    {
        private long _id;
        private Flow _flow; // parent flow
        private Dictionary<string, Trigger> _triggers = new Dictionary<string, Trigger>(); // triggers dictionary

        /// <summary>
        /// Gets wf-unique id.
        /// </summary>
        public long Id
        {
            get { return _id; }
            set
            {
                if (value == _id) return;
                _id = value;
            }
        }

        /// <summary>
        /// Gets block's parent flow.
        /// </summary>
        public Flow Flow
        {
            get { return _flow; }
            internal set
            {
                if (value == _flow) return;
                _flow = value;
            }
        }        

        /// <summary>
        /// Gets triggers dictionary.
        /// </summary>
        public DictionaryIndexedProperty<string, Trigger> Triggers
        {
            get { return new DictionaryIndexedProperty<string, Trigger>(_triggers); }
        }

        internal void AddTrigger(Trigger trigger)
        {
            _triggers.Add(trigger.ActionDef.Name, trigger);
        }

        /// <summary>
        /// Gets whether the block is disabled.
        /// </summary>
        public bool IsDisabled { get; set; }

        /// <summary>
        /// Конструктор без параметров.
        /// </summary>
        protected BlockBase()
        {            
            // TODO: подумать. возможно это нужно переделать
            AddTrigger(new Trigger(new TriggerActionDef(TriggerActionDef.ACTION_START)));
        }

        public override string ToString()
        {
            return string.Format("Flow: {0}", _id);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_id.GetHashCode()*397) ^ (_flow != null ? _flow.GetHashCode() : 0);
            }
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override object Clone()
        {
            var clone = (BlockBase)base.Clone();

            clone.Flow = Flow;
            clone._triggers = _triggers; // TODO: clone triggers

            return clone;
        }
    }    
}