namespace Flappy.Core.Entities
{
    public class Player
    {
        public float VelocityX { get; set; }
        public float VelocityY { get; set; }
        public float PositionY { get; set; }
        public float PositionX { get; set; }

        public Player(float velocityX, float velocityY, float positionX, float positionY)
        {
            VelocityX = velocityX;
            VelocityY = velocityY;
            PositionX = positionX;
            PositionY = positionY;
        }
    }
}
