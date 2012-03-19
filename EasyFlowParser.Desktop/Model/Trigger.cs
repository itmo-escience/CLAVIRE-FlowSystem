using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easis.Wfs.EasyFlow.Model
{
    /// <summary>
    /// Класс, представляющий собой триггер.
    /// </summary>
    public class Trigger
    {
        private TriggerActionDef _actionDef;
        private TriggerEventExpression _eventExpression = new TriggerEventExpression();        

        /// <summary>
        /// Возвращает действие триггера.
        /// </summary>
        public TriggerActionDef ActionDef
        {
            get { return _actionDef; }            
        }

        /// <summary>
        /// Возвращает или устанавливает совокупность событий для запуска триггера.
        /// </summary>
        public TriggerEventExpression EventExpression
        {
            get { return _eventExpression; }
            set { _eventExpression = value; }
        }

        /// <summary>
        /// Проверяет, срабатывают ли все условия для запуска триггеров.
        /// </summary>
        /// <param name="events">Произошедшие события.</param>
        /// <returns>Да / нет.</returns>
        [Obsolete]
        public bool Fires(IEnumerable<TriggerEventDef> events)
        {
            if (events == null) throw new ArgumentNullException("events");

            return _eventExpression.Matches(events);
        }

        public Trigger(TriggerActionDef actionDef)
        {
            if (actionDef == null) throw new ArgumentNullException("actionDef");
            _actionDef = actionDef;
        }

        public override string ToString()
        {
            return string.Format("{0} on {1}", _actionDef, _eventExpression);
        }
    }
}
