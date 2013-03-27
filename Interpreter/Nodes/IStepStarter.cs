namespace Easis.Wfs.Interpreting
{
    public interface IStepStarter
    {
        //TODO: ExecutionSettings должны влиять на Starter
        /// <summary>
        /// Команда на запуск шага WF по дескриптору.
        /// Не включает в себя механизм получения результата.
        /// </summary>
        /// <param name="stepRunDescriptor">Дескриптор шага</param>
        void StartStep(StepRunDescriptor stepRunDescriptor);

        bool IsDry { get; }
        void SetEventConsumer(IEventConsumer eventConsumer);

    }

}