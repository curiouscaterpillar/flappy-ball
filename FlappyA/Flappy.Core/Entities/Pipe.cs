using Flappy.Core.Core;
using Flappy.Core.Core.Abstract;
using System.Numerics;

namespace Flappy.Core.Entities
{
    public class Pipe : GameObject
    {
        public float GapYMin { get; }
        public float GapYMax { get; }
        public bool Passed { get; private set; }

        public Pipe(float gapYMin, float gapYMax, Vector2 position, float width)
        {
            GapYMin = gapYMin;
            GapYMax = gapYMax;
            Position = position;
            Width = width;
        }

        public Vector2 MoveLeft(float velocity)
        {
            Position = new Vector2(
                Position.X - (float)Math.Round(velocity * Time.DeltaTime, 2),
                Position.Y
            );

            return Position;
        }

        public void MarkPassed()
        {
            Passed = true;
        }
    }
}
