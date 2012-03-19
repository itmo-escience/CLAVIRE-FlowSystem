using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Easis.Wfs.FlowSystemService;
using NLog;

namespace FlowSystemServiceDriver
{
    class Program
    {
        static readonly Logger _log = LogManager.GetCurrentClassLogger();
        
        static void Main(string[] args)
        {
            _log.Info("Starting FlowSystemService..");

            ServiceHost proxyServiceHost = new ServiceHost(typeof(FlowSystemService));

            try
            {
                _log.Info("The FlowSystemService configured, opening....");
                proxyServiceHost.Open();
                _log.Info("FlowSystemService started.");
                Console.WriteLine("Press <ENTER> to terminate the service.");
                Console.ReadLine();
                _log.Info("Key pressed -> Closing service..");
                proxyServiceHost.Close();
            }
            catch (CommunicationException e)
            {
                _log.Warn("Communication exception with message {0}", e.Message);
            }
            catch (TimeoutException e)
            {
                _log.Warn("Timeout exception with message {0}", e.Message);
            }

            if (proxyServiceHost.State != CommunicationState.Closed)
            {
                _log.Info("Aborting the service...");
                proxyServiceHost.Abort();
            }
            Console.ReadLine();
        }
    }
}
