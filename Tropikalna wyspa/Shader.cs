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
    public class Shader
    {
        public Effect efekt;
        public Vector2 Displacement
        {
            set
            {
                if (efekt.Parameters["displacement"] != null)
                {
                    efekt.Parameters["displacement"].SetValue(value); 
                }
            }
        }
        public float FogEnabled
        {
            set
            {
                if (efekt.Parameters["fogEnabled"] != null)
                {
                    if (value > 1.0f)
                        value = 1.0f;
                    else if (value < 0.0f)
                        value = 0.0f;
                    efekt.Parameters["fogEnabled"].SetValue(value); 
                }
            }
            get { return efekt.Parameters["fogEnabled"].GetValueSingle(); }
        }
        public float FogStart
        {
            set
            {
                if (efekt.Parameters["fogStart"] != null)
                {
                    if (value < 0.0f)
                        value = 0.0f;
                    efekt.Parameters["fogStart"].SetValue(value); 
                }
            }
        }
        public float FogEnd
        {
            set
            {
                if (efekt.Parameters["fogEnd"] != null)
                {
                    if (value < 0.0f)
                        value = 0.0f;
                    efekt.Parameters["fogEnd"].SetValue(value); 
                }
            }
        }
        public Texture2D PrimaryTex
        {
            set
            {
                if (efekt.Parameters["PrimaryTex"] != null)
                {
                    efekt.Parameters["PrimaryTex"].SetValue(value); 
                }
            }
        }
        public Texture2D SecondaryTex
        {
            set
            {
                if (efekt.Parameters["SecondaryTex"] != null)
                {
                    efekt.Parameters["SecondaryTex"].SetValue(value);
                }
            }
        }
        public Matrix viewMatrix
        {
            set
            {
                if (efekt.Parameters["ViewMatrix"] != null)
                {
                    efekt.Parameters["ViewMatrix"].SetValue(value); 
                }
            }
        }
        public Matrix worldMatrix
        {
            set
            {
                if (efekt.Parameters["WorldMatrix"] != null)
                {
                    efekt.Parameters["WorldMatrix"].SetValue(value); 
                }
            }
        }
        public Matrix projectionMatrix
        {
            set
            {
                if (efekt.Parameters["ProjectionMatrix"] != null)
                {
                    efekt.Parameters["ProjectionMatrix"].SetValue(value); 
                }
            }
        }
        public Matrix WorldInverseTransposeMatrix
        {
            set
            {
                if (efekt.Parameters["WorldInvTransMat"] != null)
                {
                    efekt.Parameters["WorldInvTransMat"].SetValue(value); 
                }
            }
        }
        public Color diffuseColor
        {
            set
            {
                if (efekt.Parameters["surfaceColor"] != null)
                {
                    Vector4 a = new Vector4(value.R / 255f, value.G / 255f, value.B / 255f, value.A / 255f);
                    efekt.Parameters["surfaceColor"].SetValue(a); 
                }
            }
        }
        public Vector3 viewPosition
        {
            set
            {
                if (efekt.Parameters["ViewPosition"] != null)
                {
                    efekt.Parameters["ViewPosition"].SetValue(value); 
                }
            }
        }
        public Vector3 diffuseLightDirection
        {
            set
            {
                if (efekt.Parameters["dirLightDir"] != null)
                {
                    efekt.Parameters["dirLightDir"].SetValue(value); 
                }
            }
        }
        public Color diffuseLightColor
        {
            set
            {
                if (efekt.Parameters["dirLightColor"] != null)
                {
                    Vector4 a = new Vector4(value.R / 255f, value.G / 255f, value.B / 255f, value.A / 255f);
                    efekt.Parameters["dirLightColor"].SetValue(a); 
                }
            }
        }
        public Vector3 materialEmissive
        {
            set
            {
                if (efekt.Parameters["materialEmissive"] !=null)
                {
                    efekt.Parameters["materialEmissive"].SetValue(value); 
                }
            }
        }
        public Vector3 materialAmbient
        {
            set
            {
                if (efekt.Parameters["materialAmbient"] != null)
                {
                    efekt.Parameters["materialAmbient"].SetValue(value); 
                }
            }
        }
        public Vector3 materialDiffuse
        {
            set
            {
                if (efekt.Parameters["materialDiffuse"] != null)
                {
                    efekt.Parameters["materialDiffuse"].SetValue(value); 
                }
            }
        }
        public Vector3 materialSpecular
        {
            set
            {
                if (efekt.Parameters["materialSpecular"] != null)
                {
                    efekt.Parameters["materialSpecular"].SetValue(value);
                }
            }
        }
        public float materialPower
        {
            set
            {
                if (efekt.Parameters["materialPower"] != null)
                {
                    efekt.Parameters["materialPower"].SetValue(value); 
                }
            }
        }
        public float specularIntensity
        {
            set
            {
                if (efekt.Parameters["specularIntensity"] != null)
                {
                    efekt.Parameters["specularIntensity"].SetValue(value); 
                }
            }
        }
        public float pointLightFalloff
        {
            set
            {
                if (efekt.Parameters["pointLightFalloff"] != null)
                {
                    efekt.Parameters["pointLightFalloff"].SetValue(value); 
                }
            }
        }
        public float pointLightRange
        {
            set
            {
                if (efekt.Parameters["pointLightRange"] != null)
                {
                    efekt.Parameters["pointLightRange"].SetValue(value); 
                }
            }
        }
        public Vector3 pointLightPos
        {
            set
            {
                if (efekt.Parameters["pointLightPos"] != null)
                {
                    efekt.Parameters["pointLightPos"].SetValue(value); 
                }
            }
        }
        public Vector4 pointLightColor
        {
            set
            {
                if (efekt.Parameters["pointLightColor"] != null)
                {
                    efekt.Parameters["pointLightColor"].SetValue(value); 
                }
            }
        }

        public Shader(Effect ef)
        {
            efekt = ef;
        }
    }
}
