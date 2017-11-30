using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Tropikalna_wyspa
{
    class ReflectionSphere
    {
        private Model model;
        private TextureCube skyBoxTexture;
        private Effect efekt;
        private float size;
        private Vector3 position;

        public ReflectionSphere(string skyboxTexture, ContentManager Content, Vector3 pos, float size = 3.0f)
        {
            model = Content.Load<Model>("sphere");
            skyBoxTexture = Content.Load<TextureCube>(skyboxTexture);
            efekt = Content.Load<Effect>("Reflection");
            position = pos;
            this.size = size;
        }

        public void Draw(Matrix view, Matrix projection, Vector3 cameraPosition)
        {
            // Go through each pass in the effect, but we know there is only one...
            foreach (EffectPass pass in efekt.CurrentTechnique.Passes)
            {
                // Draw all of the components of the mesh, but we know the cube really
                // only has one mesh
                foreach (ModelMesh mesh in model.Meshes)
                {
                    // Assign the appropriate values to each of the parameters
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        part.Effect = efekt;
                        var world = Matrix.CreateScale(size) * Matrix.CreateTranslation(position);
                        part.Effect.Parameters["World"].SetValue(world);
                        part.Effect.Parameters["View"].SetValue(view);
                        part.Effect.Parameters["Projection"].SetValue(projection);
                        part.Effect.Parameters["SkyboxTexture"].SetValue(skyBoxTexture);
                        part.Effect.Parameters["CameraPosition"].SetValue(cameraPosition);
                        part.Effect.Parameters["WorldInverseTranspose"].SetValue(Matrix.Transpose(Matrix.Invert(world)));
                    }

                    // Draw the mesh with the skybox effect
                    mesh.Draw();
                }
            }
        }
    }
}
