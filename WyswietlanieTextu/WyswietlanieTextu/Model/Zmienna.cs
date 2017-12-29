using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WyswietlanieTextu.Model
{
    public class Zmienna
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public Zmienna( string name, string type)
        {
            Name = name;
            Type = type;
        }
    }
}
