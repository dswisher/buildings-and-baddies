// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BuildingsAndBaddies.Desktop.Items
{
    public class AbstractMovableItem : AbstractMapItem
    {
        private readonly float speed;

        private Vector2 target;
        private bool moving;


        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractMovableItem"/> class.
        /// </summary>
        /// <param name="texture">The sprite sheet representing the creature.</param>
        /// <param name="x">The X coordinate of the desired center of the sprite.</param>
        /// <param name="y">The Y coordinate of the desired center of the sprite.</param>
        /// <param name="width">The width of the sprite.</param>
        /// <param name="height">The height of the sprite.</param>
        /// <param name="frames">The number of frames of animation for this sprite.</param>
        /// <param name="speed">The speed for this creature.</param>
        protected AbstractMovableItem(Texture2D texture, int x, int y, int width, int height, int frames, float speed)
            : base(texture, x, y, width, height, frames)
        {
            this.speed = speed;

            target = Position;
        }


        /// <summary>
        /// Move the sprite to the specified position.
        /// </summary>
        /// <param name="x">The X coordinate desired new center of the sprite.</param>
        /// <param name="y">The Y coordinate desired new center of the sprite.</param>
        /// <param name="pathGrid">The path grid used to find a path.</param>
        public void Goto(int x, int y, PathGrid pathGrid)
        {
            // TODO - need to find a path
            target = new Vector2(x, y);
        }


        public override void Update(GameTime gameTime, List<AbstractMapItem> items, PathGrid pathGrid)
        {
            // Animate the sprite
            base.Update(gameTime, items, pathGrid);

            // If not at the target, move towards it
            var direction = target - Position;
            if (direction.Length() > 2.0)
            {
                moving = true;

                direction.Normalize();

                var delta = direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                foreach (var item in items)
                {
                    // Cannot collide with ourself
                    if (item == this)
                    {
                        continue;
                    }

                    // If we would hit left or right, zero out the X component
                    if ((delta.X > 0 && WouldIntersectRight(item, delta.X)) || (delta.X < 0 && WouldIntersectLeft(item, delta.X)))
                    {
                        delta.X = 0;
                    }

                    // If we would hit top or bottom, zero out the Y component
                    if ((delta.Y > 0 && WouldIntersectTop(item, delta.Y)) || (delta.Y < 0 && WouldIntersectBottom(item, delta.Y)))
                    {
                        delta.Y = 0;
                    }
                }

                // Move (or try to)
                if (delta != Vector2.Zero)
                {
                    // Remove the old position from the grid
                    pathGrid.RemoveItem(Bounds);

                    // Update the position and the bounds
                    Position += delta;
                    Bounds = new Rectangle((int)(Position.X - Width / 2f), (int)(Position.Y - Height / 2f), Width, Height);

                    // Add the new position into the grid
                    pathGrid.AddItem(Bounds);
                }
            }
            else
            {
                moving = false;
                target = Position;
            }
        }


        public override void Draw(SpriteBatch spriteBatch, bool debugMode)
        {
            // If in debug mode, draw a line to the target (if moving) and the collision
            // bounding rectangle
            if (debugMode)
            {
                if (moving)
                {
                    spriteBatch.DrawLine(Position, target, Color.Red);
                    spriteBatch.DrawRectangle(Bounds, Color.Red);
                }
                else
                {
                    spriteBatch.DrawRectangle(Bounds, Color.Green);
                }
            }

            // Let the base class handle the rest
            base.Draw(spriteBatch, debugMode);
        }


        /// <summary>
        /// If this creature moved left by dx, determine if it intersect the other creature.
        /// </summary>
        /// <param name="other">The other creature to check.</param>
        /// <param name="dx">The amount to move left. Should be a negative value.</param>
        /// <returns>True if they would intersect.</returns>
        private bool WouldIntersectLeft(AbstractMapItem other, float dx)
        {
            Debug.Assert(dx < 0, "dx < 0");

            // Are they above or below each other?
            if (Bounds.Top > other.Bounds.Bottom || Bounds.Bottom < other.Bounds.Top)
            {
                return false;
            }

            // Are they already past?
            if (Bounds.Left < other.Bounds.Left)
            {
                return false;
            }

            // Would they collide?
            return Bounds.Left + dx < other.Bounds.Right;
        }


        /// <summary>
        /// If this creature moved right by dx, determine if it intersect the other creature.
        /// </summary>
        /// <param name="other">The other creature to check.</param>
        /// <param name="dx">The amount to move right. Should be a positive value.</param>
        /// <returns>True if they would intersect.</returns>
        private bool WouldIntersectRight(AbstractMapItem other, float dx)
        {
            Debug.Assert(dx > 0, "dx > 0");

            // Are they above or below each other?
            if (Bounds.Top > other.Bounds.Bottom || Bounds.Bottom < other.Bounds.Top)
            {
                return false;
            }

            // Are they already past?
            if (Bounds.Right > other.Bounds.Right)
            {
                return false;
            }

            // Would they collide?
            return Bounds.Right + dx > other.Bounds.Left;
        }


        /// <summary>
        /// If this creature moved down by dy, determine if it intersect the other creature.
        /// </summary>
        /// <param name="other">The other creature to check.</param>
        /// <param name="dy">The amount to move down. Should be a positive value.</param>
        /// <returns>True if they would intersect.</returns>
        private bool WouldIntersectTop(AbstractMapItem other, float dy)
        {
            Debug.Assert(dy > 0, "dy > 0");

            // Are they left or right of each other?
            if (Bounds.Left > other.Bounds.Right || Bounds.Right < other.Bounds.Left)
            {
                return false;
            }

            // Are they already past?
            if (Bounds.Bottom > other.Bounds.Bottom)
            {
                return false;
            }

            // Would they collide?
            return Bounds.Bottom + dy > other.Bounds.Top;
        }


        /// <summary>
        /// If this creature moved up by dy, determine if it intersect the other creature.
        /// </summary>
        /// <param name="other">The other creature to check.</param>
        /// <param name="dy">The amount to move up. Should be a negative value.</param>
        /// <returns>True if they would intersect.</returns>
        private bool WouldIntersectBottom(AbstractMapItem other, float dy)
        {
            Debug.Assert(dy < 0, "dy < 0");

            // Are they left or right of each other?
            if (Bounds.Left > other.Bounds.Right || Bounds.Right < other.Bounds.Left)
            {
                return false;
            }

            // Are they already past?
            if (Bounds.Top < other.Bounds.Top)
            {
                return false;
            }

            // Would they collide?
            return Bounds.Top + dy < other.Bounds.Bottom;
        }
    }
}
