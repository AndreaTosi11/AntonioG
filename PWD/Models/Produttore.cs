using System;
using System.Collections.Generic;

#nullable disable

namespace ProjectWorkDemo.Models
{
    public partial class Produttore
    {
        public Produttore()
        {
            Mezzo = new HashSet<Mezzo>();
        }

        public int IdProduttore { get; set; }
        public string Denominazione { get; set; }

        public virtual ICollection<Mezzo> Mezzo { get; set; }
    }
}
