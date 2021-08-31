using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Vector2 = SharpDX.Vector2;

namespace MonoGameHerex
{
    public class Enemy : Character
    {
        public Enemy(Vector2 startingPos)
        {
            pos.X = startingPos.X;
            pos.Y = startingPos.Y;
        }

        public override void Update(GameTime gameTime, KeyboardState state, KeyboardState prevState)
        {
            
        }
    }
}