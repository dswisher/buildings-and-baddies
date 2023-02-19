using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

namespace BuildingsAndBaddies.Desktop
{
    public class AbstractCreature : ICollisionActor
    {
        private readonly Rectangle[] sourceRectangles;
        private readonly Texture2D texture;
        private readonly int frameThreshold;
        private readonly int width;
        private readonly int height;
        private readonly SoundEffect impactSound;

        private float frameTimer;
        private int currentFrame;
        private Point2 target;
        private float speed;
        private bool moving;


        protected AbstractCreature(Texture2D texture, int x, int y, int width, int height, int frames, SoundEffect impactSound)
        {
            this.texture = texture;
            this.width = width;
            this.height = height;
            this.impactSound = impactSound;

            Bounds = new RectangleF(new Vector2(x - width / 2, y - height / 2), new Size2(width, height));

            target = Bounds.Position;
            speed = 200;

            sourceRectangles = new Rectangle[frames];

            for (var i = 0; i < frames; i++)
            {
                sourceRectangles[i] = new Rectangle(i * width, 0, width, height);
            }

            frameThreshold = 250;
        }


        public IShapeF Bounds { get; }


        public void Goto(int x, int y)
        {
            target = new Vector2(x - width / 2, y - height / 2);
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
            var delta = target - Bounds.Position;
            if (delta.Length() > 1.0)
            {
                delta.Normalize();
                Bounds.Position += delta * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                moving = true;
            }
            else
            {
                target = Bounds.Position;      // stop
                moving = false;
            }
        }


        public void Draw(SpriteBatch spriteBatch, bool debugMode)
        {
            // If not at the target, draw a line to the target, if in debug mode
            if (debugMode)
            {
                if (moving)
                {
                    spriteBatch.DrawLine(Bounds.Position, target, Color.Red);
                    spriteBatch.DrawRectangle((RectangleF) Bounds, Color.Red);
                }
                else
                {
                    spriteBatch.DrawRectangle((RectangleF) Bounds, Color.Green);
                }
            }

            // Draw it!
            spriteBatch.Draw(
                texture,
                Bounds.Position,
                sourceRectangles[currentFrame],
                Color.White,
                0f,
                Vector2.Zero,
                Vector2.One,
                SpriteEffects.None,
                0f
            );
        }


        public void OnCollision(CollisionEventArgs collisionInfo)
        {
            impactSound.Play();

            // TODO - improve collision handling - for now, just stop
            if (moving)
            {
                Bounds.Position -= collisionInfo.PenetrationVector;
            }

            target = Bounds.Position;      // stop
            moving = false;
        }
    }
}
