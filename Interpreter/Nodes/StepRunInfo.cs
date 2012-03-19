using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Easis.Common.DataContracts;

namespace Easis.Wfs.Interpreting
{
    /// <summary>
    /// Информация о запущенном шаге
    /// </summary>
    public class StepRunInfo
    {
        public string ExternalId { get; set; }

        public ResourceRunInfo ResourceInfo { get; set; }
        public IEnumerable<NodeRunInfo> NodeInfos { get; set; }

        public enum StepRunState
        {
            Initialization,
            Started
        }

        public StepRunState State { get; set; }

        public Nullable<DateTime> Started { get; set; }
        public Nullable<DateTime> Ended { get; set; }
        public Nullable<TimeSpan> Estimated { get; set; }
    }

    public class LongRunningStepRunInfo : StepRunInfo
    {
        public string CommandEndpoint { get; set; }
        public string PublishingEndpoint { get; set; }
        public long Id { get; set; }
    }
}
