namespace Easis.Wfs.EasyFlow.Model
{
    /// <summary>
    /// ��������� ������������
    /// </summary>
    public interface IValueSetterEcx
    {
        void SetValue(CompoundVarIdentifier identifier, ValueBase value,IEvaluationContext evaluationContext);
    }
}