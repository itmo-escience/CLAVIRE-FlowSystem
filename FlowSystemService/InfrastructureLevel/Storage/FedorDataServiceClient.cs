using System;
using System.IO;
using System.Net;

namespace Easis.Wfs.FlowSystemService.PESAbstraction.Storage
{
    /// <summary>
    ///   Class for accessing DataService (Podtelkin's).
    /// </summary>
    public class FedorDataServiceClient
    {
        /// <summary>
        ///   Data service endpoint.
        /// </summary>
        private string _uri;

        /// <summary>
        ///   Data service endpoint.
        /// </summary>
        public string Uri
        {
            get { return _uri; }
            set { _uri = value; }
        }

        /// <summary>
        ///   Initializes a new instance of FedorDataServiceClient.
        /// </summary>
        /// <param name = "uri">uri of HttpDataService.</param>
        public FedorDataServiceClient(string uri)
        {
            if (uri == null) throw new ArgumentNullException("uri");
            _uri = uri;
        }

        /// <summary>
        ///   Uploads stream to the server.
        /// </summary>
        /// <returns>Uploaded data id.</returns>
        public string UploadStream(Stream sourceStream)
        {
            if (sourceStream == null) throw new ArgumentNullException("sourceStream");
            if (!sourceStream.CanRead)
                throw new IOException("Source stream doesn't allow reading.");

            // Trying to send source stream to the server:
            var request = (HttpWebRequest)WebRequest.Create(_uri);
            //request.Proxy = new WebProxy("http://proxy.ifmo.ru:3128/");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = sourceStream.Length;

            using (Stream postStream = request.GetRequestStream())
                StreamHelper.Copy(sourceStream, postStream);

            string uploadedDataId = null;
            using (var response = request.GetResponse())
            using (Stream streamResponse = response.GetResponseStream())
            {
                var responseReader = new StreamReader(streamResponse);
                uploadedDataId = responseReader.ReadToEnd();
            }

            return uploadedDataId;
        }


        /// <summary>
        ///   Downloads file with specified id to stream.
        /// </summary>
        /// <param name = "dataId">Data id.</param>
        /// <param name="targetStream"></param>
        public void DownloadStream(string dataId, Stream targetStream)
        {
            if (dataId == null) throw new ArgumentNullException("dataId");
            if (targetStream == null) throw new ArgumentNullException("targetStream");

            var requestUriString = new Uri(string.Format("{0}?data_id={1}", _uri, dataId)); // request uri string
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUriString);

            using (var response = request.GetResponse())
            using (Stream responseStream = response.GetResponseStream())
                StreamHelper.Copy(responseStream, targetStream);
        }
    }
}
