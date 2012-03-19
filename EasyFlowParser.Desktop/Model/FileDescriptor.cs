using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easis.Wfs.EasyFlow.Model
{
    /// <summary>
    /// Указатель на файл.
    /// </summary>
    public class FileDescriptor : ParsedObject, ICloneable
    {
        private long? _id;
        public long? Id { get { return _id; } set { _id = value; } }

        private string _nStorageId;

        /// <summary>
        /// Old storage id (java based)
        /// </summary>
        public string NStorageId { get { return _nStorageId; } set { _nStorageId = value; } }

        private string _storageId;

        /// <summary>
        /// New storage id.
        /// </summary>
        public string StorageId
        {
            get { return _storageId; }
            set { _storageId = value; }
        }

        // Через идентификатор связываются required файлы и datacontext
        private string _fileIdentifier;
        public string FileIdentifier { get { return _fileIdentifier; } set { _fileIdentifier = value; } }

        public FileDescriptor(  long? id = null,
                                string fileIdentifier = null,
                                string nStorageId = null,
                                string storageId = null)
        {
            _nStorageId = nStorageId;
            _fileIdentifier = fileIdentifier;
            _storageId = storageId;
            _id = id;
        }

        public FileType Type { get; set; }

        [Flags]
        public enum FileType
        {
            Required,
            CreatedInScript,
            GeneratedAfterRun,
            Return
        }

        protected override object CreateClone()
        {
            return new FileDescriptor(_id, _fileIdentifier, _nStorageId, _nStorageId);
        }

    }
}
