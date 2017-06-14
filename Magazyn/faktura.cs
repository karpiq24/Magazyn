using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazyn
{
    public struct rzecz
    {
        public string indeks;
        public uint ile;
        public double cena;
    }

    [Serializable()]  
    public class faktura
    {
        public DateTime data;
        public string nr_faktury;
        public string dostawca;
        public double wartosc;
        public List<rzecz> przedmioty;
    }
}
