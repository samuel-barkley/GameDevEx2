using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameHerex.src.view
{
    public interface IScreen
    {
        public bool IsActive { get; set; }
        
        public void Update()
        {
            // TODO: Remove if not needed.
        }
        public void Draw(SpriteBatch _spriteBatch)
        {
            
        }

        public void AddTextures(Dictionary<string, Texture2D> textures)
        {
            
        }

        public void AddLvlData(List<List<string>> mapData)
        {
            
        }

        public void AddPlayer(Character player)
        {
            
        }
    }
}