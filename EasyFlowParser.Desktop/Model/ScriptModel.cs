using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Easis.Wfs.EasyFlow.Utils;

namespace Easis.Wfs.EasyFlow.Model
{
    /// <summary>
    /// Класс, представляющий собой внутреннее представление скрипта EasyFlow.
    /// </summary>
    public class ScriptModel : ParsedObject
    {
        private Flow _mainFlow;

        public FileRequirementCollection Requirements { get; set; }

        /// <summary>
        /// Возвращает или устанавливает тело главного потока (flow {}).
        /// </summary>
        public Flow MainFlow
        {
            get { return _mainFlow; }
            set { _mainFlow = value; }
        }
        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public ScriptModel()
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="mainFlow">Тело главного потока (flow {}).</param>
        public ScriptModel(Flow mainFlow)
        {
            if (mainFlow == null) throw new ArgumentNullException("mainFlow");
            _mainFlow = mainFlow;
        }

        protected override object CreateClone()
        {
            return new ScriptModel();
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override object Clone()
        {
            var clone = (ScriptModel)base.Clone();

            clone._mainFlow = (Flow) _mainFlow.Clone();
            clone.Requirements = (FileRequirementCollection) Requirements.Clone();


            return clone;
        }
    }
}
