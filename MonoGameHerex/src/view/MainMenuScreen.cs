using System.Collections.Generic;
using Apos.Gui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameHerex.src.view
{
    public class MainMenuScreen : IScreen
    {
        public bool IsActive { get; set; } = true;
        private GraphicsDeviceManager _graphics;

        private IMGUI ui;

        public MainMenuScreen(GraphicsDeviceManager graphics, IMGUI _ui)
        {
            _graphics = graphics;
            ui = _ui;
        }

        public void Draw(SpriteBatch _spriteBatch, GameTime gameTime)
        {
            //_graphics.GraphicsDevice.Clear(Color.Black);
            
            if (ui != null)
                ui.Draw(gameTime);
        }

        public void AddTextures(Dictionary<string, Texture2D> textures)
        {
            
        }
        public void AddLvlData(List<List<string>> mapData)
        {
            
        }
    }
}