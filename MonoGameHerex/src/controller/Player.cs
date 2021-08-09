using System;
using System.Buffers.Text;
using System.Data;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;
using SharpDX;

namespace MonoGameHerex
{
    public class Player : Character
    {
        private KeyboardState _state;
        private KeyboardState _prevState;

        private Vector2 vel = new Vector2(0f , 0f);
        
        public Player() : base()
        {
            
        }

        public override void Update(KeyboardState state, KeyboardState prevState)
        {
            _state = state;
            _prevState = prevState; // TODO: Maybe unnecessary 

            HandleInputState();
            HandleMovement();
            HandleGravity();
        }

        private void HandleInputState()
        {
            if (_state.IsKeyDown(Keys.D) || _state.IsKeyDown(Keys.Right))
            {
                vel.X = 10.0f;
            }
            else if (_state.IsKeyDown(Keys.A) || _state.IsKeyDown(Keys.Left))
            {
                vel.X = -10.0f;
            }
            else
            {
                vel.X = 0f;
            }
        }

        private void HandleMovement()
        {
            float velScaler = 100.0f;
            if (vel.X != 0)
            {
                pos.X += vel.X / velScaler;
                Debug.WriteLine("Pos.X: " + Pos.X + " pos.X: " + pos.X + " vel.x: " + vel.X);
            }
        }

        private void HandleGravity()
        {
            float g = 9.81f;
            float maxVel = 20.0f;

            if (vel.Y <= maxVel)
            {
                vel.Y += g;
            }
            else if (vel.Y > maxVel)
            {
                vel.Y = maxVel;
            }
        }
    }
}