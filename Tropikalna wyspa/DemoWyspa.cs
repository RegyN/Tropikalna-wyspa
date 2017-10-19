using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tropikalna_wyspa
{
    public class DemoWyspa : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D texture;
        Vector2 texturePos;

        public DemoWyspa()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            texturePos = new Vector2(0, 0);
        }

        protected override void Initialize()
        {
            texture = new Texture2D(this.GraphicsDevice, 100, 100);
            Color[] colorData = new Color[100 * 100];
            for (int i = 0; i < 10000; i++)
                colorData[i] = Color.Red;

            texture.SetData<Color>(colorData);
            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            texture = this.Content.Load<Texture2D>("logo");
        }

        protected override void UnloadContent()
        {
            texture.Dispose();
        }

        protected override void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back ==
                    ButtonState.Pressed || Keyboard.GetState().IsKeyDown(
                    Keys.Escape))
                    Exit();

                texturePos.X += 60.0f*(float)gameTime.ElapsedGameTime.TotalSeconds;
                if (texturePos.X > this.GraphicsDevice.Viewport.Width)
                    texturePos.X = 0;
                base.Update(gameTime);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(texture, position: new Vector2(0.0f,0.0f), color:Color.White);
            spriteBatch.Draw(texture,
                destinationRectangle: new Rectangle(texture.Width, texture.Height, texture.Width, texture.Height),
                rotation: 0f / 180f * 3.1415f,
                origin: new Vector2(texture.Width / 2, texture.Height / 2));
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}