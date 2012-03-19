using System;
using System.Collections.Generic;
using System.Linq;
using Easis.Wfs.EasyFlow.Model;
using Easis.Wfs.EasyFlow.Model.Expressions;
using Easis.Wfs.Interpreting.Utils;

namespace Easis.Wfs.Interpreting
{
    public class ListBasedPackageRegistry : IPackageRegistry
    {
        private ICollection<PackageInfo> _packages = new List<PackageInfo>();

        public ListBasedPackageRegistry()
        {
            // ============= SEMP
            PackageInfo semp = new PackageInfo("QuantumChemistry", "SEMP") { Comment = "Пакет SEMP." };
            ICollection<PackageMethodInfo> methods = new List<PackageMethodInfo>();
            PackageMethodInfo zindo = new PackageMethodInfo("zindo");
            zindo.Comment = "Zindo method";

            List<ParamInfo> ps = new List<ParamInfo>();

            ps.Add(new ParamInfo() { DataType = DataType.Int, Name = "dumping", Required = false });
            ps.Add(new ParamInfo() { DataType = DataType.Double, Name = "precision", Required = false });
            ps.Add(new ParamInfo() { DataType = DataType.File, Name = "xyz_molecule", Required = true });

            List<ResultInfo> rs = new List<ResultInfo>();

            rs.Add(new ResultInfo() { DataType = DataType.File, Name = "zindo_output_file" });
            rs.Add(new ResultInfo() { DataType = DataType.File, Name = "semp_cube_out" });

            zindo.Params = ps;
            zindo.Results = rs;

            methods.Add(zindo);
            semp.Methods = methods;
            _packages.Add(semp);

            // ============= TESTP
            PackageInfo testp = new PackageInfo("QuantumChemistry", "TESTP") { Comment = "Пакет TESTP." };
            methods = new List<PackageMethodInfo>();
            PackageMethodInfo arithm = new PackageMethodInfo("arithm");
            arithm.Comment = "Arithm method";

            ps = new List<ParamInfo>();

            ps.Add(new ParamInfo() { DataType = DataType.String, Name = "operation", Required = true });
            ps.Add(new ParamInfo() { DataType = DataType.File, Name = "in_file_0", Required = true });
            ps.Add(new ParamInfo() { DataType = DataType.File, Name = "in_file_1", Required = true });

            rs = new List<ResultInfo>();

            rs.Add(new ResultInfo() { DataType = DataType.File, Name = "out_file" });

            arithm.Params = ps;
            arithm.Results = rs;

            methods.Add(arithm);
            testp.Methods = methods;
            _packages.Add(testp);

        }

        public PackageInfo[] FindPackage(string name, string domainName = null)
        {
            var ret = from package in _packages
                      where
                          package.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase) &&
                          (domainName == null ? true : package.Domain == domainName)
                      select package;

            if (ret.Count() == 0)
            {
                throw new ObjectNotFoundException();
            }
            else
            {
                return ret.ToArray();
            }
        }

        /// <summary>
        /// Строка, содержащая 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public PackageInfo FindPackageByPath(string path)
        {
            if (path == null) throw new ArgumentNullException("path");

            string[] parts = path.Split(new char[] { '.' });
            if (parts.Length < 2)
            {
                throw new ArgumentException(String.Format("Path '{0}' is not correct", path));
            }

            PackageInfo[] PackageInfo;

            // исключение, специально не ловим
            PackageInfo = FindPackage(parts[1], parts[0]);

            return PackageInfo[0];
        }

        public void RegisterPackage(PackageInfo newPackage)
        {
            throw new NotImplementedException();
        }

        public void UnregisterPackage(PackageInfo Package)
        {
            throw new NotImplementedException();
        }

        public string[] GetDomainPackageList(string domainName)
        {
            throw new NotImplementedException();
        }
    }
}