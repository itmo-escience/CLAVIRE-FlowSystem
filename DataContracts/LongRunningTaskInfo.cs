using System;
using System.Runtime.Serialization;

namespace Easis.Common.DataContracts
{
    [DataContract]
    public class LongRunningTaskInfo
    {
        [DataMember]
        public NodeDescription Description { get; set; }
        [DataMember]
        public Guid WfId { get; set; }

        [DataMember]
        public string CommandEndpoint { get; set; }
        [DataMember]
        public string PublishingEndpoint { get; set; }

    }
}