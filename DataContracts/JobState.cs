using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easis.Common.DataContracts
{
    public enum JobState
    {
        Created,
        Parsing,
        Parsed,
        Validating,
        Validated,
        DryRun,
        DryRunFinished,
        Active,
        Finished,
        Error
    }
}
