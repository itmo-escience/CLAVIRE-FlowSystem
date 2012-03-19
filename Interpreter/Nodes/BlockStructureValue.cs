using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Easis.Wfs.EasyFlow.Model;

namespace Easis.Wfs.Interpreting
{
    class BlockStructureValue : StructureValue
    {
        public static string RUN_RESULT_OUTPUTS_NAME = "outs";
        public static string SWEEP_OUTPUTS_NAME = "sweep_outs";
        public const string NAME_FIELD = "Name";

        public const string RUN_RESULT_STATUS_NAME = "Status";

        public const string RUN_RESULT_STATUS_NOT_RUNNED = "NotRunned";
        public const string RUN_RESULT_STATUS_RUNNING = "Running";
        public const string RUN_RESULT_STATUS_FINISHED = "Finished";
        public const string RUN_RESULT_STATUS_FAILED = "Failed";

        public BlockStructureValue(string name = "")
        {
            _fields.Add(NAME_FIELD,new StringValue(name));
            _fields.Add(RUN_RESULT_STATUS_NAME, new StringValue(RUN_RESULT_STATUS_NOT_RUNNED));
        }
    }
}
