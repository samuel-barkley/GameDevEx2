using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using Apos.Gui;
using FontStashSharp;
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

        public Player player;
        private Map map;
        private MainMenu mainMenu;
        private int currentLvl;
        public int Points;

        private bool updateGamePlay = true;
        
        private IMGUI _ui; // Used for UI

        private Dictionary<string, Texture2D> textures;
        
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _ui = new IMGUI();
            views = new List<IScreen> {new GameScreen(_graphics, _ui), new MainMenuScreen(_graphics, _ui)};
            _switchScreenHelper = new SwitchScreenHelper(views);
            map = new Map();
            mainMenu = new MainMenu(this, _ui, _graphics);
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            FontSystem fontSystem = FontSystemFactory.Create(GraphicsDevice, 2048, 2048);
            fontSystem.AddFont(TitleContainer.OpenStream($"{Content.RootDirectory}/font-file.ttf"));
            GuiHelper.Setup(this, fontSystem);
            
            
            Texture2D brick = Content.Load<Texture2D>("Textures/Map/mario_brick");
            Texture2D coin = Content.Load<Texture2D>("Textures/Map/mario_coin");
            Texture2D doorClosed = Content.Load<Texture2D>("Textures/map/DoorClosed");
            Texture2D doorOpen = Content.Load<Texture2D>("Textures/map/DoorOpen");
            Texture2D player_idle = Content.Load<Texture2D>("Textures/Characters/Player/Idle");
            Texture2D player_jump = Content.Load<Texture2D>("Textures/Characters/Player/Jump");
            Texture2D player_walk0 = Content.Load<Texture2D>("Textures/Characters/Player/Walk0");
            Texture2D player_walk1 = Content.Load<Texture2D>("Textures/Characters/Player/Walk1");
            Texture2D player_walk2 = Content.Load<Texture2D>("Textures/Characters/Player/Walk2");
            Texture2D goomba = Content.Load<Texture2D>("Textures/Characters/Enemy/Goomba");
            
            textures = new Dictionary<string, Texture2D>();
            textures.Add("brick", brick);
            textures.Add("coin", coin);
            textures.Add("doorClosed", doorClosed);
            textures.Add("doorOpen", doorOpen);
            textures.Add("player_idle", player_idle);
            textures.Add("player_jump", player_jump);
            textures.Add("player_walk_0", player_walk0);
            textures.Add("player_walk_1", player_walk1);
            textures.Add("player_walk_2", player_walk2);
            textures.Add("goomba", goomba);

            LoadMaps();
        }
        
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Poll for current keyboard state.
            _prevState = _state;
            _state = Keyboard.GetState();

            if (views[0].IsActive)
            {
                UpdateGameplay(gameTime);
            }
            else if (views[1].IsActive)
            {
                UpdateMenu(gameTime);
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
                    v.Draw(_spriteBatch, gameTime);
                }
            }
            _spriteBatch.End();
            
            base.Draw(gameTime);
        }

        public void startGame()
        {
            map = new Map();
            Points = 0;
            updateGamePlay = true;
            LoadMaps();
            if (player != null)
                player.Reset(map);
            _switchScreenHelper.SetView(0);
        }

        private void LoadMaps()
        {
            // Todo: Load in all maps, not just one.
            List<List<string>> mapGrids = new List<List<string>>();
            string[] mapPaths = {"Content/MapData/map0.txt", "Content/MapData/map1.txt"};
            
            mapGrids = new List<List<string>>();

            int i = 0;
            foreach (var path in mapPaths)
            {
                using (Stream fileStream = TitleContainer.OpenStream(path))
                {
                    mapGrids.Add(new List<string>()); // Adding the first map
                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        string line = reader.ReadLine();
                    
                        while (line != null)
                        {
                            mapGrids[i].Add(line);      // Index = 0 because currently only loads in one map.
                            line = reader.ReadLine();
                        }
                    }
                }

                i++;
            }
            

            foreach (var v in views)
            {
                map.mapLvl = currentLvl % 2;
                v.AddTextures(textures);
                v.AddLvlData(mapGrids, map);
            }
        }

        private void UpdateGameplay(GameTime gameTime)
        {
            if (updateGamePlay)
            {
                if (player == null || player.MadeIt)
                {
                    player = new Player(map);
                    views[0].AddPlayer(player);
                }

                if (map.enemyCount != 0)
                {
                    foreach (var enemy in map.enemies)
                    {
                        enemy.Update(gameTime, _state, _prevState);
                    }
                }

                player.Update(gameTime, _state, _prevState);
                if (!player.Alive)
                {
                    updateGamePlay = false;
                    currentLvl = 0;
                    _switchScreenHelper.SetView(1);
                }
                CheckEndState();
            }
        }

        private void CheckEndState()
        {
            if (player.MadeIt)
            {
                currentLvl++;
                Debug.WriteLine("Made it");
                Points += player.points;
                map = new Map();
                LoadMaps();
            }
        }

        private void UpdateMenu(GameTime gameTime)
        {
            mainMenu.Update(gameTime);
        }
    }
}
