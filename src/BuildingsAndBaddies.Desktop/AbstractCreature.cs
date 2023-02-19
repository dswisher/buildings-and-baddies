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


        protected AbstractCreature(Texture2D texture, int x, int y, int width, int height, int frames)
        {
            this.texture = texture;
            this.width = width;
            this.height = height;

            position = new Vector2(x, y);

            sourceRectangles = new Rectangle[frames];

            for (var i = 0; i < frames; i++)
            {
                sourceRectangles[i] = new Rectangle(i * width, 0, width, height);
            }

            frameThreshold = 250;
        }


        public void Goto(int x, int y)
        {
            // TODO - don't jump there, set a target/path to move there
            position = new Vector2(x, y);
        }


        public void Update(GameTime gameTime)
        {
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


        public void Draw(SpriteBatch spriteBatch)
        {
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
