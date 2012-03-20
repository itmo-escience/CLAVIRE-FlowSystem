using System.Collections.Generic;
using System.Runtime.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Easis.Common.DataContracts
{
    [DataContract]
    public class NodeDescription
    {
        [DataMember]
        public uint StepId;
        [DataMember]
        public string Name;
        [DataMember]
        public string Type;
        [DataMember]
        public string State;
        [DataMember]
        public string PackageName;
        [DataMember]
        public string MethodName;
        [DataMember]
        public List<uint> Parents;
        [DataMember]
        public List<uint> Children;
        
        [DataMember]
        public string ExternalId;

        //[DataMember]
        //[BsonDefaultValue(null)]
        //public Dictionary<string, string> InParams;

        [DataMember]
        public List<FileDescription> InFiles;

        [DataMember]
        public List<FileDescription> OutFiles;
        
        [BsonExtraElements]
        public BsonDocument ExtraElements { get; set; }

        #region Error
        [DataMember]
        public string ErrorComment;
        [DataMember]
        public string ErrorException;
        [DataMember]
        public string VerboseErrorComment;
        #endregion

        [DataMember]
        public RunInfo RunInfo { get; set; }
    }
}