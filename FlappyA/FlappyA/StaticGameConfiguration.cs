using Flappy.Core.Core.Abstract;

namespace FlappyA
{
    internal class StaticGameConfiguration : IGameConfiguration
    {
        public ushort BoundaryX => 40;
        public ushort BoundaryY => 15;
        public ushort PlayerStartX => 10;
        public ushort PlayerStartY => 8;

        // Pipe Spawn Settings
        public float BasePipeVelocity => 9f;
        public float MaxPipeVelocity => 20f;
        public ushort BasePipeSpawnGap => 15;
        public byte BasePipeGateGap => 3;
        public byte DifficultyScaleScoreInterval => 5;
        public byte IncreaseSpawnAfter => 10;
        public byte ReducePipeGapSizeAfter => 20;

        // Flap Settings
        public float GravityVelocity => 40f;
        public float FlapVelocity => 15f;
        public float BaseFlapInertia => 5;
        public ulong FlapThrottle => 3;
    }
}
