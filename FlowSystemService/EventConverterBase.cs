using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Eventing;
using Easis.Wfs.Interpreting;
using NLog;

namespace Easis.Wfs.FlowSystemService
{
    /// <summary>
    /// Базовый класс для цепочки обработки событий. 
    /// ChainOfResponsibility
    /// thread safe
    /// </summary>
    abstract class EventConverterBase
    {
        protected static readonly Logger _slog = LogManager.GetCurrentClassLogger();
        // Лочатся все интерфейсные методы
        protected object _syncRoot = new object();

        protected EventConverterBase _successor = null;

        public void SetSuccessor(EventConverterBase successor)
        {
            lock (_syncRoot)
                _successor = successor;
        }

        protected abstract bool InternalCanConvert(EventReport eventReport);
        protected abstract FlowEvent Convert(EventReport eventReport);
        
        public bool TryConvertEvent(EventReport eventReport, out FlowEvent res)
        {
            if(eventReport == null)
                throw new ArgumentNullException();

            lock (_syncRoot)
            {
                if (InternalCanConvert(eventReport))
                {
                    res = Convert(eventReport);
                    return true;
                }
                else
                {
                    if (_successor == null)
                    {
                        res = null;
                        return false;
                    }
                    else
                    {
                        return _successor.TryConvertEvent(eventReport, out res);
                    }
                }
            }
        }
    }

}
