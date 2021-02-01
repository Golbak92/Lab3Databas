using System;
using System.Collections.Generic;

#nullable disable

namespace Lab3Databas
{
    public partial class Butiker
    {
        public Butiker()
        {
            Lagersaldos = new HashSet<Lagersaldo>();
        }

        public int ButiksId { get; set; }
        public string Butiksnamn { get; set; }
        public string Stad { get; set; }
        public string Adress { get; set; }
        public string TelefonNr { get; set; }

        public virtual ICollection<Lagersaldo> Lagersaldos { get; set; }
    }
}
