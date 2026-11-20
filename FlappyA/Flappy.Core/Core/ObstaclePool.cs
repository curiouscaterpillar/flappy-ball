using Flappy.Core.Core.Abstract;
using Flappy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Flappy.Core.Core
{
    internal class ObstaclePool
    {
        private readonly List<Pipe> _pipes = new();
        private readonly GameState _state;
        private readonly IGameConfiguration _config;
        private readonly Random _rng;
        private float _pipeVelocity;

        public ObstaclePool(GameState state, IGameConfiguration config)
        {
            _state = state;
            _config = config;
            _rng = new Random();
            _pipeVelocity = config.BasePipeVelocity;
        }

        public IReadOnlyList<Pipe> Pipes => _pipes.AsReadOnly();

        public void Advance()
        {
            if (_pipes.Count == 0 || _config.BoundaryX - _state.CurrentPipeSpawnGap > Math.Round(_pipes.Last().PositionX, 0))
            {
                SpawnPipe();
            }

            for (int i = 0; i < _pipes.Count; i++)
            {
                _pipes[i].MoveLeft(_pipeVelocity);
            }

            if (_pipes[0].PositionX == 0)
            {
                _pipes.RemoveAt(0);
            }
        }

        public float BumpPipeVelocity() 
        {
            if(_pipeVelocity < _config.MaxPipeVelocity)
                _pipeVelocity += 1f;

            return _pipeVelocity;
        }

        public void Reset()
        {
            _pipeVelocity = _config.BasePipeVelocity;
            _pipes.Clear();
        }

        private void SpawnPipe()
        {
            float gapYMax = (ushort)_rng.Next(3 + _state.CurrentPipeGateGap, _config.BoundaryY - 3);
            float gapYMin = (ushort)(gapYMax - _state.CurrentPipeGateGap);

            this._pipes.Add(new Pipe(gapYMin, gapYMax, _config.BoundaryX, _config.PipeWidth));
        }
    }
}
