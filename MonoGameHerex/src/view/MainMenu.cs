using System.Drawing;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Color = Microsoft.Xna.Framework.Color;

namespace MonoGameHerex.src.view
{
    public class MainMenu : IScreen
    {
        public bool IsActive { get; set; }
        private GraphicsDeviceManager _graphics;

        public MainMenu(GraphicsDeviceManager graphics)
        {
            _graphics = graphics;
        }

        public void Update()
        {
            // TODO: Possibly remove if not needed.
        }

        public void Draw()
        {
            _graphics.GraphicsDevice.Clear(Color.Green);
        }
    }
}