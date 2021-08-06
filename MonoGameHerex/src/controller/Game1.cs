using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameHerex.Handlers;
using MonoGameHerex.src.view;

namespace MonoGameHerex
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        
        private List<IScreen> views;
        private List<IController> controllers;

        private SwitchScreenHelper _switchScreenHelper;

        private KeyboardState _state;
        private KeyboardState _prevState;

        private Texture2D blockTile;
        
        private Dictionary<string, Texture2D> textures;
        
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            views = new List<IScreen> {new GameScreen(_graphics), new MainMenu(_graphics)};
            _switchScreenHelper = new SwitchScreenHelper(views); 
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Texture2D brick = Content.Load<Texture2D>("Textures/Map/mario_brick");

            textures = new Dictionary<string, Texture2D>();
            textures.Add("brick", brick);

            foreach (var v in views)
            {
                v.AddTextures(textures);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            foreach (var v in views)
            {
                v.Update();     // TODO: Not sure if views should be "updated" or just drawn. Maybe remove later.
            }

            // Poll for current keyboard state.
            _prevState = _state;
            _state = Keyboard.GetState();
            
            #region Manually switch screen with I.
            if (_state.IsKeyDown(Keys.I) && !_prevState.IsKeyDown(Keys.I))  // TODO: Remove when unnecessary.
            {
                if (views[0].IsActive)
                {
                    _switchScreenHelper.SetView(1);
                }
                else if (views[1].IsActive)
                {
                    _switchScreenHelper.SetView(0);
                }
            }
            #endregion

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            _spriteBatch.Begin();
            foreach (var v in views)
            {
                if (v.IsActive)
                {
                    v.Draw(_spriteBatch);
                }
            }
            _spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
