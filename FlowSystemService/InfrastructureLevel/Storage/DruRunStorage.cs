using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Easis.Wfs.EasyFlow.Model;
using Easis.Wfs.Interpreting;

namespace Easis.Wfs.FlowSystemService.InfrastructureLevel.Storage
{
    public class DruRunStorage : IStorage
    {
        public FileDescriptor SaveFile(string fileName, string fileContent)
        {
            return new FileDescriptor(null, fileName, "example id");
        }
    }
}