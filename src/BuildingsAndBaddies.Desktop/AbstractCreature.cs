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

        private float frameTimer;
        private int currentFrame;
        private Vector2 position;
        private Vector2 target;
        private float speed;
        private bool moving;


        protected AbstractCreature(Texture2D texture, int x, int y, int width, int height, int frames)
        {
            this.texture = texture;
            this.width = width;
            this.height = height;

            position = new Vector2(x, y);
            target = position;
            speed = 200;

            sourceRectangles = new Rectangle[frames];

            for (var i = 0; i < frames; i++)
            {
                sourceRectangles[i] = new Rectangle(i * width, 0, width, height);
            }

            frameThreshold = 250;
        }


        public void Goto(int x, int y)
        {
            target = new Vector2(x, y);

            // TODO - don't jump there, set a target/path to move there
            // position = new Vector2(x, y);
        }


        public void Update(GameTime gameTime)
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
            var delta = target - position;
            if (delta.Length() > 1.0)
            {
                delta.Normalize();
                position += delta * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                moving = true;
            }
            else
            {
                moving = false;
            }
        }


        public void Draw(SpriteBatch spriteBatch, bool debugMode)
        {
            // If not at the target, draw a line to the target, if in debug mode
            if (debugMode && moving)
            {
                spriteBatch.DrawLine(position, target, Color.Red);
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
    }
}
