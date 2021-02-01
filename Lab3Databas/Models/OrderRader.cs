using System;
using System.Collections.Generic;

#nullable disable

namespace Lab3Databas
{
    public partial class OrderRader
    {
        public int Ordernr { get; set; }
        public int Radnummer { get; set; }
        public string Isbn { get; set; }
        public decimal? Pris { get; set; }
        public int? Antal { get; set; }

        public virtual Böcker IsbnNavigation { get; set; }
        public virtual OrderHuvud OrdernrNavigation { get; set; }
    }
}
