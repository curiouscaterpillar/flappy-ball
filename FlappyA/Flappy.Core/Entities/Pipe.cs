using Flappy.Core.Core;

namespace Flappy.Core.Entities
{
    public class Pipe
    {
        private float _positionX = 0;

        public float PositionX
        {
            get
            {
                return _positionX;
            }
            set
            {
                _positionX = value;
            }
        }

        public float GapYMin { get; }
        public float GapYMax { get; }
        public bool Passed { get; private set; }

        public Pipe(float gapYMin, float gapYMax, float positionX)
        {
            GapYMin = gapYMin;
            GapYMax = gapYMax;
            PositionX = positionX;
        }

        public float MoveLeft(float velocity)
        {
            PositionX -= (float)Math.Round(velocity * Time.DeltaTime, 2);

            return PositionX;
        }

        public void MarkPassed()
        {
            Passed = true;
        }
    }
}
