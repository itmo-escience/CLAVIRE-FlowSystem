using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Easis.Common;
using Easis.Common.IdGenerators;
using Easis.Wfs.EasyFlow.Model;

namespace Easis.Wfs.Interpreting
{
    public class DictBasedFileRegistry : IFileRegistry
    {
        private object _syncRoot = new object();

        //TODO: IoC injection
        private IIdGenerator<long> _idGenerator = new LongIdGenerator();

        private IDictionary<long, FileDescriptor> _files = new Dictionary<long, FileDescriptor>();

        /// <summary>
        /// Регистрирует новый файл в системе, присваивая ему уникальный id.
        /// Если файл с таким id уже есть - обновляет запись.
        /// throws ArgumentException
        /// </summary>
        /// <param name="fileDescriptor"></param>
        public void RegisterFile(FileDescriptor fileDescriptor)
        {
            if (fileDescriptor == null) throw new ArgumentNullException("fileDescriptor");

            lock (_syncRoot)
            {
                // если файл новый
                if (!fileDescriptor.Id.HasValue)
                {
                    fileDescriptor.Id = _idGenerator.GetId();
                    _files.Add(fileDescriptor.Id.Value,fileDescriptor);
                }
                else
                {
                    if (_files.ContainsKey(fileDescriptor.Id.Value))
                    {
                        // запись о файле уже есть, обновляем
                        _files[fileDescriptor.Id.Value] = fileDescriptor;
                    }
                    else
                    {
                        throw new ArgumentException("File id had been set but file is not in registry");
                    }
                }
            }
        }

        public FileDescriptor GetFileById(long id)
        {
            lock (_syncRoot)
            {
                return _files[id];
            }
        }

        public FileDescriptor [] FindFilesByType(FileDescriptor.FileType fileType)
        {
            lock (_syncRoot)
            {
                var ret = from file in _files.Values where file.Type == fileType select file;
                return ret.ToArray();
            }
        }

        public FileDescriptor CreateFileDescriptor()
        {
            FileDescriptor fd = new FileDescriptor(_idGenerator.GetId());
            return fd;
        }
    }
}
