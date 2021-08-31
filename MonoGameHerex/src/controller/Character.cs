using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameHerex.src.model;
using MonoGameHerex.src.view;

namespace MonoGameHerex
{
    public abstract class Character : IController
    {
        protected Vector2 pos = new Vector2(0f, 0f);
        protected Vector2 vel = new Vector2(0f , 0f);
        protected float velScaler = 500.0f;

        public Vector2 Pos
        {
            get { return pos; }
            protected set
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
        
        protected void CheckNeighbouringTiles(Dictionary<string, Tile> neighbours, Map _map)
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

                /*  Let's all tiles pass where the bottom of the tile is below the middle of the player.
                 *  &&
                 *  Let's all tiles pass where the bottom of the tile is above the middle of the player.
                 */
                if (tile.CollisionRect.Bottom >= (Pos.Y * GameScreen.GridSize) - GameScreen.GridSize / 2.0f && tile.CollisionRect.Bottom <= Pos.Y * GameScreen.GridSize + GameScreen.GridSize / 2.0f)
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
        
        protected void ApplyPosUpdates()
        {
            pos.X += vel.X / velScaler;
            pos.Y += vel.Y / velScaler;
        }
    }
}