﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tropikalna_wyspa
{
    class Skrzynka : Object3D
    {
        public Skrzynka(Microsoft.Xna.Framework.Content.ContentManager cm, Vector3 poz, Vector3 up, Vector3 forward, float scale = 0.025f)
            :base(cm.Load<Model>("Skrzynka"), poz, up, forward, scale)
        {
            foreach (var mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.DiffuseColor = Color.SaddleBrown.ToVector3()/2f;
                }
            }
        }
    }
}
