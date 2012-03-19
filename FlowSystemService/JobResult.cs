using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using Easis.Common.DataContracts;

namespace Easis.Wfs.FlowSystemService
{
    [DataContract]
    public class JobResult
    {
        [DataMember]
        public Guid WfId;

        public enum JobResultStatus
        {
            complited,
            partially_complited,
            failed,
        }

        #region Error
        [DataMember]
        public string ErrorComment;
        [DataMember]
        public string ErrorException;
        [DataMember]
        public string VerboseErrorComment;
        #endregion

        #region Timestamps
        [DataMember]
        public DateTime TimestampStarted;
        [DataMember]
        public DateTime TimestampFinished;
        [DataMember]
        public DateTime TimestampPushed;
        #endregion

        [DataMember]
        public string StateInJson = "";

        [DataMember]
        public JobDescription JobDescription { get; set; }

        private JobRequest _jobRequest;

        public JobResult(JobRequest jobRequest)
        {
            _jobRequest = jobRequest;
        }

        [DataMember]
        public JobRequest JobRequest
        {
            get { return _jobRequest; }
        }
    }
}