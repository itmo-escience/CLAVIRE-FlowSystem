using Easis.Wfs.EasyFlow.Model;

namespace Easis.Wfs.Interpreting
{
    public interface IFileRegistry
    {
        /// <summary>
        /// Регистрирует новый файл в системе, присваивая ему уникальный id.
        /// Если файл с таким id уже есть - обновляет запись.
        /// throws ArgumentException
        /// </summary>
        /// <param name="fileDescriptor"></param>
        void RegisterFile(FileDescriptor fileDescriptor);

        FileDescriptor GetFileById(long id);
        // TODO: IEnumerable
        FileDescriptor [] FindFilesByType(FileDescriptor.FileType fileType);
        
        /// <summary>
        /// Создает новый дескриптор файла с проставленным ID
        /// </summary>
        /// <returns></returns>
        FileDescriptor CreateFileDescriptor();
    }
}