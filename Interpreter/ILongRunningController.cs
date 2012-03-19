using MongoDB.Bson;

namespace Easis.Wfs.Interpreting
{
    public interface ILongRunningController
    {
        void SendStopCommand(string address);
        BsonDocument SendGetInputParam(string address, string paramName);
        BsonDocument SendSetInputParam(string address, string paramName, BsonValue value);
        BsonDocument SendGetOutputParam(string address, string paramName);
        BsonDocument SendCustomCommand(string address, string commandName, BsonValue commandArg);
    }
}