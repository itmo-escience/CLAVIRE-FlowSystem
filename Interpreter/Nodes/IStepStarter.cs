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

        bool IsDry { get; set; }
        void SetEventConsumer(IEventConsumer eventConsumer);

    }

}