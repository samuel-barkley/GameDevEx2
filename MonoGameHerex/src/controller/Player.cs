using System;
using System.Buffers.Text;
using System.Data;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameHerex.src.model;
using MonoGameHerex.src.view;
using Vector2 = SharpDX.Vector2;

namespace MonoGameHerex
{
    public class Player : Character
    {
        private KeyboardState _state;
        private KeyboardState _prevState;
        private GameTime _gameTime;
        private Map _map;

        private Vector2 vel = new Vector2(0f , 0f);
        private float velScaler = 100.0f;

        private int temp = 0;

        private Rectangle collisionRect;

        public Player(Map map) : base()
        {
            _map = map;
            collisionRect = new Rectangle((int) Pos.X, (int) Pos.Y, GameScreen.GridSize, GameScreen.GridSize); // Todo: Move rectangle to match sprite
        }

        public override void Update(GameTime gameTime, KeyboardState state, KeyboardState prevState)
        {
            _state = state;             // TODO: Maybe unnecessary 
            _prevState = prevState;     // TODO: Maybe unnecessary 
            _gameTime = gameTime;

            HandleInputState();
            HandleMovement();

            collisionRect.X = (int) Pos.X;
            collisionRect.Y = (int) Pos.Y;
            
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
            velScaler = 100.0f;
            if (vel.X != 0)
            {
                pos.X += vel.X / velScaler;
                //Debug.WriteLine("Pos.X: " + Pos.X + " pos.X: " + pos.X + " vel.x: " + vel.X);
            }
        }

        private void HandleGravity()
        {
            float g = 9.81f;
            float maxVel = 40.0f;

            if (vel.Y + g > maxVel)
            {
                vel.Y = maxVel;
            }
            else if (vel.Y < maxVel)
            {
                vel.Y += g * (_gameTime.ElapsedGameTime.Milliseconds / 1000.0f);
            }

            HandleGravityCollision();
            //Debug.WriteLine(vel.Y);
            pos.Y += vel.Y / velScaler;
        }

        private void HandleGravityCollision()
        {
            if (temp == 0)
            {


                foreach (var tile in _map.tiles)
                {
                    if (tile.CollisionRect.Left <= Pos.X * GameScreen.GridSize &&
                        tile.CollisionRect.Right > Pos.X * GameScreen.GridSize)
                    {
                        Debug.WriteLine("Player X: " + Pos.X + ", tile Left: " + tile.CollisionRect.Left +
                                        ", tile Right: " + tile.CollisionRect.Right + ", tile Top: " + tile.CollisionRect.Top + ", tile Type: " + tile.Type);
                    }

                    /*
                     * What I was about to do:
                     * I was about to make an algorithm to decide which tile is under your feet and check if you are
                     * colliding with that tile. After that I was going to figure out horizontal collisions and ceilings 
                     */

                }

                temp++;
            }
        }
    }
}