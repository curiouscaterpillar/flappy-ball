using System.Numerics;

namespace Flappy.Core.Core.Abstract
{
    public abstract class GameObject
    {
        protected GameObject() { }

        protected GameObject(Vector2 pos, Vector2 vel, float width)
        {
            Position = pos;
            Velocity = vel;
            Width = width;
        }

        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Width { get; set; }
    }
}
