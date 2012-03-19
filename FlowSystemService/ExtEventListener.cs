using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Eventing;
using Easis.Wfs.Interpreting;
using NLog;

namespace Easis.Wfs.FlowSystemService
{
    public class ExtEventListener
    {
        protected static readonly Logger _log = LogManager.GetCurrentClassLogger();
        private EventConverterBase _convertersChain = null;
        private IEventConsumer _eventConsumer = null;

        public ExtEventListener(IEventConsumer eventConsumer)
        {
            _eventConsumer = eventConsumer;
            _convertersChain = new PESEventConverter();

            _convertersChain.SetSuccessor(new ExecutionEventConverter());
            // TODO; вынести event sink в конфигурацию
        }

        public void Notify(Notification notification)
        {
            try
            {
                FlowEvent fevent = null;
                bool res = _convertersChain.TryConvertEvent(notification.Event, out fevent);
                if(res)
                    _eventConsumer.PushEvent(fevent);
                else
                    _log.Warn(String.Format("There is no compatible event handler in chain. Event was not handled. Event: {0} from {1}", notification.Event.Topic, notification.Event.Source));
            }
            catch (Exception ex)
            {
                _log.ErrorException("Exception cought while converting event in chain",ex);
            }
        }
    }
}
