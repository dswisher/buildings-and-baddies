// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BuildingsAndBaddies.Desktop
{
    public static class SpriteBatchExtensions
    {
        private static Texture2D shapeTexture;

        // From: https://community.monogame.net/t/line-drawing/6962/4
        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, Color color, float thickness = 1f)
        {
            var distance = Vector2.Distance(point1, point2);
            var angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            DrawLine(spriteBatch, point1, distance, angle, color, thickness);
        }


        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 point, float length, float angle, Color color, float thickness = 1f)
        {
            var origin = new Vector2(0f, 0.5f);
            var scale = new Vector2(length, thickness);
            spriteBatch.Draw(GetTexture(spriteBatch), point, null, color, angle, origin, scale, SpriteEffects.None, 0);
        }


        // Based on https://github.com/craftworkgames/MonoGame.Extended/blob/develop/src/cs/MonoGame.Extended/Math/ShapeExtensions.cs
        public static void DrawRectangle(this SpriteBatch spriteBatch, Rectangle rect, Color color, float thickness = 1f)
        {
            var texture = GetTexture(spriteBatch);
            var topLeft = new Vector2(rect.X, rect.Y);
            var topRight = new Vector2(rect.Right - thickness, rect.Y);
            var bottomLeft = new Vector2(rect.X, rect.Bottom - thickness);
            var horizontalScale = new Vector2(rect.Width, thickness);
            var verticalScale = new Vector2(thickness, rect.Height);

            spriteBatch.Draw(texture, topLeft, null, color, 0f, Vector2.Zero, horizontalScale, SpriteEffects.None, 0);
            spriteBatch.Draw(texture, topLeft, null, color, 0f, Vector2.Zero, verticalScale, SpriteEffects.None, 0);
            spriteBatch.Draw(texture, topRight, null, color, 0f, Vector2.Zero, verticalScale, SpriteEffects.None, 0);
            spriteBatch.Draw(texture, bottomLeft, null, color, 0f, Vector2.Zero, horizontalScale, SpriteEffects.None, 0);
        }


        private static Texture2D GetTexture(SpriteBatch spriteBatch)
        {
            if (shapeTexture == null)
            {
                shapeTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                shapeTexture.SetData(new[] { Color.White });
            }

            return shapeTexture;
        }
    }
}
