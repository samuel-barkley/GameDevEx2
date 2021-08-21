using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameHerex.src.model;
using MonoGameHerex.src.view;
using SharpDX.Direct3D9;
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

        private Vector2 posToAdd;

        private Rectangle collisionRect;

        private bool toMoveLeft;
        private bool toMoveRight;
        private bool toJump;

        private bool stopJump;

        public Player(Map map)
        {
            pos = new Vector2(2.2f, 2.0f);
            _map = map;
            collisionRect = new Rectangle((int) Pos.X, (int) Pos.Y, GameScreen.GridSize, GameScreen.GridSize); // Todo: Move rectangle to match sprite
            posToAdd = new Vector2();
        }

        public override void Update(GameTime gameTime, KeyboardState state, KeyboardState prevState)
        {
            _state = state;             // TODO: Maybe unnecessary 
            _prevState = prevState;     // TODO: Maybe unnecessary 
            _gameTime = gameTime;

            HandleInputState();
            HandleMovement();

            HandleGravity();
            HandleCollisions();

            applyPosUpdates();
        }

        private void HandleInputState()
        {
            
            if (_state.IsKeyDown(Keys.Space))
            {
                if (!stopJump)
                {
                    toJump = true;
                    stopJump = true;
                }
                else
                {
                    toJump = false;
                }
            }

            if (_state.IsKeyUp(Keys.Space))
            {
                stopJump = false;
            }

            if (_state.IsKeyDown(Keys.D) || _state.IsKeyDown(Keys.Right))
            {
                toMoveRight = true;
            }
            else
            {
                toMoveRight = false;
            }
            
            if (_state.IsKeyDown(Keys.A) || _state.IsKeyDown(Keys.Left))
            {
                toMoveLeft = true;
            }
            else
            {
                toMoveLeft = false;
            }
        }

        private void HandleMovement()
        {
            if (toMoveLeft)
            {
                vel.X = -30;
            }
            else if (toMoveRight)
            {
                vel.X = 30;
            }
            else
            {
                vel.X = 0;
            }
            

            // Debug.WriteLine("toMoveLeft: " + toMoveLeft + ", toMoveRight: " + toMoveRight);
            /*
            if (isJump && onGround)
            {
                vel.Y = jumpForce;
                pos.Y -= 0.2f;
            }*/
        }

        private void HandleGravity()
        {
            float g = 40f;
            float maxVel = 50.0f;

            if (vel.Y + g > maxVel)
            {
                vel.Y = maxVel;
            }
            else if (vel.Y + g < maxVel)
            {
                vel.Y += g * (_gameTime.ElapsedGameTime.Milliseconds / 1000.0f);
            }
            
            //Debug.WriteLine(pos.X + ", " + pos.Y);
        }

        private void HandleCollisions()
        {
            double spriteOffset = 2;

            Tile aboveTile = null;
            Tile rightTile = null;
            Tile bottomTile = null;
            Tile leftTile = null;

            foreach (var tile in _map.tiles)
            {
                if (tile.CollisionRect.Right < (Pos.X * GameScreen.GridSize) + GameScreen.GridSize && tile.CollisionRect.Left > (Pos.X * GameScreen.GridSize) - GameScreen.GridSize)
                {
                    if (tile.CollisionRect.Bottom < (Pos.X * GameScreen.GridSize) + GameScreen.GridSize && tile.CollisionRect.Bottom > Pos.Y + 2 * GameScreen.GridSize)
                    {
                        // Gets top tile
                        aboveTile = tile;
                    }
                    
                    if (tile.CollisionRect.Top > Pos.Y * GameScreen.GridSize && tile.CollisionRect.Top < (Pos.Y * GameScreen.GridSize) + GameScreen.GridSize )
                    {
                        // Gets bottom tile
                        bottomTile = tile;
                    }
                }

                if (tile.CollisionRect.Bottom >= (Pos.Y * GameScreen.GridSize) /*- GameScreen.GridSize*/ && tile.CollisionRect.Bottom <= Pos.Y * GameScreen.GridSize + GameScreen.GridSize)
                {
                    if (tile.CollisionRect.Right <= (Pos.X * GameScreen.GridSize) /*- GameScreen.GridSize */ /*/ 2.0f*/ && tile.CollisionRect.Right > (Pos.X * GameScreen.GridSize) - GameScreen.GridSize /* 1.5f*/)
                    {
                        // Gets Left tile
                        leftTile = tile;
                    }

                    if (tile.CollisionRect.Left >= (Pos.X * GameScreen.GridSize) + GameScreen.GridSize / 2.0f && tile.CollisionRect.Left < (Pos.X * GameScreen.GridSize) + GameScreen.GridSize)
                    {
                        // Gets right tile
                        rightTile = tile;
                    }
                }
            }
            
            if (aboveTile != null && bottomTile != null && leftTile != null && rightTile != null)
            {
                { }
            }

            if (Pos.Y > 4)
            {
                { } // Todo: Remove when I don't want the Debug selected tiles
            }

            /* old collision detection
            List<Tile> tiles = new List<Tile>();

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
                                    if (vel.Y >= 0.0f)
                                    {
                                        onGround = true;
                                    }
                                    onGround = true;
                                    vel.Y = 0.0f;
                                    pos.Y = (float) tile.CollisionRect.Y / GameScreen.GridSize;
                                }

                                if (!isJump && _state.IsKeyDown(Keys.Space))
                                {
                                    vel.Y = 0.0f;
                                    pos.Y = (float) tile.CollisionRect.Y / GameScreen.GridSize;
                                }
                            }
                            else if (tile.CollisionRect.Top < Pos.Y * GameScreen.GridSize + GameScreen.GridSize / 2.0f)
                            {
                                onGround = false;
                            }
                            
                            tiles.Add(tile);
                        }
                    }
                }
            }
            
            if (isJump)
            {
                isJump = false;
            }
            */
        }

        private void applyPosUpdates()
        {
            float velScaler = 500.0f;
            pos.X += vel.X / velScaler;
            pos.Y += vel.Y / velScaler;
        }
    }
}