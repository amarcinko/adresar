using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adresar.Models
{
    public class KontaktViewModel
    {
        public Kontakt Kontakt { get; set; }
        public Unos Unos { get; set; }
    }

    public class KontaktUnosViewModel
    {
        public Kontakt Kontakt { get; set; }
        public IList<Unos> Unosi { get; set; }
        public Unos PrazanUnos { get; set; }
    }
}
