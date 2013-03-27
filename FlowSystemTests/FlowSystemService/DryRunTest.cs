using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Easis.Common.DataContracts;
using Easis.Wfs.EasyFlow.Model;
using Easis.Wfs.FlowSystemService;
using Easis.Wfs.FlowSystemService.InfrastructureLevel.LongRunning;
using Easis.Wfs.FlowSystemService.InfrastructureLevel.Storage;
using Easis.Wfs.Interpreting;
using ExecutionContext = Easis.Common.DataContracts.ExecutionContext;

namespace FlowSystemTests.FlowSystemService
{
    using NUnit.Framework;

    [TestFixture]
    class DryRunTest
    {
        [Test]
        public void DryRun()
        {
            Job job = new Job(new JobRequest(){Script = @"
step step1 runs Orca.Dft() 
post code ruby
puts 4
code end;

step step2 runs Testp.arithm ( A = step1.Result.outs[""33""], B  = 3, C = step1.Post[""a""]);
step step3 runs Testp.arithm ( A = sweep [1,2,3], B = step2.Result.outs );"

            }, Guid.NewGuid());
            job.Execute(new FlowSystemContext(), new NormalExecutionStepStarter(),new DruRunStorage(), new DryRunLongRunningController() );
            //job.DryRun();
        }
    }
}
