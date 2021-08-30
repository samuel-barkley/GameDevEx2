using System.Diagnostics;
using Apos.Gui;
using FontStashSharp;
using Microsoft.Xna.Framework;

namespace MonoGameHerex
{
    // Controls the UI and events of the Main Menu Screen.
    public class MainMenu : IController
    {
        private IMGUI ui;
        private Game gameInstance;  // this is needed if you want to shut down the game from within the main menu.

        private bool showFun;

        public MainMenu(Game game1, IMGUI _ui)
        {
            ui = _ui;
            gameInstance = game1;
        }
        
        public void Update(GameTime gameTime)
        {
            GuiHelper.UpdateSetup();
            ui.UpdateAll(gameTime);

            // Create your UI.
            Panel.Push();
            if (Button.Put("Show fun").Clicked) {
                showFun = !showFun;
            }
            if (showFun) {
                Label.Put("This is fun!");
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