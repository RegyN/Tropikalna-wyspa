using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tropikalna_wyspa
{
    // TODO: Wyczyścić kod shaderów, tak ogólnie, potem sprzątnąć też trochę w tej klasie bo jest syf.
    class Shader
    {
        public Effect efekt;
        public Vector2 Displacement
        {
            set { efekt.Parameters["displacement"].SetValue(value); }
        }
        public float FogEnabled
        {
            set
            {
                if (value > 1.0f)
                    value = 1.0f;
                else if (value < 0.0f)
                    value = 0.0f;
                efekt.Parameters["fogEnabled"].SetValue(value);
            }
            get { return efekt.Parameters["fogEnabled"].GetValueSingle(); }
        }
        public float FogStart
        {
            set
            {
                if (value < 0.0f)
                    value = 0.0f;
                efekt.Parameters["fogStart"].SetValue(value);
            }
        }
        public float FogEnd
        {
            set
            {
                if (value < 0.0f)
                    value = 0.0f;
                efekt.Parameters["fogEnd"].SetValue(value);
            }
        }
        public Texture2D PrimaryTex
        {
            set { efekt.Parameters["PrimaryTex"].SetValue(value); }
        }
        public Texture2D SecondaryTex
        {
            set { efekt.Parameters["SecondaryTex"].SetValue(value); }
        }
        public Matrix viewMatrix
        {
            set { efekt.Parameters["ViewMatrix"].SetValue(value); }
        }
        public Matrix worldMatrix
        {
            set { efekt.Parameters["WorldMatrix"].SetValue(value); }
        }
        public Matrix projectionMatrix
        {
            set { efekt.Parameters["ProjectionMatrix"].SetValue(value); }
        }
        public Matrix WorldInverseTransposeMatrix
        {
            set { efekt.Parameters["WorldInvTransMat"].SetValue(value); }
        }
        public Color diffuseColor
        {
            set
            {
                Vector4 a = new Vector4(value.R/255f, value.G/255f, value.B/255f, value.A/255f );
                efekt.Parameters["surfaceColor"].SetValue(a);
            }
        }
        public Vector3 viewPosition
        {
            set
            {
                efekt.Parameters["ViewPosition"].SetValue(value);
            }
        }
        public Vector3 diffuseLightDirection
        {
            set
            {
                efekt.Parameters["dirLightDir"].SetValue(value);
            }
        }
        public Color diffuseLightColor
        {
            set
            {
                Vector4 a = new Vector4(value.R/255f, value.G/255f, value.B/255f, value.A/255f);
                efekt.Parameters["dirLightColor"].SetValue(a);
            }
        }
        public Vector3 materialEmissive
        {
            set
            {
                efekt.Parameters["materialEmissive"].SetValue(value);
            }
        }
        public Vector3 materialAmbient
        {
            set
            {
                efekt.Parameters["materialAmbient"].SetValue(value);
            }
        }
        public Vector3 materialDiffuse
        {
            set
            {
                efekt.Parameters["materialDiffuse"].SetValue(value);
            }
        }
        public Vector3 materialSpecular
        {
            set
            {
                efekt.Parameters["materialSpecular"].SetValue(value);
            }
        }
        public float materialPower
        {
            set
            {
                efekt.Parameters["materialPower"].SetValue(value);
            }
        }
        public float specularIntensity
        {
            set
            {
                efekt.Parameters["specularIntensity"].SetValue(value);
            }
        }
        public float pointLightFalloff
        {
            set
            {
                efekt.Parameters["pointLightFalloff"].SetValue(value);
            }
        }
        public float pointLightRange
        {
            set
            {
                efekt.Parameters["pointLightRange"].SetValue(value);
            }
        }
        public Vector3 pointLightPos
        {
            set
            {
                efekt.Parameters["pointLightPos"].SetValue(value);
            }
        }
        public Vector4 pointLightColor
        {
            set
            {
                efekt.Parameters["pointLightColor"].SetValue(value);
            }
        }

        public Shader(Effect ef)
        {
            efekt = ef;
        }
    }
}
