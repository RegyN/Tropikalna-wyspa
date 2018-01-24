using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Tropikalna_wyspa
{
    class NullPostProcessor : PostProcessor
    {
        public NullPostProcessor(GraphicsDevice graphicsDevice, ContentManager Content)
            : base(Content.Load<Effect>("NoPostProc"), graphicsDevice)
        {
        }
    }
}
