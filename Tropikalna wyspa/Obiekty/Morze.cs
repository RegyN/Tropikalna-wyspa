using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tropikalna_wyspa.Obiekty
{
    class Morze : SquarePrimitive
    {
        public Shader shader;
        public Morze(Microsoft.Xna.Framework.Content.ContentManager cm, GraphicsDevice gd, float size)
            : base(gd, size)
        {
            shader = new Shader(cm.Load<Effect>("OceanShader").Clone());
            shader.efekt.CurrentTechnique = shader.efekt.Techniques["Tex"];
        }
    }
}
