using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
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
        private float jumpForce = -50.0f;
        
        private KeyboardState _state;
        private KeyboardState _prevState;
        private GameTime _gameTime;
        private Map _map;

        private Vector2 vel = new Vector2(0f , 0f);
        private float velScaler = 500.0f;

        private int temp = 0;

        private Vector2 posToAdd;

        private Rectangle collisionRect;

        private bool toMoveLeft;
        private bool toMoveRight;
        private bool toJump;

        private bool stopJump;
        
        private bool hasbeennotnull;
        private int timesnotnull;

        private int bottomHits;
        private List<Tile> bottomsHit;

        public Player(Map map)
        {
            bottomsHit = new List<Tile>();
            
            pos = new Vector2(2.5f, 2.0f);
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
            // Debug.WriteLine("IsJump: " + toJump + ", onGround: " + onGround);
            if (toJump && onGround)
            {
                vel.Y = jumpForce;
                pos.Y -= 0.2f;
            }
        }

        private void HandleGravity()
        {
            float g = 70f;
            float maxVel = 150.0f;

            if (vel.Y + g > maxVel)
            {
                vel.Y = maxVel;
            }
            else if (vel.Y + g < maxVel)
            {
                vel.Y += g * (_gameTime.ElapsedGameTime.Milliseconds / 1000.0f);
            }
        }

        private void HandleCollisions()
        {
            double spriteOffset = 2;

            Dictionary<string, Tile> neighbours = new Dictionary<string, Tile>
            {
                {"up", null}, {"right", null}, {"down", null}, {"left", null}
            };

            CheckNeighbouringTiles(neighbours);

            
            if (neighbours["up"] != null && neighbours["down"] != null)
            {
                // Checks if colliding with floor.
                if (neighbours["down"].Type == TileType.Ground)
                {
                    if (neighbours["down"].CollisionRect.Top - (Pos.Y * GameScreen.GridSize) < (vel.Y / velScaler) * GameScreen.GridSize)
                    {
                        pos.Y = neighbours["down"].CollisionRect.Top / (float) GameScreen.GridSize;
                        vel.Y = 0.0f;
                        onGround = true;
                    }
                    else
                    {
                        onGround = false;
                    }
                }
                else
                {
                    onGround = false;
                }
                
                // Checks if colliding with ceiling.
                if (neighbours["up"].Type == TileType.Ground)
                {
                    if ((Pos.Y * GameScreen.GridSize - GameScreen.GridSize + 1) - neighbours["up"].CollisionRect.Bottom < (vel.Y / velScaler) * GameScreen.GridSize)
                    {
                        pos.Y = (float)(neighbours["down"].CollisionRect.Bottom - GameScreen.GridSize) / GameScreen.GridSize;
                        vel.Y = 0.0f;
                        
                    }
                }
            }

            if (neighbours["left"] != null && neighbours["right"] != null)
            {
                // Checks if colliding with wall left
                if (neighbours["left"].Type == TileType.Ground)
                {
                    if (neighbours["left"].CollisionRect.Right > Pos.X * GameScreen.GridSize - (float) GameScreen.GridSize / 2)
                    {
                        vel.X = 0;
                        pos.X = (float) (neighbours["left"].CollisionRect.Right + GameScreen.GridSize / 2 + 2) / GameScreen.GridSize;
                    }
                }
                
                // Checks if colliding with wall right
                if (neighbours["right"].Type == TileType.Ground)
                {
                    if (neighbours["right"].CollisionRect.Left < Pos.X * GameScreen.GridSize + (float) GameScreen.GridSize / 2)
                    {
                        vel.X = 0;
                        pos.X = (float) (neighbours["right"].CollisionRect.Left - GameScreen.GridSize / 2 - 2) / GameScreen.GridSize;
                    }
                }
            }

            if (Pos.Y * GameScreen.GridSize >=385)
            {
                {
                    // Todo: Remove when don't need to debug anymore.
                }
            }
        }

        private void CheckNeighbouringTiles(Dictionary<string, Tile> neighbours)
        {
            foreach (var tile in _map.tiles)
            {
                // Only lets tile through that are above and below the player
                if (tile.CollisionRect.Right <= (Pos.X * GameScreen.GridSize) + GameScreen.GridSize + 5 && tile.CollisionRect.Left > (Pos.X * GameScreen.GridSize) - GameScreen.GridSize - 5)
                {
                    // First part of the statement lets all tiles through that are above the pos of the player. The second part lets the tiles through that are max 1 gridspace away from the pos.
                    if (tile.CollisionRect.Bottom < (Pos.Y * GameScreen.GridSize) /*uncomment to add head collision + GameScreen.GridSize*/ && tile.CollisionRect.Bottom > Pos.Y - GameScreen.GridSize /*Add another gridsize to add headcollision*/)
                    {
                        // Gets top tile
                        neighbours["up"] = tile;
                    }
                    // First part of the statement lets all tiles through that are below the pos of the player. The second part lets the tiles through that are max 1 gridspace away from the pos.
                    if (tile.CollisionRect.Top >= Pos.Y * GameScreen.GridSize - 5 && tile.CollisionRect.Top < (Pos.Y * GameScreen.GridSize) + GameScreen.GridSize)
                    {
                        // Gets bottom tile
                        neighbours["down"] = tile;
                    }
                }

                /*  Let's all tiles pass where the bottom of the tile is below the top of the player.
                 *  &&
                 *  Let's all tiles pass where the bottom of the tile is above the bottom of the player.
                 */
                if (tile.CollisionRect.Bottom >= (Pos.Y * GameScreen.GridSize) - GameScreen.GridSize && tile.CollisionRect.Bottom <= Pos.Y * GameScreen.GridSize)
                {
                    // First part of the statement lets all tiles through that are to the left of the center pos of the player. The second part lets the tiles through that are max 1 gridSpace away from the center point of the player.
                    if (tile.CollisionRect.Right <= (Pos.X * GameScreen.GridSize) /*- GameScreen.GridSize */ /*/ 2.0f*/ && tile.CollisionRect.Right > (Pos.X * GameScreen.GridSize) - GameScreen.GridSize /* 1.5f*/)
                    {
                        // Gets Left tile
                        neighbours["left"] = tile;
                    }
                    // First part of the statement lets all tiles through that are to the right of the center pos of the player. The second part lets the tiles through that are max 1 gridSpace away from the center point of the player.
                    if (tile.CollisionRect.Left >= (Pos.X * GameScreen.GridSize) /*+ GameScreen.GridSize / 2.0f*/ && tile.CollisionRect.Left < (Pos.X * GameScreen.GridSize) + GameScreen.GridSize)
                    {
                        // Gets right tile
                        neighbours["right"] = tile;
                    }
                }
            }
        }

        private void applyPosUpdates()
        {
            
            pos.X += vel.X / velScaler;
            pos.Y += vel.Y / velScaler;
        }
    }
}