using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easis.Wfs.EasyFlow.Model
{
    /// <summary>
    /// Класс с описанием действия триггера.
    /// </summary>
    public class TriggerActionDef : ParsedObject
    {
        private string _name;

        public const string ACTION_START = "action_start";

        /// <summary>
        /// Имя действия.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="name">Имя действия.</param>
        public TriggerActionDef(string name)
        {
            _name = name;
        }

        public override string ToString()
        {
            return string.Format("{0}", _name);
        }

        protected override object CreateClone()
        {
            return new TriggerActionDef(_name);
        }
    }
}