using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGameHerex
{
    public abstract class Character : IController
    {
        protected Vector2 pos;
        private Vector2 vel;
        private Rectangle bounds;

        public Vector2 Pos
        {
            get { return pos; }
            private set
            {
                pos = Pos;
            }
        }

        public Character()
        {
            
        }
        
        public virtual void Update(GameTime gameTime, KeyboardState state, KeyboardState prevState)
        {
            
        }
    }
}