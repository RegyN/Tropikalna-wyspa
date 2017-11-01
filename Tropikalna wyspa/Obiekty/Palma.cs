using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tropikalna_wyspa
{
    class Palma : Object3D
    {
        public Palma(Microsoft.Xna.Framework.Content.ContentManager cm, Vector3 poz, Vector3 up, Vector3 forward, float scale = 1f)
            :base(cm.Load<Model>("Palma"), poz, up, forward, scale)
        {

        }
    }
}
