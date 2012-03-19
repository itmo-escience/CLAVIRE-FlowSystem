using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Easis.Wfs.EasyFlow.Model;

namespace Easis.Wfs.Interpreting
{
    // Кэширование глобальных переменных добавить сюда в метод GetValue

    public class BlockDataScope : DataScope
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public bool HasName
        {
            get { return !String.IsNullOrEmpty(_name); }
        }

        public new GlobalDataScope ParentScope
        {
            get { return (GlobalDataScope)base.ParentScope; }
        }

        public BlockDataScope(DataScope parent, string name = null) : base(parent)
        {
            _name = name;
        }

        public override void SetValue(CompoundVarIdentifier identifier, ValueBase value, IEvaluationContext evaluationContext)
        {
            if (identifier == null) throw new ArgumentNullException("identifier");
            if (identifier.Count() == 0) throw new ArgumentException("identifier is empty");

            string first = identifier.First().Name;

            bool ret = false;

            // Ищем в переменных
            if (!ret)
            {
                var namedVar = from variable in Variables where variable.Name == first select variable;
                if (namedVar.Count() > 0)
                {
                    System.Diagnostics.Debug.Assert(namedVar.Count() == 1);

                    // есть переменная с таким именем/ привсаиваем новое значение
                    namedVar.First().SetValue(identifier, value,evaluationContext);

                    ret = true;
                }
            }

            // У родителя не ищем

            if (!ret) throw new InvalidOperationException(String.Format("Undefined variable {0} in current block scope", identifier.TextBehind));

        }
    }
}
