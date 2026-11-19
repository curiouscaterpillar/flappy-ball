using FlappyA;
using Flappy.Core;
using Flappy.Core.Core;
using Flappy.Core.Renderer;
using FlappyA.Implementations;
using FlappyA.Implementations.Renderer;
using Flappy.Core.Core.Abstract;

#region Register dependencies

IDependencyResolver resolver = new LocalDependencyResolver();

resolver.AddSingleton<IGameConfiguration, StaticGameConfiguration>(new StaticGameConfiguration());
resolver.AddSingleton<GameState>();
resolver.AddSingleton<IInputProvider, WindowsInputProvider>();
resolver.AddSingleton<IRenderer, ConsoleRenderer>();
resolver.AddSingleton<Game>();
resolver.AddSingleton<IFramerateMonitor, FramerateMonitor>();

#endregion 

var game = resolver.Resolve<Game>();
var renderer = resolver.Resolve<IRenderer>();
var framerate = resolver.Resolve<IFramerateMonitor>();

framerate.Watch();

// Run
while (true)
{

    game.Update();
    await renderer.RequestFrameAsync(game.State);

    framerate.Frame();
}