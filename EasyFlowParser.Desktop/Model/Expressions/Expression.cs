using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easis.Wfs.EasyFlow.Model
{
    /// <summary>
    /// Класс узла дерева выражения.
    /// </summary>
    public abstract class Expression : ParsedObject, IEvaluatable
    {
        /// <summary>
        /// Вычисляет значение данного объекта с использованием переданного контекста.
        /// </summary>
        /// <param name="ctx">Контекст вычисления.</param>
        /// <returns>Результат вычисления.</returns>
        public abstract ValueBase Evaluate(IEvaluationContext ctx);
    }
}