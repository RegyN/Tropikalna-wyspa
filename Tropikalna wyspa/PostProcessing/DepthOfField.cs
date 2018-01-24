using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Tropikalna_wyspa
{
    public class DepthOfField : PostProcessor
    {
        // Depth map and un-blurred render of scene. The blurred render
        // will be set as the Input value
        public Texture2D DepthMap;
        public Texture2D Unblurred;
        public float MaxDepth = 50;
        public float Focus = 12;
        public DepthOfField(GraphicsDevice graphicsDevice,
        ContentManager Content) : base(Content.Load<Effect>("DepthOfField"), graphicsDevice)
        {
        }
        public override void Draw()
        {
            // Set the samplers for all three textures to PointClamp
            // so we can sample pixel values directly
            this.Effect.Parameters["MaxDepth"].SetValue(MaxDepth);
            this.Effect.Parameters["Focus"].SetValue((Focus)/255*MaxDepth);
            this.Effect.Parameters["blurred"].SetValue(DepthMap);
            this.Effect.Parameters["clear"].SetValue(Unblurred);
            this.Effect.Parameters["depth"].SetValue(DepthMap);

            base.Draw();
        }
    }
}
