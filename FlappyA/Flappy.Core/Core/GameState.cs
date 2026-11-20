using Flappy.Core.Core.Abstract;
using Flappy.Core.Entities;

namespace Flappy.Core.Core
{
    public sealed class GameState
    {
        private readonly IGameConfiguration _config;


        public uint Score { get; set; }
        public bool IsStarted { get; set; }
        public bool IsOver { get; set; }
        public bool Flapped { get; set; }
        public float FlapInertia { get; set; }

        #region Difficulty

        public ushort CurrentPipeSpawnGap { get; internal set; }
        public byte CurrentPipeGateGap { get; internal set; }
        public ulong NextDifficultyScaleAtScore { get; internal set; }

        #endregion

        public Player Player { get; private set; }
        public IReadOnlyList<Pipe> Pipes { get; set; } = new List<Pipe>();

        public GameState(IGameConfiguration config)
        {
            _config = config;
            Player = new Player(0, 0, config.PlayerStartX, _config.PlayerStartY, _config.PlayerWidth);
        }

        public void Reset()
        {
            Score = 0;
            IsStarted = false;
            IsOver = false;
            Flapped = false;
            Player = new Player(0, 0, _config.PlayerStartX, _config.PlayerStartY, _config.PlayerWidth);
            Pipes = new List<Pipe>();

            FlapInertia = _config.BaseFlapInertia;
            CurrentPipeGateGap = 0;
            CurrentPipeSpawnGap = 0;
            NextDifficultyScaleAtScore = 0;
        }
    }
}
