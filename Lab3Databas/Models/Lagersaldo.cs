using System;
using System.Collections.Generic;

#nullable disable

namespace Lab3Databas
{
    public partial class Lagersaldo
    {
        public int ButiksId { get; set; }
        public string Isbn { get; set; }
        public int? Antal { get; set; }

        public virtual Butiker Butiks { get; set; }
        public virtual Böcker IsbnNavigation { get; set; }

        public object This { get { return this; } }
    }
}
