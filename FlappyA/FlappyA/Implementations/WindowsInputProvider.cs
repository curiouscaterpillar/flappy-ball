using Flappy.Core.Core;
using Flappy.Core.Core.Abstract;

namespace FlappyA.Implementations
{
    internal class WindowsInputProvider : IInputProvider
    {
        public bool ActionButtonPressed()
        {
            if(Console.KeyAvailable)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Spacebar)
                {
                    return true;
                }
            }

            return false;
        }

        public ControlScheme? RequestInput()
        {
            if(Console.KeyAvailable)
            {
                var key = Console.ReadKey(true);
                return ControlScheme.Flap;
            }

            return null;
        }
    }
}
