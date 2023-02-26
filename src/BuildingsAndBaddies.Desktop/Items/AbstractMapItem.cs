// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using BuildingsAndBaddies.Desktop.Grid;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BuildingsAndBaddies.Desktop.Items
{
    public class AbstractMapItem
    {
        private readonly int frameThreshold;
        private readonly Texture2D texture;
        private readonly Rectangle[] sourceRectangles;


        private int currentFrame;
        private float frameTimer;

        protected AbstractMapItem(Texture2D texture, int x, int y, int width, int height, int frames)
        {
            this.texture = texture;
            Width = width;
            Height = height;

            sourceRectangles = new Rectangle[frames];
            for (var i = 0; i < frames; i++)
            {
                sourceRectangles[i] = new Rectangle(i * width, 0, width, height);
            }

            Position = new Vector2(x, y);
            Bounds = new Rectangle(x - width / 2, y - height / 2, width, height);

            frameThreshold = 250;

            IsFixed = this is SimpleBuilding;
        }


        public Rectangle Bounds { get; protected set; }
        public bool IsFixed { get; }

        protected int Width { get; }
        protected int Height { get; }
        protected Vector2 Position { get; set; }


        public virtual void Update(GameTime gameTime, List<AbstractMapItem> items, PathGrid pathGrid)
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
        }


        public virtual void Draw(SpriteBatch spriteBatch, bool debugMode)
        {
            spriteBatch.Draw(
                texture,
                Position,
                sourceRectangles[currentFrame],
                Color.White,
                0f,
                new Vector2(Width / 2f, Height / 2f),
                Vector2.One,
                SpriteEffects.None,
                0f);
        }
    }
}
