using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Easis.Wfs.EasyFlow.Model;

namespace Easis.Wfs.Interpreting
{
    public class FileParam
    {
        public FileDescriptor FileDescriptor { get; set; }
        public string RoleName { get; set; }

        public FileParam(FileDescriptor fileDescriptor, string roleName)
        {
            FileDescriptor = fileDescriptor;
            RoleName = roleName;
        }
    }

    public class StepRunParameters
    {
        private readonly IDictionary<string, string> _params = new Dictionary<string, string>();
        private ICollection<FileParam> _inputFiles = new List<FileParam>();
        private ICollection<FileParam> _outputFiles = new List<FileParam>();

        public IDictionary<string, string> Params
        {
            get { return _params; }
        }

        public ICollection<FileParam> InputFiles
        {
            get { return _inputFiles; }
        }

        public ICollection<FileParam> OutputFiles
        {
            get { return _outputFiles; }
        }

        public StepRunParameters()
        {
        }
    }
}
