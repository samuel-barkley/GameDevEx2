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

        private int temp = 0;

        private bool toMoveLeft;
        private bool toMoveRight;
        private bool toJump;

        private bool stopJump;
        
        private bool hasbeennotnull;
        private int timesnotnull;

        private Tile currentTile;

        public int points;
        private int pointsCollectedThisLvl;
        public bool MadeIt = false;

        public Player(Map map)
        {
            pos = new Vector2(0f, 0f);
            _map = map;
            setSpawnLocation();
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
            HandleExitPoint();
            
            ApplyPosUpdates();
        }
        
        private void setSpawnLocation()
        {
            foreach (var tile in _map.tiles)
            {
                if (tile.Type == TileType.Start)
                {
                    pos.X = (float) tile.GridPos.X / GameScreen.GridSize;
                    pos.Y = (float) tile.GridPos.Y / GameScreen.GridSize;
                }

                if (tile.Type == TileType.Coin)
                {
                    { }
                }
            }
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

            CheckNeighbouringTiles(neighbours, _map);
            
            // If there wasn't a coin, it opens the door.
            if (_map.coinCount <= pointsCollectedThisLvl)
            {
                Exit.isOpen = true;
            }

            #region checks for ground collisions
            
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
            #endregion

            // Checks if colliding with a coin. If you are it removes it from the map and gives the player points.
            if (isOnTile() == TileType.Coin)
            {
                points++;
                pointsCollectedThisLvl++;
                _map.mapLayout[currentTile.GridPos.Y / GameScreen.GridSize, currentTile.GridPos.X / GameScreen.GridSize] = TileType.Air;
            }
        }

        // Checks if player is on a particular tile.
        private TileType isOnTile()
        {
            foreach (var tile in _map.tiles)
            {
                // Filters through tiles and lets the ones through that fit though the horizontal line.
                if (tile.CollisionRect.Top <= Pos.Y * GameScreen.GridSize - GameScreen.GridSize / 2.0f && tile.CollisionRect.Bottom > Pos.Y * GameScreen.GridSize - GameScreen.GridSize / 2.0f)
                {
                    // Filter through the remaining tiles to find the one that fits on the horizontal axis.
                    if (tile.CollisionRect.Left <= Pos.X * GameScreen.GridSize && tile.CollisionRect.Right > Pos.X * GameScreen.GridSize)
                    {
                        currentTile = tile;
                        TileType tileType = tile.Type;
                        if (tileType == TileType.Coin)
                        {
                            currentTile.Type = TileType.Air;
                        }
                        return tileType;
                    }
                }
            }

            Debug.WriteLine("Couldn't find which tile the player is on. The player might've fallen off the map.");
            return TileType.Air;
        }

        // Populated the neighbours tile dictionary. This is used for collision detection.

        private void HandleExitPoint()
        {
            if (isOnTile() == TileType.End && Exit.isOpen)
            {
                MadeIt = true;
            }
        }

        
    }
}