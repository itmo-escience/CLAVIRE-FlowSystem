namespace Easis.Wfs.Interpreting
{
    /// <summary>
    /// Интерфейс базы пакетов
    /// </summary>
    public interface IPackageRegistry
    {
        PackageInfo[] FindPackage(string name, string domainName = null);
        PackageInfo FindPackageByPath(string path);
        void RegisterPackage(PackageInfo newPackage);
        void UnregisterPackage(PackageInfo Package);
        string[] GetDomainPackageList(string domainName);
    }
}