using Flappy.Core.Core.Abstract;
using System.Numerics;

namespace Flappy.Core.Entities
{
    public class Player : GameObject
    {
        public Player(Vector2 position, Vector2 velocity, float width)
            :base(position, velocity, width)
        { }
    }
}
