using Flappy.Core.Core;

namespace Flappy.Core.Entities
{
    public class Pipe
    {
        public float PositionX { get; set; }
        public float Width { get; set; }

        public float GapYMin { get; }
        public float GapYMax { get; }
        public bool Passed { get; private set; }

        public Pipe(float gapYMin, float gapYMax, float positionX, float width)
        {
            GapYMin = gapYMin;
            GapYMax = gapYMax;
            PositionX = positionX;
            Width = width;
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
