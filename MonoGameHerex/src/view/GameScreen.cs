using System;
using System.Collections.Generic;
using Apos.Gui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameHerex.Handlers;
using MonoGameHerex.src.model;
using MonoGame.Extended;

namespace MonoGameHerex.src.view
{
    public class GameScreen : IScreen
    {
        public static int GridSize;
        public bool IsActive { get; set; } = false;
        private GraphicsDeviceManager _graphics;
        private Dictionary<string, Texture2D> _textures;
        private int gridSize;
        private int _currentLvl;
        private List<List<string>> _mapDataString;
        private Map _map;
        private Character _player;

        // Makes the coins smaller than a "gridSize". 
        private int coinOffset = 3;

        private IMGUI ui;

        public GameScreen(GraphicsDeviceManager graphics, IMGUI _ui)
        {
            _graphics = graphics;
            gridSize = GraphicsDeviceManager.DefaultBackBufferHeight / Map.gridCount.Y;
            GridSize = gridSize;
            SetLvl(0);

            ui = _ui;
        }

        /*
         * Draws everything relating to the actual gameplay screen. (all levels)
         */
        public void Draw(SpriteBatch _spriteBatch, GameTime gameTime)
        {
            _graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            
            // Drawing Map
            for (int i = 0; i < _map.mapLayout.GetLength(0); i++)
            {
                for (int j = 0; j < _map.mapLayout.GetLength(1); j++)
                {
                    switch (_map.mapLayout[i, j])
                    {
                        case TileType.Air:
                            break;
                        case TileType.Ground:
                            _spriteBatch.Draw(_textures["brick"], new Rectangle(gridSize * j, gridSize * i, gridSize, gridSize), Color.White);
                            break;
                        case TileType.Coin:
                            _spriteBatch.Draw(_textures["coin"], new Rectangle(gridSize * j + coinOffset, gridSize * i + coinOffset, gridSize - coinOffset * 2, gridSize - coinOffset * 2), Color.White);
                            break;
                        case TileType.End:
                            if (Exit.isOpen)
                            {
                                _spriteBatch.Draw(_textures["doorOpen"], new Rectangle(gridSize * j, gridSize * (i - 1), gridSize, gridSize * 2), Color.White);
                            }
                            else
                            {
                                _spriteBatch.Draw(_textures["doorClosed"], new Rectangle(gridSize * j, gridSize * (i - 1), gridSize, gridSize * 2), Color.White);
                            }
                            
                            break;
                    }
                }
            }
            
            // Draw Player
            if (_player != null)
                _spriteBatch.Draw(_textures["player_idle"], new Rectangle((int) (_player.Pos.X * gridSize - gridSize / 2.0f), (int) (_player.Pos.Y * gridSize - gridSize), gridSize, gridSize), Color.White);
            
            // Draw Enemies
            if (_map.enemies.Count != 0)
            {
                foreach (var enemy in _map.enemies)
                {
                    _spriteBatch.Draw(_textures["goomba"], new Rectangle((int) (enemy.Pos.X * gridSize - gridSize / 2.0f), (int) (enemy.Pos.Y * gridSize - gridSize), GridSize, GridSize), Color.White);
                }
            }
            
            DrawGrid(_spriteBatch);
        }

        public void AddTextures(Dictionary<string, Texture2D> textures)
        {
            _textures = textures;
        }

        public void AddLvlData(List<List<string>> mapData, Map map)
        {
            _mapDataString = mapData;
            _map = DeserialiseMapHelper.DeserialiseMap(_mapDataString[map.mapLvl], map);
            _map.addTiles(); // Notifies map to make list of tiles for collision detection.
        }

        public void UpdatelvlData(Map map)
        {
            _map = map; // Todo: remove if not necessary
        }

        public void SetLvl(int id)
        {
            _currentLvl = id;
        }

        public void AddPlayer(Character player)
        {
            _player = player;
        }

        private void DrawGrid(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _map.mapLayout.GetLength(0); i++)
            {
                for (int j = 0; j < _map.mapLayout.GetLength(1); j++)
                {
                    spriteBatch.DrawRectangle(new Rectangle(j * GridSize, i * GridSize, GridSize, GridSize), Color.Black);
                }
            }
        }
    }
}