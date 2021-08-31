using System.Collections.Generic;
using System.Data;
using MonoGameHerex.src.view;
using SharpDX;
using Point = Microsoft.Xna.Framework.Point;

namespace MonoGameHerex.src.model
{
    public class Map
    {
        public static Point gridCount = new Point(20, 17);
        public TileType[,] mapLayout;
        public List<Tile> tiles;
        public int enemyCount;
        public int coinCount;
        public int mapLvl;

        public List<Enemy> enemies;

        public Map()
        {
            mapLayout = new TileType[gridCount.Y, gridCount.X];   // Map is 20 Tiles wide and 17 Tiles high
            tiles = new List<Tile>();
            setDefaultMap();
            enemies = new List<Enemy>();
            Exit.isOpen = false;
        }

        private void setDefaultMap()
        {
            for (int i = 0; i < mapLayout.GetLength(0); i++)
            {
                for (int j = 0; j < mapLayout.GetLength(1); j++)
                {
                    mapLayout[i, j] = TileType.Ground;
                }
            }
            mapLayout[10, 5] = TileType.Air;
        }

        public void addTiles()
        {
            coinCount = 0;
            for (int i = 0; i < mapLayout.GetLength(0); i++)
            {
                for (int j = 0; j < mapLayout.GetLength(1); j++)
                {
                    tiles.Add(new Tile(new Point(j * GameScreen.GridSize, i * GameScreen.GridSize), mapLayout[i, j]));
                    if (mapLayout[i, j] == TileType.Coin)
                    {
                        coinCount++;
                    }

                    if (mapLayout[i, j] == TileType.Enemy)
                    {
                        enemyCount++;
                        Enemy enemy = new Enemy(new Vector2(j, i + 1), this);
                        enemies.Add(enemy);
                    }
                }
            }
        }
    }
}