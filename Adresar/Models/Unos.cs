using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adresar.Models
{
    public class Unos
    {
        public int ID { get; set; }
        public string Broj { get; set; }
        public Tip Tip { get; set; }
        [DisplayName("Opis broja")]
        public string Opis { get; set; }
        [ForeignKey("Kontakt")]
        public int KontaktId { get; set; }

        public virtual Kontakt Kontakt { get; set; }
    }

    public enum Tip
    {
        Mobitel,
        Kucni,
        Posao
    }
}
