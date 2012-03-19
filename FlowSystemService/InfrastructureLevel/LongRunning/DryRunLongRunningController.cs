using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Easis.Wfs.Interpreting;
using MongoDB.Bson;

namespace Easis.Wfs.FlowSystemService.InfrastructureLevel.LongRunning
{
    public class DryRunLongRunningController : ILongRunningController
    {
        public void SendStopCommand(string address)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}