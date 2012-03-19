using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easis.Wfs.EasyFlow.Model
{
    /// <summary>
    /// Требование файла для WF.
    /// </summary>
    public class FileRequirement : ParsedObject
    {
        private string _name;

        /// <summary>
        /// Возвращает идентификатор требуемого файла.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        public FileRequirement(string name)
        {
            if (name == null) throw new ArgumentNullException("name");
            _name = name;
        }

        public override string ToString()
        {
            return string.Format("{0}", _name);
        }

        protected override object CreateClone()
        {
            return new FileRequirement(_name);
        }
    }
}
