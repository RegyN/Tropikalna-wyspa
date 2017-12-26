using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tropikalna_wyspa
{
    public class PostProcessor
    {
        // Shader przetwarzający, zawiera tylko PixelShader
        public Effect Effect { get; protected set; }
        // Tekstura do przetwarzania
        public Texture2D Input { get; set; }

        protected GraphicsDevice graphicsDevice;
        protected static SpriteBatch spriteBatch;

        public PostProcessor(Effect Effect, GraphicsDevice graphicsDevice)
        {
            this.Effect = Effect;
            if (spriteBatch == null)
                spriteBatch = new SpriteBatch(graphicsDevice);
            this.graphicsDevice = graphicsDevice;
        }

        // Draws the input texture using the pixel shader postprocessor
        public virtual void Draw()
        {
            int screenWidth = graphicsDevice.PresentationParameters.BackBufferWidth;
            int screenHeight = graphicsDevice.PresentationParameters.BackBufferHeight;
            // Set effect parameters if necessary
            if (Effect.Parameters["ScreenWidth"] != null)
                Effect.Parameters["ScreenWidth"].
                SetValue(screenWidth);
            if (Effect.Parameters["ScreenHeight"] != null)
                Effect.Parameters["ScreenHeight"].
                SetValue(screenHeight);
            // Initialize the spritebatch and effect
            Effect.CurrentTechnique = Effect.Techniques["Grayscale"];
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, effect: Effect);
            // Draw the input texture

            Rectangle screenRectangle = new Rectangle(0, 0, screenWidth, screenHeight);
            spriteBatch.Draw((Texture2D)Input, screenRectangle, Color.White);
            // End the spritebatch and effect
            spriteBatch.End();
            // Clean up render states changed by the spritebatch
            graphicsDevice.DepthStencilState = DepthStencilState.Default;
            graphicsDevice.BlendState = BlendState.Opaque;
        }
    }
}
