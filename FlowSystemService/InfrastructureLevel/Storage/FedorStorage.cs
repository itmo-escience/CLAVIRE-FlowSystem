using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Easis.Wfs.EasyFlow.Model;
using Easis.Wfs.Interpreting;

namespace Easis.Wfs.FlowSystemService.PESAbstraction.Storage
{
    public class FedorStorage : IStorage
    {
        private readonly string _uri;

        public FedorStorage(string uri)
        {
            _uri = uri;
        }

        public FileDescriptor SaveFile(string fileName, string fileContent)
        {
            FedorDataServiceClient cli = new FedorDataServiceClient(_uri);
            byte[] byteArray = Encoding.UTF8.GetBytes(fileContent);
            MemoryStream stream = new MemoryStream(byteArray);
            string id = cli.UploadStream(stream);
            FileDescriptor ret = new FileDescriptor(null, fileName, id);
            return ret;
        }
    }
}