using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameHerex.src.view;

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

        protected Vector2 absPos
        {
            get
            {
                return pos * GameScreen.GridSize;
            }
            set
            {
                pos = value / GameScreen.GridSize;
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