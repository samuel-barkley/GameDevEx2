using Microsoft.Xna.Framework;

namespace MonoGameHerex.src.model
{
    public enum TileType { Coin, Enemy, Start, End, Ground}
    
    public class Tile
    {
        private TileType _type;
        private Point _gridPos;
        private Point _absPos;
        
        public TileType Type => _type;
        public Point GridPos => _gridPos;
        
    }
}