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
        public Matrix worldMatrix { get; private set; }

        public Object3D(Model mod, Vector3 poz)
        {
            model = mod;
            worldMatrix = Matrix.CreateWorld(poz, Vector3.Forward, Vector3.Up);
        }
    }
}
