using System.Diagnostics;
using Apos.Gui;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameHerex
{
    // Controls the UI and events of the Main Menu Screen.
    public class MainMenu : IController
    {
        private IMGUI ui;
        private Game1 gameInstance;  // this is needed if you want to shut down the game from within the main menu.
        private GraphicsDeviceManager graphics;

        private bool showFun;

        public MainMenu(Game1 game1, IMGUI _ui, GraphicsDeviceManager _graphics)
        {
            ui = _ui;
            gameInstance = game1;
            graphics = _graphics;
        }
        
        public void Update(GameTime gameTime)
        {
            GuiHelper.UpdateSetup();
            ui.UpdateAll(gameTime);

            // Create your UI.
            Panel panel = Panel.Push();
            panel.XY = new Vector2(graphics.PreferredBackBufferWidth / 2.0f - panel.FullWidth / 2.0f, graphics.PreferredBackBufferHeight / 2.0f - panel.FullHeight / 2.0f);
            if (Button.Put("Start Game").Clicked)
            {
                gameInstance.startGame();
            }
            if (Button.Put("Quit").Clicked) {
                gameInstance.Exit();
            }
            Panel.Pop();

            // Call UpdateCleanup at the end.
            GuiHelper.UpdateCleanup();
        }
    }
}