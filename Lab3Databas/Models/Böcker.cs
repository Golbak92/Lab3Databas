using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable

namespace Lab3Databas
{
    [DebuggerDisplay("{Bookname}")]

    public partial class Böcker
    {
        public Böcker()
        {
            Lagersaldos = new HashSet<Lagersaldo>();
            OrderRaders = new HashSet<OrderRader>();
        }

        public string Isbn { get; set; }
        public string Titel { get; set; }
        public string Språk { get; set; }
        public decimal? Pris { get; set; }
        public DateTime? Utgivningsdatum { get; set; }
        public int? FörfattareId { get; set; }

        public virtual Författare Författare { get; set; }
        public virtual ICollection<Lagersaldo> Lagersaldos { get; set; }
        public virtual ICollection<OrderRader> OrderRaders { get; set; }

        public object This { get { return this; } }
    }
}
