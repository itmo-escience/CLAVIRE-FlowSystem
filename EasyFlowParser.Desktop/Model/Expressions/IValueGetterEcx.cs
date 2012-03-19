namespace Easis.Wfs.EasyFlow.Model
{
    public interface IValueGetterEcx
    {
        /// <summary>
        /// Интерфейс получения значения.
        /// Соглашение следующее. В метод приходит compound "A.B", где A - это this.
        /// throws InvalidOperationException
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="evaluationContext"></param>
        /// <returns></returns>
        ValueBase GetValue(CompoundVarIdentifier identifier, IEvaluationContext evaluationContext);
    }
}