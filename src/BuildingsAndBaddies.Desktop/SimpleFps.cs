// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BuildingsAndBaddies.Desktop
{
    // Based on https://community.monogame.net/t/a-simple-monogame-fps-display-class/10545
    public class SimpleFps
    {
        private const double MsgFrequency = 1.0f;

        private readonly SpriteFont font;

        private double frames;
        private double updates;
        private double elapsed;
        private double last;
        private double now;
        private string msg = string.Empty;

        public SimpleFps(SpriteFont font)
        {
            this.font = font;
            frames = 0;
            updates = 0;
            elapsed = 0;
        }


        public void Update(GameTime gameTime)
        {
            now = gameTime.TotalGameTime.TotalSeconds;
            elapsed = now - last;
            if (elapsed > MsgFrequency)
            {
                msg = $"Fps: {frames / elapsed:0.000}\nElapsed time: {elapsed:0.000}\nUpdates: {updates:0.000}\nFrames: {frames:0.000}";

                elapsed = 0;
                frames = 0;
                updates = 0;
                last = now;
            }

            updates++;
        }


        public void Draw(SpriteBatch spriteBatch, Vector2 fpsDisplayPosition, Color fpsTextColor)
        {
            spriteBatch.DrawString(font, msg, fpsDisplayPosition, fpsTextColor);
            frames++;
        }
    }
}
