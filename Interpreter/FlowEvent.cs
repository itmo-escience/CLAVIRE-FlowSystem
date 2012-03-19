using System;

namespace Easis.Wfs.Interpreting
{
    /// <summary>
    /// Событие, относящиеся к шагам WF.
    /// </summary>
    public class FlowEvent
    {
        #region Events
        public const string FLOW_STARTED = "flow_started";
        public const string BLOCK_STARTED = "block_started";
        public const string PRE_SECTION_STARTED = "pre_section_started";
        public const string PRE_SECTION_FINISHED = "pre_section_finished";
        public const string RUN_STARTED = "run_started";
        public const string RUN_FINISHED = "run_finished";
        public const string POST_SECTION_STARTED = "pre_section_started";
        public const string POST_SECTION_FINISHED = "pre_section_finished";
        public const string BLOCK_FINISHED = "block_finished";
        public const string FLOW_FINISHED = "flow_finished";
        public const string BLOCK_FINISHED_WITH_ERROR = "block_finished_with_error";

        public const string START_SWEEPING = "start_sweeping";

        public const string BLOCK_RESUMED = "block_resumed";
        public const string BLOCK_STOPPED = "block_stopped";
        public const string BLOCK_PAUSED = "block_paused";

        public const string ERROR = "error";

        public const string STEP_RUN_INFO_READY = "step_run_info_ready";

        // Special event types, enebling user control
        public const string CONTROL_BLOCK_PAUSE = "control_block_pause";
        public const string CONTROL_BLOCK_STOP = "control_block_stop";
        public const string CONTROL_BLOCK_RESUME = "control_block_resume";

        public const string CONTROL_FLOW_RESUME = "flow_resumed";
        public const string CONTROL_FLOW_STOP = "flow_stopped";
        public const string CONTROL_FLOW_PAUSE = "flow_paused";

        #endregion

        public const long UNDEFINED_ID = -1;

        private readonly string _name;
        private readonly Guid _wfId;
        private readonly long _blockId;
        private readonly object _eventArg;

        public FlowEvent(string name, Guid wfId, long blockId, object eventArg = null)
        {
            _name = name;
            _wfId = wfId;
            _blockId = blockId;
            _eventArg = eventArg;
        }

        public string Name
        {
            get { return _name; }
        }

        public Guid WfId
        {
            get { return _wfId; }
        }

        public long BlockId
        {
            get { return _blockId; }
        }

        public object EventArg
        {
            get { return _eventArg; }
        }

        public override string ToString()
        {
            return String.Format("@{0}(WF#{1}.{2} +{3})", _name, _wfId, _blockId,_eventArg ?? "nullarg");
        }
    }
}
