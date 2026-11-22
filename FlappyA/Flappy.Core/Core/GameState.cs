using Flappy.Core.Core.Abstract;
using Flappy.Core.Entities;

namespace Flappy.Core.Core
{
    public sealed class GameState
    {
        private byte _tailLength;
        private readonly IGameConfiguration _config;
        private Queue<Point> _playerTailQueue;

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
        public Queue<Point> PlayerTail => _playerTailQueue;

        public GameState(IGameConfiguration config)
        {
            _config = config;
            Player = new Player(0, 0, config.PlayerStartX, _config.PlayerStartY, _config.PlayerWidth);
            _playerTailQueue = GetInitialQueueState();
            _tailLength = (byte)(config.PlayerStartX / 0.75f);
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
            _playerTailQueue = GetInitialQueueState();
        }

        public void RecordPlayerLocation()
        {
            // Record location
            if (_playerTailQueue.Count < _tailLength)
            {
                _playerTailQueue.Enqueue(new Point(
                    (float)Math.Ceiling(Player.PositionX - 1f),
                    (float)Math.Ceiling(Player.PositionY))
                );

                return;
            }

            // Make room if queue is full.
            _playerTailQueue.Dequeue();
        }

        private Queue<Point> GetInitialQueueState()
        {
            var playerPos = new Point(Player.PositionX, Player.PositionY);
            var q = new Queue<Point>();

            for (int i = 0; i < _tailLength; i++)
                q.Enqueue(playerPos);

            return q;
        }
    }
}
