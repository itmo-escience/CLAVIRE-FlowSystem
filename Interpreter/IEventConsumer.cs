namespace Easis.Wfs.Interpreting
{
    public interface IEventConsumer
    {
        void PushEvent(FlowEvent flowEvent);
    }
}