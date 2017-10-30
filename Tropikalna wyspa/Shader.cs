using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tropikalna_wyspa
{
    class Shader
    {
        public Effect efekt;
        public Matrix viewMatrix
        {
            set { efekt.Parameters["ViewMatrix"].SetValue(value); }
        }
        public Matrix projectionMatrix
        {
            set { efekt.Parameters["ProjectionMatrix"].SetValue(value); }
        }
        public Matrix worldMatrix
        {
            set { efekt.Parameters["WorldMatrix"].SetValue(value); }
        }
        public Color diffuseColor
        {
            set
            {
                Vector4 a = new Vector4(value.R/255f, value.G/255f, value.B/255f, value.A/255f );
                efekt.Parameters["DiffuseColor"].SetValue(a);
            }
        }
        public Vector3 diffuseLightDirection
        {
            set
            {
                efekt.Parameters["DiffuseLightDirection"].SetValue(value);
            }
        }
        public Color ambientColor
        {
            set
            {
                Vector4 a = new Vector4(value.R/255f, value.G/255f, value.B/255f, value.A/255f );
                efekt.Parameters["AmbienceColor"].SetValue(a);
            }
        }
        public Matrix WorldInverseTransposeMatrix
        {
            set { efekt.Parameters["WorldInverseTransposeMatrix"].SetValue(value); }
        }

        public Shader(Effect ef)
        {
            efekt = ef;
        }
    }
}
