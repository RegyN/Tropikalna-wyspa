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
        Dictionary<String, Model> modele;

        public void DodajModel(Model mod, string nazwa)
        {
            modele.Add(nazwa, mod);
        }
        public Model PobierzModel(string nazwa)
        {
            modele.TryGetValue(nazwa, out Model wynik);
            return wynik;
        }
        private static readonly ResourceManager instance = new ResourceManager();
        public static ResourceManager Instance
        {
            get
            {
                return instance;
            }
        }
        private ResourceManager()
        {
            modele = new Dictionary<string, Model>();
        }
    }
}
