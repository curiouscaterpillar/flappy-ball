using Flappy.Core.Core;
using Flappy.Core.Core.Abstract;

namespace Flappy.Core
{
    public class Game
    {
        private readonly IInputProvider _inputProvider;
        private readonly IGameConfiguration _config;

        private readonly GameState _state;
        private readonly ObstaclePool _pipePool;

        private ulong _ticks = 0;
        private ulong _lastFlapAt = 0;

        public Game(IInputProvider inputProvider, IGameConfiguration config, GameState state)
        {
            _inputProvider = inputProvider;
            _state = state;
            _config = config;
            _pipePool = new ObstaclePool(state, config);

            InitDifficulty();
        }

        public GameState State { get => _state; }

        public void Update()
        {
            ControlScheme? input = _inputProvider.RequestInput();
            bool didFlap = input.HasValue && input.Value == ControlScheme.Flap;

            if (_state.IsOver)
            {
                if (didFlap)
                {
                    Restart();
                }

                return;
            }

            if (_state.IsStarted == false && didFlap)
            {
                _state.IsStarted = true;
            }

            if (_state.IsStarted == false)
                return;

            _state.Flapped = didFlap;

            AdjustDifficulty();

            ProcessPlayerMovement();

            _pipePool.Advance();
            _state.Pipes = _pipePool.Pipes;

            ProcessPipeCollisions();
            ProcessBoundaryCollisions();

            _ticks++;
        }

        public void Restart()
        {
            State.Reset();
            _pipePool.Reset();

            InitDifficulty();
        }

        private void ProcessPlayerMovement()
        {
            if (State.Flapped)
            {
                if ((_ticks - _lastFlapAt) >= _config.FlapThrottle)
                    State.Player.VelocityY = (-1f) * _config.FlapVelocity;
            }

            State.Player.PositionY = State.Player.PositionY + State.Player.VelocityY * Time.DeltaTime;
            State.Player.VelocityY += _config.GravityVelocity * Time.DeltaTime;
        }

        private void ProcessBoundaryCollisions()
        {
            float playerY = State.Player.PositionY;
            float playerX = State.Player.PositionX;

            // Player left bounds of game space?
            if (playerY <= 0 || playerY >= _config.BoundaryY)
            {
                State.IsOver = true;
            }
        }

        private void ProcessPipeCollisions()
        {
            float playerY = (float)Math.Round(State.Player.PositionY);
            float playerX = (float)Math.Round(State.Player.PositionX);

            for (int i = 0; i < _state.Pipes.Count; i++)
            {
                var pipe = _state.Pipes[i];

                float playerWidthUnits = State.Player.Width;
                float pipeWidthUnits = pipe.Width;
                float pipeX = (float)Math.Floor(pipe.PositionX);

                bool noCollision = pipe.Passed 
                    || ((playerX - playerWidthUnits + 1.5f) - (pipeX + pipeWidthUnits) < 0);

                if (noCollision)
                    continue;

                if (playerY > Math.Round(pipe.GapYMin) && playerY <= Math.Round(pipe.GapYMax))
                {
                    // Player scores
                    State.Score++;
                    pipe.MarkPassed();
                    return;
                }

                // Player collided with the pipe
                State.IsOver = true;
                return;
            }
        }

        private void AdjustDifficulty()
        {
            if (State.Score == State.NextDifficultyScaleAtScore)
            {
                if (State.CurrentPipeSpawnGap > 10)
                    State.CurrentPipeSpawnGap -= 1;

                State.NextDifficultyScaleAtScore = State.Score + _config.DifficultyScaleScoreInterval;
                _pipePool.BumpPipeVelocity();
            }
        }

        private void InitDifficulty()
        {
            State.NextDifficultyScaleAtScore = _config.DifficultyScaleScoreInterval;
            State.CurrentPipeGateGap = _config.BasePipeGateGap;
            State.CurrentPipeSpawnGap = _config.BasePipeSpawnGap;
        }
    }
}
