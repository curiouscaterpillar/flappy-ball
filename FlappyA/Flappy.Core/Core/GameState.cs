using Flappy.Core.Core.Abstract;
using Flappy.Core.Entities;
using System.Numerics;

namespace Flappy.Core.Core
{
    public sealed class GameState
    {
        private byte _tailLength;
        private readonly IGameConfiguration _config;
        private Queue<Vector2> _playerTailQueue;

        public uint Score { get; set; }
        public bool IsStarted { get; set; }
        public bool IsOver { get; set; }
        public bool Flapped { get; set; }

        #region Difficulty

        public ushort CurrentPipeSpawnGap { get; internal set; }
        public byte CurrentPipeGateGap { get; internal set; }
        public ulong NextDifficultyScaleAtScore { get; internal set; }

        #endregion
        public Player Player { get; private set; }
        public IReadOnlyList<Pipe> Pipes { get; set; } = new List<Pipe>();
        public Queue<Vector2> PlayerTail => _playerTailQueue;

        public GameState(IGameConfiguration config)
        {
            _config = config;
            _playerTailQueue = GetInitialQueueState();
            _tailLength = (byte)(config.PlayerStartX / 0.75f);

            Player = CreatePlayerAtStart();
        }

        public void Reset()
        {
            Score = 0;
            IsStarted = false;
            IsOver = false;
            Flapped = false;
            Pipes = new List<Pipe>();
            _playerTailQueue = GetInitialQueueState();

            CurrentPipeGateGap = 0;
            CurrentPipeSpawnGap = 0;
            NextDifficultyScaleAtScore = 0;

            Player = CreatePlayerAtStart();
        }

        public void RecordPlayerLocation()
        {
            // Record location
            if (_playerTailQueue.Count < _tailLength)
            {
                var tailCandidateVector = new Vector2(
                     (float)Math.Ceiling(Player.Position.X - 1f),
                     (float)Math.Ceiling(Player.Position.Y));
                _playerTailQueue.Enqueue(tailCandidateVector);

                return;
            }

            // Make room if queue is full.
            _playerTailQueue.Dequeue();
        }

        private Queue<Vector2> GetInitialQueueState()
        {
            var q = new Queue<Vector2>();

            for (int i = 0; i < _tailLength; i++)
                q.Enqueue(Player.Position);

            return q;
        }

        private Player CreatePlayerAtStart()
        {
            return new Player(
                new Vector2(_config.PlayerStartX, _config.PlayerStartY),
                new Vector2(0, 0), _config.PlayerWidth);
        }
    }
}
