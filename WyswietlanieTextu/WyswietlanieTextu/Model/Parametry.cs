using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WyswietlanieTextu.Model
{
    public class Parametry
    {
        public string Text { get; set; }
       
        public Parametry( string text)
        {
            Text = text;
        }

        public Parametry()
        {
        }
    }
}
