using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

namespace BuildingsAndBaddies.Desktop
{
    public class Game1 : Game
    {
        private const int MapWidth = 1600;
        private const int MapHeight = 900;

        private enum ClickMode
        {
            AddGuardBot,
            AddHoverBot,
            AddTreadBot,
            SetTarget
        }

        private readonly List<AbstractCreature> creatures = new();
        private readonly CollisionComponent collisionComponent;

        private ClickMode currentMode = ClickMode.SetTarget;

        private Texture2D guardTexture;
        private Texture2D hoverTexture;
        private Texture2D treadTexture;

        private SoundEffect clickSound;
        private SoundEffect dropSound;
        private SoundEffect dustbinSound;
        private SoundEffect impactSound;
        private SoundEffect snapSound;

        private SpriteFont defaultFont;

        private MouseState currentMouseState;
        private MouseState lastMouseState;

        private KeyboardState currentKeyboardState;
        private KeyboardState lastKeyboardState;

        private bool debugMode;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public Game1()
        {
            Content.RootDirectory = "Content";

            IsMouseVisible = true;

            graphics = new GraphicsDeviceManager(this);

            collisionComponent = new CollisionComponent(new RectangleF(0,0, MapWidth, MapHeight));
        }


        protected override void Initialize()
        {
            base.Initialize();

            graphics.PreferredBackBufferWidth = MapWidth;
            graphics.PreferredBackBufferHeight = MapHeight;
            // graphics.IsFullScreen = true;
            graphics.ApplyChanges();
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            clickSound = Content.Load<SoundEffect>("sounds/click");
            dropSound = Content.Load<SoundEffect>("sounds/drop");
            dustbinSound = Content.Load<SoundEffect>("sounds/dustbin");
            impactSound = Content.Load<SoundEffect>("sounds/impact");
            snapSound = Content.Load<SoundEffect>("sounds/snap");

            defaultFont = Content.Load<SpriteFont>("fonts/default");

            // TODO - use a sprite sheet
            // TODO - how to load content and hook it to creatures? This is ugly.

            guardTexture = Content.Load<Texture2D>("creatures/guardbot1");
            hoverTexture = Content.Load<Texture2D>("creatures/hoverbot1");
            treadTexture = Content.Load<Texture2D>("creatures/treadbot1");
        }


        protected override void Update(GameTime gameTime)
        {
            // Process mouse clicks
            lastMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            if (lastMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed)
            {
                switch (currentMode)
                {
                    case ClickMode.SetTarget:
                        // TODO - need a sound effect for this
                        foreach (var creature in creatures)
                        {
                            creature.Goto(currentMouseState.X, currentMouseState.Y);
                        }
                        break;

                    case ClickMode.AddGuardBot:
                        AddCreature(new GuardBot(guardTexture, currentMouseState.X, currentMouseState.Y, impactSound));
                        break;

                    case ClickMode.AddHoverBot:
                        AddCreature(new HoverBot(hoverTexture, currentMouseState.X, currentMouseState.Y, impactSound));
                        break;

                    case ClickMode.AddTreadBot:
                        AddCreature(new TreadBot(treadTexture, currentMouseState.X, currentMouseState.Y, impactSound));
                        break;
                }
            }

            // Handle keyboard events
            lastKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            if (currentKeyboardState.IsKeyDown(Keys.D) && !lastKeyboardState.IsKeyDown(Keys.D))
            {
                clickSound.Play();
                debugMode = !debugMode;
            }

            if (currentKeyboardState.IsKeyDown(Keys.Escape) && !lastKeyboardState.IsKeyDown(Keys.Escape))
            {
                if (currentMode != ClickMode.SetTarget)
                {
                    dropSound.Play();
                    currentMode = ClickMode.SetTarget;
                }
            }

            if (currentKeyboardState.IsKeyDown(Keys.X) && !lastKeyboardState.IsKeyDown(Keys.X))
            {
                // TODO - how to play a sound effect and have the game wait until done?
                Exit();
            }

            if (currentKeyboardState.IsKeyDown(Keys.C) && !lastKeyboardState.IsKeyDown(Keys.C))
            {
                dustbinSound.Play();
                currentMode = ClickMode.SetTarget;

                foreach (var creature in creatures)
                {
                    collisionComponent.Remove(creature);
                }

                creatures.Clear();
            }

            if (currentKeyboardState.IsKeyDown(Keys.G) && !lastKeyboardState.IsKeyDown(Keys.G))
            {
                clickSound.Play();
                currentMode = ClickMode.AddGuardBot;
            }

            if (currentKeyboardState.IsKeyDown(Keys.T) && !lastKeyboardState.IsKeyDown(Keys.T))
            {
                clickSound.Play();
                currentMode = ClickMode.AddTreadBot;
            }

            if (currentKeyboardState.IsKeyDown(Keys.H) && !lastKeyboardState.IsKeyDown(Keys.H))
            {
                clickSound.Play();
                currentMode = ClickMode.AddHoverBot;
            }

            // Update all the creatures
            foreach (var creature in creatures)
            {
                creature.Update(gameTime);
            }

            collisionComponent.Update(gameTime);

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            foreach (var creature in creatures)
            {
                creature.Draw(spriteBatch, debugMode);
            }

            DrawBuildItem();

            if (debugMode)
            {
                spriteBatch.DrawString(defaultFont, "Debug", new Vector2(10, 875), Color.Black);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }


        private void AddCreature(AbstractCreature creature)
        {
            snapSound.Play();
            creatures.Add(creature);
            collisionComponent.Insert(creature);
            currentMode = ClickMode.SetTarget;
        }


        private void DrawBuildItem()
        {
            Texture2D texture;
            int width;
            int height;

            switch (currentMode)
            {
                case ClickMode.AddGuardBot:
                    texture = guardTexture;
                    width = 28;
                    height = 31;
                    break;

                case ClickMode.AddHoverBot:
                    texture = hoverTexture;
                    width = 32;
                    height = 30;
                    break;

                case ClickMode.AddTreadBot:
                    texture = treadTexture;
                    width = 28;
                    height = 31;
                    break;

                default:
                    return;
            }

            var sourceRectangle = new Rectangle(0, 0, width, height);
            var position = new Vector2(currentMouseState.X - width / 2, currentMouseState.Y - height / 2);

            spriteBatch.Draw(texture, position, sourceRectangle, Color.White);
        }
    }
}
