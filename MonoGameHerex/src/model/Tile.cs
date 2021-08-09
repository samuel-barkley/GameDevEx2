using Microsoft.Xna.Framework;
using MonoGameHerex.src.view;

namespace MonoGameHerex.src.model
{
    public enum TileType { Coin, Enemy, Start, End, Ground, Air}
    
    public class Tile
    {
        private TileType _type;
        private Point _gridPos;
        private Rectangle collisionRect;
        
        public TileType Type => _type;
        public Point GridPos => _gridPos;
        public Rectangle CollisionRect => collisionRect;

        public Tile(Point pos, TileType type)
        {
            _gridPos = pos;
            _type = type;
            collisionRect = new Rectangle(pos.X, pos.Y, GameScreen.GridSize, GameScreen.GridSize);
        }
    }
}