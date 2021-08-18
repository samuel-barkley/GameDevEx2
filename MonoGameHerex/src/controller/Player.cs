using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameHerex.src.model;
using MonoGameHerex.src.view;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace MonoGameHerex
{
    public class Player : Character
    {
        private bool isJump;
        private bool onGround;
        private float jumpForce = -20.0f;
        
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
            pos = new Vector2(0.2f, 0.0f);
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
            if (_state.IsKeyDown(Keys.Space))
            {
                isJump = true;
            }
            
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
            }

            if (isJump && onGround)
            {
                vel.Y = jumpForce;
                pos.Y -= 0.2f;
            }
        }

        private void HandleGravity()
        {
            float g = 30f;
            float maxVel = 50.0f;

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
            double spriteOffset = 2;
            List<Tile> tiles = new List<Tile>();

            bool foundCollision = false;
            if (temp == 0)
            {
                foreach (var tile in _map.tiles)
                {
                    if (tile.CollisionRect.Left <= Pos.X * GameScreen.GridSize &&
                        tile.CollisionRect.Right > Pos.X * GameScreen.GridSize)
                    {
                        if (tile.CollisionRect.Bottom > Pos.Y * GameScreen.GridSize && tile.Type == TileType.Ground)
                        {
                            if (tile.CollisionRect.Top <= Pos.Y * GameScreen.GridSize + spriteOffset)
                            {
                                if (!isJump)
                                {
                                    foundCollision = true;
                                    if (vel.Y >= 0.0f)
                                    {
                                        onGround = true;
                                    }
                                    onGround = true;
                                    vel.Y = 0.0f;
                                    pos.Y = tile.CollisionRect.Y / GameScreen.GridSize;
                                }
                            }
                            else if (tile.CollisionRect.Top < Pos.Y * GameScreen.GridSize + GameScreen.GridSize / 2)
                            {
                                onGround = false;
                            }
                            
                            tiles.Add(tile);
                        }
                    }
                }
/*
                if (foundCollision)
                {
                    if (vel.Y >= 0.0f)
                    {
                        onGround = true;
                    }
                    vel.Y = 0.0f;
                }*/
            }

            if (isJump)
            {
                isJump = false;
            }
        }
    }
}