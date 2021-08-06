using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameHerex.src.model;

namespace MonoGameHerex.src.view
{
    public class GameScreen : IScreen
    {
        public bool IsActive { get; set; } = true;
        private GraphicsDeviceManager _graphics;
        private Dictionary<string, Texture2D> _textures;
        private int gridSize;
        private int _currentLvl;
        private Map _map;

        public GameScreen(GraphicsDeviceManager graphics)
        {
            _graphics = graphics;
            gridSize = GraphicsDeviceManager.DefaultBackBufferHeight / Map.gridCount.Y;
            SetLvl(0);
        }

        public void Update()
        {
            // TODO: Remove if not needed.
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            _graphics.GraphicsDevice.Clear(Color.Red);
            //_spriteBatch.Draw(_textures["brick"], new Rectangle(20, 20, 20, 20), Color.White);
            
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
                    }
                    
                }
            }
            
        }

        public void AddTextures(Dictionary<string, Texture2D> textures)
        {
            _textures = textures;
        }

        public void SetLvl(int id)
        {
            _currentLvl = id;
            _map = new Map();
        }
    }
}