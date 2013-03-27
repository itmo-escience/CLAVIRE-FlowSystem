namespace Easis.Wfs.Interpreting
{
    public interface IStepStarter
    {
        //TODO: ExecutionSettings ������ ������ �� Starter
        /// <summary>
        /// ������� �� ������ ���� WF �� �����������.
        /// �� �������� � ���� �������� ��������� ����������.
        /// </summary>
        /// <param name="stepRunDescriptor">���������� ����</param>
        void StartStep(StepRunDescriptor stepRunDescriptor);

        bool IsDry { get; }
        void SetEventConsumer(IEventConsumer eventConsumer);

    }

}