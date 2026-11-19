using Flappy.Core.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flappy.Core
{
    public interface ISystem
    {
        void OnUpdate(GameState state);
    }
}
