using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Easis.Common.DataContracts
{
    [DataContract]
    public class ExecutionContext : ICloneable
    {
        [DataMember]
        public string UserId { get; set; }
        [DataMember]
        public string TempUserCert { get; set; }
        [DataMember]
        public string Priority { get; set; }
        [DataMember]
        public IDictionary<string, object> ExtraElements = new Dictionary<string, object>();


        public object Clone()
        {
            ExecutionContext ret = new ExecutionContext();
            ret.UserId = UserId;
            ret.TempUserCert = TempUserCert;
            ret.Priority = Priority;

            foreach (var extraElement in ExtraElements)
            {
                ret.ExtraElements.Add(extraElement);
            }
            
            return ret;
        }
    }

    [DataContract]
    public class JobDescription
    {

        [DataMember]
        public ExecutionContext ExecutionContext;

        /// <summary>
        /// Identificator
        /// </summary>
        [BsonElement("&ID")]
        [BsonRepresentation(BsonType.String)]
        [DataMember]
        public Guid ID;

        [BsonElement("_id")]
        [DataMember]
        public ObjectId _id;

        [BsonRepresentation(BsonType.String)]
        [DataMember]
        public Guid WfId;

        [BsonExtraElements]
        public BsonDocument ExtraElements { get; set; }

        /// <summary>
        /// User comment
        /// </summary>
        [DataMember]
        public string Comment;

        #region Timestamps
        [DataMember]
        public Nullable<DateTime> PushedAt;
        [DataMember]
        public Nullable<DateTime> StartedAt;
        [DataMember]
        public Nullable<DateTime> FinishedAt;
        [DataMember]
        public DateTime Timestamp;
        #endregion

        [BsonRepresentation(BsonType.String)]
        [DataMember]
        public JobState State;

        [DataMember]
        public IList<FileDescription> Outputs;

        [DataMember]
        public string InterpreterState;

        [DataMember]
        public IList<NodeDescription> Nodes;

        public enum JobResultStatus
        {
            completed,
            partially_completed,
            failed,
        }

        [DataMember]
        public JobResultStatus WorkflowStatus;

        #region Error
        [DataMember]
        public string ErrorComment;
        [DataMember]
        public string ErrorException;
        [DataMember]
        public string VerboseErrorComment;
        #endregion

        [DataMember]
        public JobRequest JobRequest;

    }

    [DataContract]
    public class RunInfo
    {
        [DataMember]
        public string ExternalId { get; set; }
        
        [DataMember]
        public ResourceRunInfo ResourceInfo { get; set; }
        [DataMember]
        public IEnumerable<NodeRunInfo> NodeInfo { get; set; }

        [DataMember]
        public Nullable<DateTime> Started { get; set; }
        [DataMember]
        public Nullable<DateTime> Ended { get; set; }

        //TODO: make separate class
        [DataMember]
        public double Estimation { get; set; }
        [DataMember]
        public double EstimationDispersion { get; set; }

        [DataMember]
        public Dictionary<string, double> EstimationsOnResources { get; set; }

        [DataMember]
        public string State { get; set; }
    }

    [DataContract]
    public class ResourceRunInfo
    {
        [DataMember]
        public string ResourceType { get; set; }
        [DataMember]
        public string ResourceName { get; set; }
        [DataMember]
        public string GeoCoordinates { get; set; }
        [DataMember]
        public int NodesTotal { get; set; }
        [DataMember]
        public string ResourceDescription { get; set; }
        [DataMember]
        public IEnumerable<string> SupportedArchitectures { get; set; }
        [DataMember]
        public int CoresCount { get; set; }
    }

    [DataContract]
    public class NodeRunInfo
    {
        [DataMember]
        public string NodeName { get; set; }
        [DataMember]
        public IEnumerable<string> SupportedArchitectures { get; set; }
        [DataMember]
        public int CoresUsed;
    }
}


