using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BuildingsAndBaddies.Desktop
{
    public class AbstractCreature
    {
        private readonly int width;
        private readonly int height;
        private readonly Rectangle[] sourceRectangles;
        private readonly Texture2D texture;
        private readonly int frameThreshold;
        private readonly float speed;

        private float frameTimer;
        private int currentFrame;

        private Rectangle bounds;
        private Vector2 position;
        private Vector2 target;
        private bool moving;


        /// <summary>
        /// Create a creature.
        /// </summary>
        /// <param name="texture">The sprite sheet representing the creature.</param>
        /// <param name="x">The X coordinate of the desired center of the sprite.</param>
        /// <param name="y">The Y coordinate of the desired center of the sprite.</param>
        /// <param name="width">The width of the sprite.</param>
        /// <param name="height">The height of the sprite.</param>
        /// <param name="frames">The number of frames of animation for this sprite.</param>
        /// <param name="speed">The speed for this creature.</param>
        protected AbstractCreature(Texture2D texture, int x, int y, int width, int height, int frames, float speed)
        {
            this.texture = texture;
            this.width = width;
            this.height = height;
            this.speed = speed;

            bounds = new Rectangle(x - width / 2, y - height / 2, width, height);

            position = new Vector2(x, y);
            target = position;

            sourceRectangles = new Rectangle[frames];

            for (var i = 0; i < frames; i++)
            {
                sourceRectangles[i] = new Rectangle(i * width, 0, width, height);
            }

            frameThreshold = 250;
        }


        /// <summary>
        /// Move the sprite to the specified position.
        /// </summary>
        /// <param name="x">The X coordinate desired new center of the sprite.</param>
        /// <param name="y">The Y coordinate desired new center of the sprite.</param>
        public void Goto(int x, int y)
        {
            // TODO - need to find a path
            target = new Vector2(x, y);
        }


        public void Update(GameTime gameTime, List<AbstractCreature> creatures)
        {
            // Animate the sprite
            if (frameTimer > frameThreshold)
            {
                // Time to advance to the next frame
                currentFrame = (currentFrame + 1) % sourceRectangles.Length;
                frameTimer = 0;
            }
            else
            {
                // Not yet time to advance, just increment the timer
                frameTimer += gameTime.ElapsedGameTime.Milliseconds;
            }

            // If not at the target, move towards it
            var direction = target - position;
            if (direction.Length() > 2.0)
            {
                moving = true;

                direction.Normalize();

                var delta = direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                foreach (var creature in creatures)
                {
                    // Cannot collide with ourself
                    if (creature == this)
                    {
                        continue;
                    }

                    // If we would hit left or right, zero out the X component
                    if ((delta.X > 0 && WouldIntersectRight(creature, delta.X)) || (delta.X < 0 && WouldIntersectLeft(creature, delta.X)))
                    {
                        delta.X = 0;
                    }

                    // If we would hit top or bottom, zero out the Y component
                    if ((delta.Y > 0 && WouldIntersectTop(creature, delta.Y)) || (delta.Y < 0 && WouldIntersectBottom(creature, delta.Y)))
                    {
                        delta.Y = 0;
                    }
                }

                // Move (or try to)
                if (delta != Vector2.Zero)
                {
                    position += delta;
                    bounds = new Rectangle((int)(position.X - width / 2f), (int)(position.Y - height / 2f), width, height);
                }
            }
            else
            {
                moving = false;
                target = position;
            }
        }


        public void Draw(SpriteBatch spriteBatch, bool debugMode)
        {
            // If in debug mode, draw a line to the target (if moving) and the collision
            // bounding rectangle
            if (debugMode)
            {
                if (moving)
                {
                    spriteBatch.DrawLine(position, target, Color.Red);
                    spriteBatch.DrawRectangle(bounds, Color.Red);
                }
                else
                {
                    spriteBatch.DrawRectangle(bounds, Color.Green);
                }
            }

            // Draw it!
            spriteBatch.Draw(
                texture,
                position,
                sourceRectangles[currentFrame],
                Color.White,
                0f,
                new Vector2(width / 2f, height / 2f),
                Vector2.One,
                SpriteEffects.None,
                0f
            );
        }


        /// <summary>
        /// If this creature moved left by dx, would it intersect the other creature?
        /// </summary>
        /// <param name="other">The other creature to check.</param>
        /// <param name="dx">The amount to move left. Should be a negative value.</param>
        /// <returns>True if they would intersect.</returns>
        private bool WouldIntersectLeft(AbstractCreature other, float dx)
        {
            Debug.Assert(dx < 0);

            // Are they above or below each other?
            if (bounds.Top > other.bounds.Bottom || bounds.Bottom < other.bounds.Top)
            {
                return false;
            }

            // Are they already past?
            if (bounds.Left < other.bounds.Left)
            {
                return false;
            }

            // Would they collide?
            return bounds.Left + dx < other.bounds.Right;
        }


        /// <summary>
        /// If this creature moved right by dx, would it intersect the other creature?
        /// </summary>
        /// <param name="other">The other creature to check.</param>
        /// <param name="dx">The amount to move right. Should be a positive value.</param>
        /// <returns>True if they would intersect.</returns>
        private bool WouldIntersectRight(AbstractCreature other, float dx)
        {
            Debug.Assert(dx > 0);

            // Are they above or below each other?
            if (bounds.Top > other.bounds.Bottom || bounds.Bottom < other.bounds.Top)
            {
                return false;
            }

            // Are they already past?
            if (bounds.Right > other.bounds.Right)
            {
                return false;
            }

            // Would they collide?
            return bounds.Right + dx > other.bounds.Left;
        }


        /// <summary>
        /// If this creature moved down by dy, would it intersect the other creature?
        /// </summary>
        /// <param name="other">The other creature to check.</param>
        /// <param name="dy">The amount to move down. Should be a positive value.</param>
        /// <returns>True if they would intersect.</returns>
        private bool WouldIntersectTop(AbstractCreature other, float dy)
        {
            Debug.Assert(dy > 0);

            // Are they left or right of each other?
            if (bounds.Left > other.bounds.Right || bounds.Right < other.bounds.Left)
            {
                return false;
            }

            // Are they already past?
            if (bounds.Bottom > other.bounds.Bottom)
            {
                return false;
            }

            // Would they collide?
            return bounds.Bottom + dy > other.bounds.Top;
        }


        /// <summary>
        /// If this creature moved up by dy, would it intersect the other creature?
        /// </summary>
        /// <param name="other">The other creature to check.</param>
        /// <param name="dy">The amount to move up. Should be a negative value.</param>
        /// <returns>True if they would intersect.</returns>
        private bool WouldIntersectBottom(AbstractCreature other, float dy)
        {
            Debug.Assert(dy < 0);

            // Are they left or right of each other?
            if (bounds.Left > other.bounds.Right || bounds.Right < other.bounds.Left)
            {
                return false;
            }

            // Are they already past?
            if (bounds.Top < other.bounds.Top)
            {
                return false;
            }

            // Would they collide?
            return bounds.Top + dy < other.bounds.Bottom;
        }
    }
}
