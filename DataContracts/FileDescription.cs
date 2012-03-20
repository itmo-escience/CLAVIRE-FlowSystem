using System.Runtime.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Easis.Common.DataContracts
{
    [DataContract]
    public class FileDescription
    {
        [DataMember]
        public string FileIdentifier;
        [DataMember]
        public string StorageId;
        [DataMember]
        public string NStorageId;
        [DataMember]
        public string Id;
        [DataMember]
        public string Type;

        [BsonExtraElements]
        public BsonDocument ExtraElements { get; set; }

    }
}