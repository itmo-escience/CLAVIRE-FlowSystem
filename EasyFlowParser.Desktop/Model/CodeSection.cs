using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Easis.Wfs.EasyFlow.Model
{
    /// <summary>
    /// Класс для хранения секции кода для обработки перед и после запусков, а также в телах IF и FOR.
    /// </summary>
    public class CodeSection : ParsedObject
    {
        protected override object CreateClone()
        {
            return new CodeSection();
        }
    }
}
