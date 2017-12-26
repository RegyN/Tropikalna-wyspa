using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tropikalna_wyspa
{
    class RenderCapture
    {
        RenderTarget2D renderTarget;
        GraphicsDevice graphicsDevice;
        public RenderCapture(GraphicsDevice GraphicsDevice)
        {
            var width = GraphicsDevice.PresentationParameters.BackBufferWidth;
            var height = GraphicsDevice.PresentationParameters.BackBufferHeight;
            this.graphicsDevice = GraphicsDevice;
            renderTarget = new RenderTarget2D(GraphicsDevice,
            width, height, false, SurfaceFormat.Color, DepthFormat.Depth24);
        }
        // Begins capturing from the graphics device
        public void Begin()
        {
            graphicsDevice.SetRenderTarget(renderTarget);
        }
        // Stop capturing
        public void End()
        {
            graphicsDevice.SetRenderTarget(null);
        }
        // Returns what was captured
        public Texture2D GetTexture()
        {
            return renderTarget;
        }
    }
}
