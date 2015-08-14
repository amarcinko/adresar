using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adresar.Models
{
    public class Kontakt
    {
        public int ID { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Grad { get; set; }
        public string Slika { get; set; }
        public string Opis { get; set; }

        public virtual ICollection<Unos> Brojevi { get; set; }
    }
}
