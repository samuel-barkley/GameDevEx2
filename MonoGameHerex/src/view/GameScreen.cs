using System;
using System.Collections.Generic;
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
        public bool IsActive { get; set; } = true;
        private GraphicsDeviceManager _graphics;
        private Dictionary<string, Texture2D> _textures;
        private int gridSize;
        private int _currentLvl;
        private List<List<string>> _mapDataString;
        private Map _map;
        private Character _player;

        public GameScreen(GraphicsDeviceManager graphics)
        {
            _graphics = graphics;
            gridSize = GraphicsDeviceManager.DefaultBackBufferHeight / Map.gridCount.Y;
            GridSize = gridSize;
            SetLvl(0);
        }

        /*
         * Draws everything relating to the actual gameplay screen. (all levels)
         */
        public void Draw(SpriteBatch _spriteBatch)
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
                        case TileType.End:
                            _spriteBatch.Draw(_textures["doorClosed"], new Rectangle(gridSize * j, gridSize * (i - 1), gridSize, gridSize * 2), Color.White);
                            break;
                    }
                    
                }
            }
            
            // Draw Player
            if (_player != null)
                _spriteBatch.Draw(_textures["player_idle"], new Rectangle((int) (_player.Pos.X * gridSize - gridSize / 2), (int) (_player.Pos.Y * gridSize - gridSize), gridSize, gridSize), Color.White);
            
        }

        public void AddTextures(Dictionary<string, Texture2D> textures)
        {
            _textures = textures;
        }

        public void AddLvlData(List<List<string>> mapData, Map map)
        {
            _mapDataString = mapData;
            _map = DeserialiseMapHelper.DeserialiseMap(_mapDataString[0], map); // TODO: Currently only one map load in.
            _map.addTiles(); // Notifies map to make list of tiles for collision detection.
        }

        public void SetLvl(int id)
        {
            _currentLvl = id;
            //_map = new Map(); // Todo: no need to instantiate new map here I think.
        }

        public void AddPlayer(Character player)
        {
            _player = player;
        }
    }
}