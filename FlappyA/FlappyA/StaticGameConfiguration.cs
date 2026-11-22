using Flappy.Core.Core.Abstract;

namespace FlappyA
{
    internal class StaticGameConfiguration : IGameConfiguration
    {
        public ushort BoundaryX => 40;
        public ushort BoundaryY => 15;

        public float PlayerWidth => 2f;
        public float PlayerStartX => 10f;
        public float PlayerStartY => 8f;

        // Pipe Spawn Settings
        public float PipeWidth => 2f;
        public float BasePipeVelocity => 9f;
        public float MaxPipeVelocity => 20f;
        public ushort BasePipeSpawnGap => 15;
        public byte BasePipeGateGap => 3;
        public byte DifficultyScaleScoreInterval => 5;
        public byte IncreaseSpawnAfter => 10;
        public byte ReducePipeGapSizeAfter => 20;

        // Flap Settings
        public float GravityVelocity => 90f;
        public float FlapVelocity => 27f;
        public ulong FlapThrottle => 3;
    }
}
