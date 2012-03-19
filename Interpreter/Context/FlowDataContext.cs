using System.Collections.Generic;

namespace Easis.Wfs.Interpreting
{
    /// <summary>
    /// Контекст WF, куда входят все входные данные, заданные пользователем.
    /// </summary>
    public class FlowDataContext
    {
        // идентификатор -> id в стораже
        private IDictionary<string, string> _inputFiles;
        // идентификатор -> значение
        private IDictionary<string, string> _inputParameters;

        public IDictionary<string, string> InputFiles
        {
            get { return _inputFiles; }
        }

        public IDictionary<string, string> InputParameters
        {
            get { return _inputParameters; }
        }

        public FlowDataContext()
        {
            _inputFiles = new Dictionary<string, string>();
            _inputParameters = new Dictionary<string, string>();
        }
    }
}
