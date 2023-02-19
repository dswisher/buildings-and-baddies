using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BuildingsAndBaddies.Desktop
{
    public class Game1 : Game
    {
        private enum ClickMode
        {
            AddGuardBot,
            AddHoverBot,
            AddTreadBot,
            SetTarget
        }

        private readonly List<AbstractCreature> creatures = new();

        private ClickMode currentMode = ClickMode.SetTarget;

        private Texture2D guardTexture;
        private Texture2D hoverTexture;
        private Texture2D treadTexture;

        private MouseState currentMouseState;
        private MouseState lastMouseState;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public Game1()
        {
            Content.RootDirectory = "Content";

            IsMouseVisible = true;

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1600;
            graphics.PreferredBackBufferHeight = 900;
            // graphics.IsFullScreen = true;
            graphics.ApplyChanges();
        }


#if false
        protected override void Initialize()
        {
            base.Initialize();
        }
#endif


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO - use a sprite sheet
            // TODO - how to load content and hook it to creatures? This is ugly.

            guardTexture = Content.Load<Texture2D>("creatures/guardbot1");
            hoverTexture = Content.Load<Texture2D>("creatures/hoverbot1");
            treadTexture = Content.Load<Texture2D>("creatures/treadbot1");
        }


        protected override void Update(GameTime gameTime)
        {
            // Default exit settings
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            // Process mouse clicks
            lastMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            if (lastMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed)
            {
                switch (currentMode)
                {
                    case ClickMode.SetTarget:
                        foreach (var creature in creatures)
                        {
                            creature.Goto(currentMouseState.X, currentMouseState.Y);
                        }
                        break;

                    case ClickMode.AddGuardBot:
                        creatures.Add(new GuardBot(guardTexture, currentMouseState.X, currentMouseState.Y));
                        currentMode = ClickMode.SetTarget;
                        break;

                    case ClickMode.AddHoverBot:
                        creatures.Add(new HoverBot(hoverTexture, currentMouseState.X, currentMouseState.Y));
                        currentMode = ClickMode.SetTarget;
                        break;

                    case ClickMode.AddTreadBot:
                        creatures.Add(new TreadBot(treadTexture, currentMouseState.X, currentMouseState.Y));
                        currentMode = ClickMode.SetTarget;
                        break;
                }
            }

            // Update all the creatures
            foreach (var creature in creatures)
            {
                creature.Update(gameTime);
            }

            // Handle keyboard events
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.G))
            {
                currentMode = ClickMode.AddGuardBot;
            }

            if (keyboardState.IsKeyDown(Keys.T))
            {
                currentMode = ClickMode.AddTreadBot;
            }

            if (keyboardState.IsKeyDown(Keys.H))
            {
                currentMode = ClickMode.AddHoverBot;
            }

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            foreach (var creature in creatures)
            {
                creature.Draw(spriteBatch);
            }

            // TODO - if in a "build mode", draw the item-to-build at the current mouse position
            // TODO - draw a HUD that shows the current mode and whatnot

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
