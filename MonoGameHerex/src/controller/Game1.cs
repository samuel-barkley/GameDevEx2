using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Mime;
using System.Windows.Forms.VisualStyles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameHerex.Handlers;
using MonoGameHerex.src.model;
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

        private Player player;
        
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
            Texture2D doorClosed = Content.Load<Texture2D>("Textures/map/DoorClosed");
            Texture2D doorOpen = Content.Load<Texture2D>("Textures/map/DoorOpen");
            Texture2D player_idle = Content.Load<Texture2D>("Textures/Characters/Player/Idle");
            Texture2D player_jump = Content.Load<Texture2D>("Textures/Characters/Player/Jump");
            Texture2D player_walk0 = Content.Load<Texture2D>("Textures/Characters/Player/Walk0");
            Texture2D player_walk1 = Content.Load<Texture2D>("Textures/Characters/Player/Walk1");
            Texture2D player_walk2 = Content.Load<Texture2D>("Textures/Characters/Player/Walk2");
            
            
            textures = new Dictionary<string, Texture2D>();
            textures.Add("brick", brick);
            textures.Add("doorClosed", doorClosed);
            textures.Add("doorOpen", doorOpen);
            textures.Add("player_idle", player_idle);
            textures.Add("player_jump", player_jump);
            textures.Add("player_walk_0", player_walk0);
            textures.Add("player_walk_1", player_walk1);
            textures.Add("player_walk_2", player_walk2);
            
            List<List<string>> mapData = LoadMaps();
            foreach (var v in views)
            {
                v.AddTextures(textures);
                v.AddLvlData(mapData);
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

            if (views[0].IsActive)
            {
                UpdateGameplay();
            }
            else if (views[1].IsActive)
            {
                UpdateMenu();
            }
            
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

        private List<List<string>> LoadMaps()
        {
            // Todo: Load in all maps, not just one.
            List<List<string>> mapGrids = new List<List<string>>();
            string mapPath = "Content/MapData/map0.txt" /*string.Format("Content/levels/{0}.txt", 0)*/;

            mapGrids = new List<List<string>>();
            
            using (Stream fileStream = TitleContainer.OpenStream(mapPath))
            {
                mapGrids.Add(new List<string>()); // Adding the first map
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    string line = reader.ReadLine();
                    
                    while (line != null)
                    {
                        mapGrids[0].Add(line);      // Index = 0 because currently only loads in one map.
                        line = reader.ReadLine();
                    }
                }
            }

            return mapGrids;
        }

        private void UpdateGameplay()
        {
            if (player == null)
            {
                player = new Player();
                views[0].AddPlayer(player);
            }
            
            player.Update(_state, _prevState);
        }

        private void UpdateMenu()
        {
            
        }
    }
}
