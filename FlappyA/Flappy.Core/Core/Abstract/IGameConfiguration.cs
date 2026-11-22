namespace Flappy.Core.Core.Abstract
{
    public interface IGameConfiguration
    {
        public ushort BoundaryX { get; }
        public ushort BoundaryY { get; }

        public float PlayerWidth { get; }
        public float PlayerStartX { get; }
        public float PlayerStartY { get; }

        public float PipeWidth { get; }
        public float BasePipeVelocity { get; }
        public float MaxPipeVelocity { get; }
        public ushort BasePipeSpawnGap { get; }
        public byte BasePipeGateGap { get; }
        public byte DifficultyScaleScoreInterval { get; }

        public float FlapVelocity { get; }
        public float GravityVelocity { get; }
        public ulong FlapThrottle { get; }
    }
}
