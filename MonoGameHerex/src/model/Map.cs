namespace MonoGameHerex.src.model
{
    public class Map
    {
        public TileType[,] mapLayout;

        public Map()
        {
            mapLayout = new TileType[15, 15];
        }
    }
}