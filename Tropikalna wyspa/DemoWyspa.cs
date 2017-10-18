using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tropikalna_wyspa
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class DemoWyspa : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D texture;
        Vector2 position;

        public DemoWyspa()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            position = new Vector2(0, 0);
            this.IsFixedTimeStep = true;
            this.TargetElapsedTime = new System.TimeSpan(0, 0, 0, 0, 200);
        }

        protected override void OnActivated(object sender, EventArgs args)
        {
            Window.Title = "Aktywne okno";
            base.OnActivated(sender, args);
        }

        protected override void OnDeactivated(object sender, EventArgs args)
        {
            Window.Title = "Nieaktywne okno";
            base.OnActivated(sender, args);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            texture = new Texture2D(this.GraphicsDevice, 100, 100);
            Color[] colorData = new Color[100 * 100];
            for (int i = 0; i < 10000; i++)
                colorData[i] = Color.Red;

            texture.SetData<Color>(colorData);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            texture.Dispose();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back ==
                    ButtonState.Pressed || Keyboard.GetState().IsKeyDown(
                    Keys.Escape))
                    Exit();

                position.X += 60.0f*(float)gameTime.ElapsedGameTime.TotalSeconds;
                if (position.X > this.GraphicsDevice.Viewport.Width)
                    position.X = 0;
                base.Update(gameTime);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(texture, position);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
