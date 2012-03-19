using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Easis.Wfs.EasyFlow.Model;

namespace Easis.Wfs.Interpreting
{
    /// <summary>
    /// Результат выполнения запуска шага.
    /// Конвертируется в объект внутреннего представления EasyFlow - result.
    /// </summary>
    public class StepRunResult
    {
        public enum ResultStatus
        {
                Completed,
                Failed,
                Unknown // DryRun
        }

        public string ExternalId { get; set; }

        public string ErrorComment { get; set; }

        public ResultStatus Status { get; set; }

        private readonly ICollection<FileDescriptor> _outputFiles = new List<FileDescriptor>();

        public IDictionary<string, string> OutputParams { get; set; }

        public ICollection<FileDescriptor> OutputFiles
        {
            get { return _outputFiles; }
        }

    }
}
