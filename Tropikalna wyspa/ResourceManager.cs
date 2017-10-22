using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tropikalna_wyspa
{
    class ResourceManager
    {
        private static readonly ResourceManager instance = new ResourceManager();
        public static ResourceManager Instance
        {
            get
            {
                return instance;
            }
        }
        private ResourceManager()
        {}
        
    }
}
