using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Easis.Wfs.EasyFlow.Model
{
    /// <summary>
    /// Класс для описания доступа к переменной (someVar[indexer].SomeProperty["someString"])
    /// </summary>
    public class CompoundVarIdentifier : CompoundName<SimpleVarIdentifier>
    {
        public CompoundVarIdentifier()
        {
        }

        public CompoundVarIdentifier(string str)
            : base(str)
        {
        }

        protected override SimpleVarIdentifier MakePart(string part)
        {
            // TODO: indexer parsing?
            return new SimpleVarIdentifier(part);
        }

        public override bool CheckPart(SimpleVarIdentifier part)
        {
            Regex regex = new Regex(@"^[a-z_A-Z]\w*$");

            return regex.IsMatch(part.Name);
        }

        // TODO: check
        /// <summary>
        /// Возврящает новый сложный идентификатор, состоящий из частей, начиная с index'ной (включительно) текущего.
        /// 0-based index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public CompoundVarIdentifier SubIdentifier(int index)
        {
            if (index < 0 || index > NameParts.Count) throw new IndexOutOfRangeException("index");

            if(index == NameParts.Count)
                return new CompoundVarIdentifier();

            CompoundVarIdentifier ret = new CompoundVarIdentifier();
            int n = NameParts.Count;
            for (int i = 0; i < n - index; i++)
            {
                ret.AddPart(NameParts[i + index]);
            }
            return ret;
        }

        public static CompoundVarIdentifier Concat(CompoundVarIdentifier compoundVarIdentifier1, CompoundVarIdentifier compoundVarIdentifier2)
        {
            if (compoundVarIdentifier1 == null) throw new ArgumentNullException("compoundVarIdentifier1");
            if (compoundVarIdentifier2 == null) throw new ArgumentNullException("compoundVarIdentifier2");

            CompoundVarIdentifier ret = new CompoundVarIdentifier(compoundVarIdentifier1.TextBehind + compoundVarIdentifier1.Delimeter + compoundVarIdentifier2.TextBehind);
            return ret;
        }

        public static CompoundVarIdentifier Concat(string compoundVarIdentifier1, string compoundVarIdentifier2)
        {
            CompoundVarIdentifier ret = new CompoundVarIdentifier(compoundVarIdentifier1 + "." + compoundVarIdentifier2);
            return ret;
        }

        protected override object CreateClone()
        {
            return new CompoundVarIdentifier();
        }
    }
}