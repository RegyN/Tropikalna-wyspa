#region Usingi
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

namespace Primitives3D
{
    public class SquarePrimitive : GeometricPrimitive
    {
        public SquarePrimitive(GraphicsDevice graphicsDevice)
            :this(graphicsDevice, 1, Color.CornflowerBlue)
        {}

        public SquarePrimitive(GraphicsDevice graphicsDevice, float size, Color color)
        {
            // Plane ma dwie ścianki, więc każdej potrzebny jest wektor normalny.
            Vector3[] normals =
            {
                new Vector3(0,1,0),
                new Vector3(0,-1,0)
            };

            foreach (var normal in normals)
            {
                // Dwa wektory pokazujące kierunki krawędzi ścianki
                Vector3 side1 = new Vector3(normal.Y, normal.Z, normal.X);
                Vector3 side2 = Vector3.Cross(normal, side1);

                // Dwa trójkąty na ściankę
                AddIndex(CurrentVertex + 0);
                AddIndex(CurrentVertex + 1);
                AddIndex(CurrentVertex + 2);

                AddIndex(CurrentVertex + 0);
                AddIndex(CurrentVertex + 2);
                AddIndex(CurrentVertex + 3);

                // Cztery narożniki na ściankę
                AddVertex((Vector3.Zero - side1 - side2) * size / 2, normal, color);
                AddVertex((Vector3.Zero - side1 + side2) * size / 2, normal, color);
                AddVertex((Vector3.Zero + side1 + side2) * size / 2, normal, color);
                AddVertex((Vector3.Zero + side1 - side2) * size / 2, normal, color);
            }
            InitializePrimitive(graphicsDevice);
        }
    }
}
