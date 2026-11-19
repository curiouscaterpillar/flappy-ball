using Flappy.Core.Core;
namespace Flappy.Core.Renderer
{
    public interface IRenderer
    {
        Task RequestFrameAsync(GameState state);
    }
}
