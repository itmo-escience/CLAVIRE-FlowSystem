namespace Easis.Wfs.Interpreting
{
    public interface IControllable
    {
        void Pause();
        void Resume();
        void Stop();
    }

    public interface IController
    {
        void Pause(long blockId);
        void Resume(long blockId);
        void Stop(long blockId);
    }
}