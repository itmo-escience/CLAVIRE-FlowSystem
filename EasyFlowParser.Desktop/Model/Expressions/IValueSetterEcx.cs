namespace Easis.Wfs.EasyFlow.Model
{
    /// <summary>
    /// Интерфейс присваивания
    /// </summary>
    public interface IValueSetterEcx
    {
        void SetValue(CompoundVarIdentifier identifier, ValueBase value,IEvaluationContext evaluationContext);
    }
}