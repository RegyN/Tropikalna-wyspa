using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tropikalna_wyspa
{
    class Krysztal : Object3D
    {
        public Krysztal(Microsoft.Xna.Framework.Content.ContentManager cm, Vector3 poz, Vector3 up, Vector3 forward, float scale = 0.1f)
            : base(cm.Load<Model>("Krysztal"), poz, up, forward, scale)
        {
            shader = new Shader(cm.Load<Effect>("NoTexturePhong").Clone());
            shader.efekt.CurrentTechnique = shader.efekt.Techniques["NoTex"];
            shader.worldMatrix = worldMatrix;
            shader.WorldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(worldMatrix));
            shader.diffuseColor = Color.White;
            shader.diffuseLightColor = Color.White;
            shader.materialEmissive = new Vector3(0f, 0f, 0f);
            shader.materialAmbient = new Vector3(.1f, .1f, .1f);
            shader.materialDiffuse = Color.LightYellow.ToVector3();
            shader.materialSpecular = Color.LightYellow.ToVector3();
            shader.materialPower = 50f;
            shader.specularIntensity = 1f;
            shader.diffuseColor = Color.Gray;
        }
    }
}