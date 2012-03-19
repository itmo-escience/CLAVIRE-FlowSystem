using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Easis.Wfs.EasyFlow.Model;

namespace Easis.Wfs.Interpreting
{
    class PackageNameResolver
    {
        //TODO: Заглушка, реализовать нормально с использованием знаний о базе пакетов
        public string ResolvePackageName(RunObjectName runObjectName)
        {
            if (runObjectName == null) throw new ArgumentNullException("runObjectName");
            if (runObjectName.NameParts == null || runObjectName.NameParts.Count == 0)
            {
                throw new ArgumentException("RunObjectName "+runObjectName+" contains no name parts");
            }

            return runObjectName.NameParts[0];
        }

        public string ResolveMethodName(RunObjectName runObjectName)
        {
            if (runObjectName == null) throw new ArgumentNullException("runObjectName");
            if (runObjectName.NameParts == null || runObjectName.NameParts.Count == 0)
            {
                throw new ArgumentException("RunObjectName " + runObjectName + " contains no name parts");
            }
            
            string ret;

            if(runObjectName.NameParts.Count == 1)
                ret = "";
            else
            {
                ret = runObjectName.NameParts[1];
            }
            
            return ret;
        }
    }
}
