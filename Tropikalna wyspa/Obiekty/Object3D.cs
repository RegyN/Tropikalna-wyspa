using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tropikalna_wyspa
{
    // Klasa opisująca pojedynczy graficzny obiekt 3D wczytany z pliku zewnętrznego
    class Object3D
    {
        public Model model { get; set; }
        public Matrix worldMatrix
        {
            get
            {
                return Matrix.CreateScale(scale) * Matrix.CreateWorld(position, forwardDirection, upDirection);
            }
        }
        public float scale;
        public Vector3 position;
        public Vector3 upDirection;
        public Vector3 forwardDirection;
        public Shader shader;

        public Object3D(Model mod, Vector3 poz)
            :this(mod, poz, Vector3.Up, Vector3.Forward)
        {}

        public Object3D(Model mod, Vector3 poz, Vector3 up, Vector3 forward, float sc = 1)
        {
            scale = sc;
            model = mod;
            forwardDirection = forward;
            upDirection = up;
            position = poz;
        }

        public void Draw()
        {
            for (int i = 0; i < model.Meshes.Count; i++)
            {
                ModelMesh mesh = model.Meshes[i];
                foreach (var part in mesh.MeshParts)
                {
                    part.Effect = shader.efekt;
                    mesh.Draw();
                }
            }
        }
    }
}
