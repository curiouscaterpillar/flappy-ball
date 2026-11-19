using System;
using System.Collections.Generic;
using System.Text;

namespace Flappy.Core.Core.Abstract
{
    public interface IInputProvider
    {
        ControlScheme? RequestInput();
    }
}
