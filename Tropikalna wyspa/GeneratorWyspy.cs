using Microsoft.Xna.Framework.Graphics;
using Primitives3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tropikalna_wyspa
{
    class GeneratorWyspy
    {
        public static HeightMapTerrain ZrobWyspe(GraphicsDevice gd, int size)
        {
            return new HeightMapTerrain(gd, 1, GenerujMapeWysokosci(size));
        }

        private static float[][] GenerujMapeWysokosci(int size)
        {
            float[][] mapa = new float[size][];
            for(int i = 0; i<size; i++)
            {
                mapa[i] = new float[size];
                for(int j = 0; j<size; j++)
                {
                    mapa[i][j] = PodajWysokosc(i, j, size / 2);
                }
            }
            return mapa;
        }

        private static float PodajWysokosc(int x, int y, int offset)
        {
            float X = (float)x-(float)offset;
            float Y = (float)y-(float)offset;
            return -1*(X * X + Y * Y) / 20;
        }
    }
}
