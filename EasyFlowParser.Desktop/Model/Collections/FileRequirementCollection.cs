using System;
using System.Collections.Generic;
using System.Linq;

namespace Easis.Wfs.EasyFlow.Model
{
    public class FileRequirementCollection : ParsedCollection<FileRequirement>,  ICloneable
    {
        public FileRequirementCollection()
        {
        }

        public FileRequirementCollection(IEnumerable<FileRequirement> itemCollection): base(itemCollection)
        {
        }

        public object Clone()
        {
            return new FileRequirementCollection(this.Select(requirement => (FileRequirement)requirement.Clone()));
        }
    }
}
