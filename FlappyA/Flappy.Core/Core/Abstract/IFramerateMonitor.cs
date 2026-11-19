namespace Flappy.Core.Core.Abstract
{
    public record FramerateSnapshot(long elapsedMs, int frameCount, short framerate);
    public interface IFramerateMonitor
    {
        void Watch();
        void Stop();
        void Frame();
        FramerateSnapshot Reset();
        FramerateSnapshot GetSnapshot();
    }
}
