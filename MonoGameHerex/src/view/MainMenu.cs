using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;

namespace MonoGameHerex.src.view
{
    public class MainMenu : IScreen
    {
        public bool IsActive { get; set; } = false;
        private GraphicsDeviceManager _graphics;

        public MainMenu(GraphicsDeviceManager graphics)
        {
            _graphics = graphics;
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            _graphics.GraphicsDevice.Clear(Color.Green);
        }

        public void AddTextures(Dictionary<string, Texture2D> textures)
        {
            
        }
        public void AddLvlData(List<List<string>> mapData)
        {
            
        }
    }
}