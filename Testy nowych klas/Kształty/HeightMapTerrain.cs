﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Primitives3D
{
    public class HeightMapTerrain : GeometricPrimitive
    {
        public HeightMapTerrain(GraphicsDevice graphicsDevice)
            : this(graphicsDevice, 1, 
                      new float[][]{
                          new float[]{0f,0f},
                          new float[]{0f,0f}
                      },
                      new Vector3[][]{
                          new Vector3[]{ new Vector3(0, 1, 0), new Vector3(0, 1, 0) },
                          new Vector3[]{ new Vector3(0, 1, 0), new Vector3(0, 1, 0) }
                      },
                      Color.SandyBrown)
        { }

        public HeightMapTerrain(GraphicsDevice graphicsDevice, float size, float[][] heightMap, Vector3[][] normalMap, Color color)
        {
            for(int i = 0; i<heightMap.Length-1; i++)
            {
                for (int j = 0; j < heightMap[0].Length - 1; j++)
                {
                    // Wektory pokazujące kierunki krawędzi zewnętrznych kwadratu
                    Vector3 sideLeft = new Vector3(1f, heightMap[i+1][j] - heightMap[i][j], 0f);
                    Vector3 sideRight = new Vector3(1f, heightMap[i+1][j+1] - heightMap[i][j+1], 0f);
                    Vector3 sideUp = new Vector3(0f, heightMap[i][j+1] - heightMap[i][j], 1f);
                    Vector3 sideDown = new Vector3(0f, heightMap[i+1][j+1] - heightMap[i+1][j], 1f);

                    // Wektory normalne obu trójkątów
                    Vector3 normal1 = Vector3.Cross(sideLeft, sideDown);
                    Vector3 normal2 = Vector3.Cross(sideRight, sideUp);

                    // Dodaje dwie trojkatne scianki
                    AddIndex(CurrentVertex + 0);
                    AddIndex(CurrentVertex + 1);
                    AddIndex(CurrentVertex + 2);

                    AddIndex(CurrentVertex + 3);
                    AddIndex(CurrentVertex + 4);
                    AddIndex(CurrentVertex + 5);

                    Vector3 upperLeftPos = new Vector3(i, heightMap[i][j], j);

                    AddVertex(upperLeftPos, normalMap[i][j], color);
                    AddVertex(upperLeftPos + sideLeft, normalMap[i+1][j], color);
                    AddVertex(upperLeftPos + sideLeft + sideDown, normalMap[i+1][j+1], color);
                    AddVertex(upperLeftPos, normalMap[i][j], color);
                    AddVertex(upperLeftPos + sideUp + sideRight, normalMap[i+1][j+1], color);
                    AddVertex(upperLeftPos + sideUp, normalMap[i][j+1], color);
                }
                InitializePrimitive(graphicsDevice);
            }
        }
    }
}
