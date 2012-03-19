namespace Easis.Wfs.EasyFlow.Model
{
    /// <summary>
    /// Интерфейс вычислимых в ValueBase объектов.
    /// </summary>
    public interface IEvaluatable
    {
        /// <summary>
        /// Вычисляет значение данного объекта с использованием переданного контекста.
        /// </summary>
        /// <param name="ctx">Контекст вычисления.</param>
        /// <returns>Результат вычисления.</returns>
        ValueBase Evaluate(IEvaluationContext ctx);
    }
}
