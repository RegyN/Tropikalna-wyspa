
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tropikalna_wyspa
{
    class Drogowskaz : Object3D
    {
        public Drogowskaz(Microsoft.Xna.Framework.Content.ContentManager cm, Vector3 poz, Vector3 up, Vector3 forward, float scale = 0.001f)
            : base(cm.Load<Model>("Drogowskaz"), poz, up, forward, scale)
        {
            shader = new Shader(cm.Load<Effect>("TexturePhong").Clone());
            shader.efekt.Parameters["textureImage"].SetValue(cm.Load<Texture>("DrogowskazTex"));
            shader.worldMatrix = worldMatrix;
            shader.WorldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(worldMatrix));
            shader.materialEmissive = new Vector3(0f, 0f, 0f);
            shader.materialAmbient = new Vector3(.1f, .1f, .1f);
            shader.materialDiffuse = Color.SaddleBrown.ToVector3();
            shader.materialSpecular = Color.SaddleBrown.ToVector3();
            shader.materialPower = 50f;
            shader.specularIntensity = 1f;
            shader.diffuseColor = Color.SaddleBrown;
        }
    }
}
