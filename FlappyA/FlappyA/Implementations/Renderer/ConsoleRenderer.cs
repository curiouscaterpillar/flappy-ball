using Flappy.Core.Core;
using Flappy.Core.Core.Abstract;
using Flappy.Core.Entities;
using Flappy.Core.Renderer;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace FlappyA.Implementations.Renderer
{
    internal class ConsoleRenderer : IRenderer
    {
        private readonly IGameConfiguration _config;
        private readonly IFramerateMonitor _ramerateMonitor;

        public ConsoleRenderer(IGameConfiguration config, IFramerateMonitor framerateMonitor)
        {
            _config = config;
            _ramerateMonitor = framerateMonitor;

            Console.CursorVisible = false;
            Console.OutputEncoding = Encoding.UTF8;
        }

        public Task RequestFrameAsync(GameState state)
        {
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = state.Flapped
                ? ConsoleColor.Yellow
                : ConsoleColor.Blue;

            if (state.IsStarted == false)
            {
                Console.WriteLine("Press Spacebar to begin.");
            }

            DrawGameState(ref state);
            DrawHUD(ref state);

            if (state.IsOver)
            {
                DrawGameOver();
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Black;
            }

            return Task.CompletedTask;
        }

        private void DrawHUD(ref GameState state)
        {
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("Framerate: {0}", _ramerateMonitor.GetSnapshot().framerate);
            Console.WriteLine("Score: {0:F2}", state.Score);
            Console.WriteLine("DeltaTime: {0:F2}", Time.DeltaTime);
            Console.WriteLine("Velocity: {0:F2}", state.Player.PositionY);
            Console.WriteLine("-------------------------------------------");
        }

        private void DrawGameOver()
        {
            Console.WriteLine("Yikes! Game Over");
            Console.BackgroundColor = ConsoleColor.DarkRed;
            return;
        }

        private void DrawGameState(ref GameState state)
        {
            for (byte y = 0; y < _config.BoundaryY; y++)
            {
                for (byte x = 0; x < _config.BoundaryX; x++)
                {
                    string nextChar = GetNextSymbol(x, y, ref state);
                    Console.Write(nextChar);
                }

                Console.WriteLine();
            }
        }

        private string GetNextSymbol(float x, float y, ref readonly GameState state)
        {
            string? next;

            next = GetBoundaryChar(x, y);
            next = next ?? GetPipeSymbol(x, y, in state);
            next = next ?? GetPlayerChar(x, y, in state);

            return next ?? "  ";
        }

        private string? GetBoundaryChar(float x, float y)
        {
            if (Round(y) == 0 || Round(y) == Round(_config.BoundaryY - 1))
            {
                return "--";
            }
            else if (Round(x) == 0 || Round(x) == Round(_config.BoundaryX - 1))
            {
                return "||";
            }

            return null;
        }

        private string? GetPipeSymbol(float x, float y, ref readonly GameState state)
        {
            for (int i = 0; i < state.Pipes.Count; i++)
            {
                Pipe pipe = state.Pipes[i];
                if (Round(x) == Round(pipe.PositionX + pipe.Width))
                {
                    StringBuilder sb = new StringBuilder();
                    for (int unit = 0; unit < pipe.Width; unit++)
                    {
                        // leading
                        if (unit == 0)
                        {
                            sb.Append('(');
                        }

                        if (unit == pipe.Width - 1)
                        {
                            sb.Append(')');
                        }
                    }

                    if (y <= pipe.GapYMin || y > pipe.GapYMax)
                    {
                        return sb.ToString();
                    }
                }

                continue;
            }

            return null;
        }

        private string? GetPlayerChar(float x, float y, ref readonly GameState state)
        {
            if (Round(x) == Round(state.Player.PositionX) && Round(y) == Round(state.Player.PositionY))
            {
                return "● ";
            }

            return null;
        }

        private float Round(float number) => (float)Math.Round(number, 0);
    }
}