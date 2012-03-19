using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Easis.Wfs.EasyFlow.Utils;

namespace Easis.Wfs.EasyFlow.Model
{
    public enum TriggerEventExpressionType
    {
        Or,
        And
    }

    public class TriggerEventExpression
    {
        private TriggerEventExpressionType _expressionType = TriggerEventExpressionType.And;
        private List<TriggerEventDef> _events = new List<TriggerEventDef>();

        public ListIndexedProperty<TriggerEventDef> Events
        {
            get { return new ListIndexedProperty<TriggerEventDef>(_events); }
        }

        public TriggerEventExpressionType ExpressionType
        {
            get { return _expressionType; }
            set { _expressionType = value; }
        }

        public bool Matches(IEnumerable<TriggerEventDef> events)
        {                                
            if (_expressionType == TriggerEventExpressionType.And)
                return !events.Except(_events).Any(); // AND: да, если вычитание пусто

            return events.Intersect(_events).Any(); // OR: да, если пересечение непусто
        }

        public void AddEvent(TriggerEventDef eventDef)
        {
            if (eventDef == null) throw new ArgumentNullException("eventDef");
            _events.Add(eventDef);
        }

        public TriggerEventExpression()
        {
        }

        public TriggerEventExpression(TriggerEventExpressionType expressionType)
        {
            _expressionType = expressionType;
        }

        public override string ToString()
        {
            return _events.Aggregate("", (s, @event) => s + (s == "" ? "" : (_expressionType == TriggerEventExpressionType.And ? " && " : " || ")) + @event);
        }
    }
}