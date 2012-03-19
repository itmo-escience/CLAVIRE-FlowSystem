using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eventing;
using Easis.Wfs.Interpreting;
using NLog;

namespace ExternalEventListener
{
    /// <summary>
    /// Базовый класс для цепочки обработки событий. 
    /// ChainOfResponsibility
    /// thread safe
    /// </summary>
    abstract class EventConverterBase
    {
        protected static readonly Logger _log = LogManager.GetCurrentClassLogger();
        // Лочатся все интерфейсные методы
        protected object _syncRoot = new object();

        protected EventConverterBase _successor = null;

        public void SetSuccessor(EventConverterBase successor)
        {
            lock (_syncRoot)
                _successor = successor;
        }

        protected abstract bool CanConvert(EventReport eventReport);
        protected abstract FlowEvent Convert(EventReport eventReport);

        public FlowEvent ConvertEvent(EventReport eventReport)
        {
            if(eventReport == null)
                throw new ArgumentNullException();

            lock (_syncRoot)
            {
                if (CanConvert(eventReport))
                {
                    return Convert(eventReport);
                }
                else
                {
                    if (_successor == null)
                    {
                        throw new NullReferenceException("Chain of handlers finished. Event was not handled.");
                    }
                    else
                    {
                        return _successor.ConvertEvent(eventReport);
                    }
                }
            }
        }
    }
}
