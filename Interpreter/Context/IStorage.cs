using Easis.Wfs.EasyFlow.Model;

namespace Easis.Wfs.Interpreting
{
    public interface IStorage
    {
        FileDescriptor SaveFile(string fileName, string fileContent);
    }
}