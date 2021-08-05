using Microsoft.Xna.Framework;

namespace MonoGameHerex.src.view
{
    public class GameScreen : IScreen
    {
        public bool IsActive { get; set; } = true;
        private GraphicsDeviceManager _graphics;

        public GameScreen(GraphicsDeviceManager graphics)
        {
            _graphics = graphics;
        }

        public void Update()
        {
            // TODO: Remove if not needed.
        }

        public void Draw()
        {
            _graphics.GraphicsDevice.Clear(Color.Red);
        }
    }
}