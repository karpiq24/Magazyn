using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazyn
{
    [Serializable()]  
    public class towar
    {
        public string indeks, nazwa,uwagi;
        public double brutto, netto, ostatni;
        public uint stan;
    }
}
