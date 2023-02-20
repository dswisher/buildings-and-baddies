// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using BuildingsAndBaddies.Desktop.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BuildingsAndBaddies.Desktop
{
    public class Game1 : Game
    {
        private const int MapWidth = 1600;
        private const int MapHeight = 900;

        private readonly List<AbstractMapItem> mapItems = new();
        private readonly Vector2 fpsPos;

        private ClickMode currentMode = ClickMode.Normal;
        private bool buildBlocked;

        private Texture2D guardTexture;
        private Texture2D hoverTexture;
        private Texture2D treadTexture;

        private Texture2D[] buildingTextures;
        private int currentBuildingTexture;

        private SoundEffect clickSound;
        private SoundEffect dropSound;
        private SoundEffect dustbinSound;
        private SoundEffect impactSound;
        private SoundEffect snapSound;

        private SpriteFont defaultFont;

        private SimpleFps fps;
        private bool showFps;

        private MouseState currentMouseState;
        private MouseState lastMouseState;

        private KeyboardState currentKeyboardState;
        private KeyboardState lastKeyboardState;

        private bool debugMode;
        private bool paused;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public Game1()
        {
            Content.RootDirectory = "Content";

            IsMouseVisible = true;

            graphics = new GraphicsDeviceManager(this);

            // graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = MapWidth;
            graphics.PreferredBackBufferHeight = MapHeight;
            graphics.ApplyChanges();

            fpsPos = new Vector2(10, 10);
        }


        private enum ClickMode
        {
            AddGuardBot,
            AddHoverBot,
            AddTreadBot,
            AddBuilding,
            Normal
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

            buildingTextures = new Texture2D[6];
            for (var i = 0; i < buildingTextures.Length; i++)
            {
                // _02 thru _07
                buildingTextures[i] = Content.Load<Texture2D>($"buildings/scifiStructure_0{i + 2}");
            }

            fps = new SimpleFps(defaultFont);
        }


        protected override void Update(GameTime gameTime)
        {
            // Process mouse clicks
            lastMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            if (lastMouseState.RightButton == ButtonState.Released && currentMouseState.RightButton == ButtonState.Pressed)
            {
                if (currentMode == ClickMode.Normal)
                {
                    // TODO - need a sound effect for this
                    foreach (var item in mapItems)
                    {
                        if (item is AbstractMovableItem ac)
                        {
                            ac.Goto(currentMouseState.X, currentMouseState.Y);
                        }
                    }
                }
            }

            if (lastMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed)
            {
                DoBuild();
            }

            // Update all the items
            if (!paused)
            {
                foreach (var item in mapItems)
                {
                    item.Update(gameTime, mapItems);
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

            if (currentKeyboardState.IsKeyDown(Keys.F) && !lastKeyboardState.IsKeyDown(Keys.F))
            {
                clickSound.Play();
                showFps = !showFps;
            }

            if (currentKeyboardState.IsKeyDown(Keys.Space) && !lastKeyboardState.IsKeyDown(Keys.Space))
            {
                clickSound.Play();
                paused = !paused;
            }

            if (currentKeyboardState.IsKeyDown(Keys.Escape) && !lastKeyboardState.IsKeyDown(Keys.Escape))
            {
                if (currentMode != ClickMode.Normal)
                {
                    dropSound.Play();
                    currentMode = ClickMode.Normal;
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
                currentMode = ClickMode.Normal;
                mapItems.Clear();
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

            if (currentKeyboardState.IsKeyDown(Keys.B) && !lastKeyboardState.IsKeyDown(Keys.B))
            {
                clickSound.Play();

                if (currentMode == ClickMode.AddBuilding)
                {
                    currentBuildingTexture = (currentBuildingTexture + 1) % buildingTextures.Length;
                }

                currentMode = ClickMode.AddBuilding;
            }

            if (currentKeyboardState.IsKeyDown(Keys.H) && !lastKeyboardState.IsKeyDown(Keys.H))
            {
                clickSound.Play();
                currentMode = ClickMode.AddHoverBot;
            }

            fps.Update(gameTime);

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            foreach (var item in mapItems)
            {
                item.Draw(spriteBatch, debugMode);
            }

            if (showFps)
            {
                fps.Draw(spriteBatch, fpsPos, Color.Blue);
            }

            DrawBuildItem();

            if (debugMode)
            {
                spriteBatch.DrawString(defaultFont, "Debug", new Vector2(10, 875), Color.Black);
            }

            if (paused)
            {
                // TODO - some sort of fancy "paused" overlay
                const string pauseMsg = "Paused";
                var size = defaultFont.MeasureString(pauseMsg);
                var pos = new Vector2(MapWidth / 2f - size.X / 2, MapHeight / 2f - size.Y / 2);
                spriteBatch.DrawString(defaultFont, pauseMsg, pos, Color.Red);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }


        private void DoBuild()
        {
            if (buildBlocked)
            {
                return;
            }

            switch (currentMode)
            {
                case ClickMode.AddGuardBot:
                    snapSound.Play();
                    mapItems.Add(new GuardBot(guardTexture, currentMouseState.X, currentMouseState.Y));
                    break;

                case ClickMode.AddHoverBot:
                    snapSound.Play();
                    mapItems.Add(new HoverBot(hoverTexture, currentMouseState.X, currentMouseState.Y));
                    break;

                case ClickMode.AddTreadBot:
                    snapSound.Play();
                    mapItems.Add(new TreadBot(treadTexture, currentMouseState.X, currentMouseState.Y));
                    break;

                case ClickMode.AddBuilding:
                    snapSound.Play();
                    mapItems.Add(new SimpleBuilding(buildingTextures[currentBuildingTexture], currentMouseState.X, currentMouseState.Y));
                    break;
            }
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

                case ClickMode.AddBuilding:
                    texture = buildingTextures[currentBuildingTexture];
                    width = 64;
                    height = 64;
                    break;

                default:
                    return;
            }

            var sourceRectangle = new Rectangle(0, 0, width, height);
            var position = new Vector2(currentMouseState.X - width / 2, currentMouseState.Y - height / 2);
            var targetRectangle = new Rectangle((int)position.X, (int)position.Y, width, height);

            buildBlocked = false;
            foreach (var item in mapItems)
            {
                if (item.Bounds.Intersects(targetRectangle))
                {
                    buildBlocked = true;
                    break;
                }
            }

            if (buildBlocked)
            {
                // TODO - need better way to indicate blocked build
                spriteBatch.DrawRectangle(targetRectangle, Color.Red);
            }
            else
            {
                spriteBatch.Draw(texture, position, sourceRectangle, Color.White);
            }
        }
    }
}
