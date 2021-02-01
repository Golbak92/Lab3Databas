using System;
using System.Collections.Generic;

#nullable disable

namespace Lab3Databas
{
    public partial class OrderHuvud
    {
        public OrderHuvud()
        {
            OrderRaders = new HashSet<OrderRader>();
        }

        public int Ordernr { get; set; }
        public string Leveranstatus { get; set; }
        public DateTime? Beställningsdatum { get; set; }
        public int? KundId { get; set; }

        public virtual Kunder Kund { get; set; }
        public virtual ICollection<OrderRader> OrderRaders { get; set; }
    }
}
