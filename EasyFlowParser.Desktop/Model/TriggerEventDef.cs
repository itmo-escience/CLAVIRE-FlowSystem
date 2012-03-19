using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easis.Wfs.EasyFlow.Model
{
    /// <summary>
    /// Событие триггера.
    /// </summary>
    public class TriggerEventDef: ParsedObject, IEquatable<TriggerEventDef>
    {
        private string _name;
        private BlockBase _source;
        private bool _isExplicit = false;

        public const string FinishedEventName = "block_finished";
        public const string StepRunInfoReadyEventName = "step_run_info_ready";

        /// <summary>
        /// Возвращает имя события.
        /// </summary>
        public string Name
        {
            get { return _name; }        
        }

        /// <summary>
        /// Возвращает источник события.
        /// </summary>
        public BlockBase Source
        {
            get { return _source;  }
        }

        /// <summary>
        /// Gets whether the event was defined explicitly.
        /// </summary>
        public bool IsExplicit
        {
            get { return _isExplicit; }
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="name">Имя события.</param>
        /// <param name="source">Блок-источник события.</param>
        /// <param name="isExplicit"></param>
        public TriggerEventDef(string name, BlockBase source, bool isExplicit)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (source == null) throw new ArgumentNullException("source");            

            _name = name;
            _source = source;
            _isExplicit = isExplicit;
        }

        public bool Equals(TriggerEventDef other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other._name, _name) && Equals(other._source, _source);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(TriggerEventDef)) return false;
            return Equals((TriggerEventDef) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((_name != null ? _name.GetHashCode() : 0)*397) ^ (_source != null ? _source.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            return string.Format("{0}.{1}", _source, _name);
        }

        protected override object CreateClone()
        {
            return new TriggerEventDef(_name, _source, _isExplicit);
        }
    }
}