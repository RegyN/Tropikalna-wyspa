using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text;

namespace Tropikalna_wyspa
{
    public class DemoWyspa : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D texture;
        Vector2 texturePos;
        KeyboardState prevState;

        public DemoWyspa()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            texturePos = new Vector2(0, 0);
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            texture = new Texture2D(this.GraphicsDevice, 100, 100);
            Color[] colorData = new Color[100 * 100];
            for (int i = 0; i < 10000; i++)
                colorData[i] = Color.Red;
            prevState = Keyboard.GetState();
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
                KeyboardState state = Keyboard.GetState();
                if (state.IsKeyDown(Keys.Escape))
                    Exit();
                if (state.IsKeyDown(Keys.A))
                    System.Diagnostics.Debug.WriteLine("Siema");
                if (state.IsKeyDown(Keys.Down))
                    texturePos.Y += 10;
                if (state.IsKeyDown(Keys.Up))
                    texturePos.Y -= 10;
                if (state.IsKeyDown(Keys.Left))
                    texturePos.X -= 10;
                if (state.IsKeyDown(Keys.Right))
                    texturePos.X += 10;

                MouseState mstate = Mouse.GetState();
                if (mstate.RightButton == ButtonState.Pressed)
                    Exit();
                base.Update(gameTime);
                prevState = state;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(texture, position: texturePos, color:Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}