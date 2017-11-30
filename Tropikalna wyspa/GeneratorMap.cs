using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tropikalna_wyspa
{
    class GeneratorMap
    {
        public static HeightMapTerrain ZrobParabolicznaWyspe(GraphicsDevice gd, int size)
        {
            return new HeightMapTerrain(gd, 1, GenerujMapeWysokosci(size), GenerujMapeNormali(size));
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

        private static Vector3[][] GenerujMapeNormali(int size)
        {
            Vector3[][] normale = new Vector3[size][];
            for (int i = 0; i < size; i++)
            {
                normale[i] = new Vector3[size];
                for (int j = 0; j < size; j++)
                {
                    normale[i][j] = PodajNormal(i, j, size / 2);
                }
            }
            return normale;
        }

        private static Vector3 PodajNormal(int x, int y, int offset)
        {
            float X = (float)x - (float)offset;
            float Y = (float)y - (float)offset;
            var ret = Vector3.Normalize(new Vector3(1*X/20,1,1*Y/20));
            return ret;
        }

        private static float PodajWysokosc(int x, int y, int offset)
        {
            float X = (float)x-(float)offset;
            float Y = (float)y-(float)offset;
            return -1*(X * X + Y * Y) / 40;
        }

        public static HeightMapTerrain ZrobMorze(GraphicsDevice gd, int size)
        {
            float[][] mapa = new float[size][];
            for (int i = 0; i < size; i++)
            {
                mapa[i] = new float[size];
                for (int j = 0; j < size; j++)
                {
                    mapa[i][j] = 0.0f;
                }
            }
            Vector3[][] normale = new Vector3[size][];
            for (int i = 0; i < size; i++)
            {
                normale[i] = new Vector3[size];
                for (int j = 0; j < size; j++)
                {
                    normale[i][j] = new Vector3(0.0f, 1.0f, 0.0f);
                }
            }
            return new HeightMapTerrain(gd, 15, mapa, normale);
        }
    }
}
