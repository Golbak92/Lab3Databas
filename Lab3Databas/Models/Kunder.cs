using System;
using System.Collections.Generic;

#nullable disable

namespace Lab3Databas
{
    public partial class Kunder
    {
        public Kunder()
        {
            OrderHuvuds = new HashSet<OrderHuvud>();
        }

        public int KundId { get; set; }
        public string Förnamn { get; set; }
        public string Efternamn { get; set; }
        public string TelefonNr { get; set; }
        public string Email { get; set; }

        public virtual ICollection<OrderHuvud> OrderHuvuds { get; set; }
    }
}
