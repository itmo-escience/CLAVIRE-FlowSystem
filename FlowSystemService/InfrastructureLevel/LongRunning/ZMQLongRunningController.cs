using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Easis.Wfs.Interpreting;
using MongoDB.Bson;
using ZMQ;

namespace Easis.Wfs.FlowSystemService.PESAbstraction.LongRunning
{
    public sealed class ZmqLongRunningController : ILongRunningController
    {
        //private IDictionary<string, Socket> _socketCache = new Dictionary<string, Socket>();
        private Context _context = new Context(1);

        public ZmqLongRunningController()
        {
        }

        public void SendStopCommand(string address)
        {
            BsonDocument bd = SendCustomCommand(address, "stop", null);
        }

        public BsonDocument SendGetInputParam(string address, string paramName)
        {
            throw new NotImplementedException();
        }

        public BsonDocument SendSetInputParam(string address, string paramName, BsonValue value)
        {
            throw new NotImplementedException();
        }

        public BsonDocument SendGetOutputParam(string address, string paramName)
        {
            throw new NotImplementedException();
        }

        public BsonDocument SendCustomCommand(string address, string commandName, BsonValue commandArg)
        {
            //  Socket to talk to server
            using (Socket requester = _context.Socket(SocketType.REQ))
            {
                requester.Connect(address);
                BsonDocument req = new BsonDocument();
                req.Add(new BsonElement("command", address));
                req.Add(new BsonElement("param", commandArg));
                requester.Send(req.AsByteArray);
                byte[] reply = requester.Recv();
                return BsonDocument.ReadFrom(reply);
            }
        }
    }
}