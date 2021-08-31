using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameHerex.src.model;
using MonoGameHerex.src.view;
using Vector2 = SharpDX.Vector2;

namespace MonoGameHerex
{
    public class Enemy : Character
    {
        private Map map;
        
        public Enemy(Vector2 startingPos, Map _map)
        {
            pos.X = startingPos.X;
            pos.Y = startingPos.Y;
            map = _map;
            vel.X = 10.0f;
        }

        public override void Update(GameTime gameTime, KeyboardState state, KeyboardState prevState)
        {
            Dictionary<string, Tile> neighbours = new Dictionary<string, Tile>
            {
                {"up", null}, {"right", null}, {"down", null}, {"left", null}
            };
            
            CheckNeighbouringTiles(neighbours, map);

            if (neighbours["left"] == null || neighbours["right"] == null)
            {
                vel.X *= -1;
            }
            else if (neighbours["left"].Type == TileType.Ground)
            {
                if (neighbours["left"].CollisionRect.Right > Pos.X * GameScreen.GridSize - GameScreen.GridSize / 2.0f)
                {
                    vel.X *= -1;
                }
            }
            else if (neighbours["right"].Type == TileType.Ground)
            {
                if (neighbours["right"].CollisionRect.Left < Pos.X * GameScreen.GridSize + GameScreen.GridSize / 2.0f)
                {
                    vel.X *= -1;
                }
            }
            
            ApplyPosUpdates();
        }
    }
}