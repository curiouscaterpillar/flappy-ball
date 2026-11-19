using Flappy.Core.Core.Abstract;
using System.Diagnostics;

namespace Flappy.Core.Core
{
    public  class FramerateMonitor : IFramerateMonitor
    {
        private readonly Stopwatch _stopwatch;
        private int _frameCount;
        private long _msSinceLastFrame;

        public FramerateMonitor()
        {
            _stopwatch = new Stopwatch();
        }

        public void Watch()
        {
            _stopwatch.Start();
        }

        public void Stop()
        {
            _stopwatch.Stop();
        }

        public void Frame()
        {
            if (false == this._stopwatch.IsRunning)
            {
                return;
            }

            Time.DeltaTime = (_stopwatch.ElapsedMilliseconds - _msSinceLastFrame) / 1000f;
            _msSinceLastFrame = _stopwatch.ElapsedMilliseconds;

            _frameCount++;
        }

        public FramerateSnapshot GetSnapshot()
            => new FramerateSnapshot(
                _stopwatch.ElapsedMilliseconds, _frameCount, GetFramerate()
            );

        public FramerateSnapshot Reset()
        {
            FramerateSnapshot result = GetSnapshot();

            _stopwatch.Reset();

            return result;
        }

        private short GetFramerate()
        {
            if (false == _stopwatch.IsRunning)
            {
                return 0;
            }

            double elapsedSeconds = _stopwatch.Elapsed.TotalSeconds;

            return (short)Math.Ceiling(
                _frameCount / (elapsedSeconds == 0 ? 1 : elapsedSeconds)
            );
        }
    }
}
