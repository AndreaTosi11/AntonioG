using System;
using System.Collections.Generic;

#nullable disable

namespace ProjectWorkDemo.Models
{
    public partial class TipoForn
    {
        public TipoForn()
        {
            ServiziFornitore = new HashSet<ServiziFornitore>();
        }

        public int IdTipoForn { get; set; }
        public string Descrizione { get; set; }
        public virtual ICollection<ServiziFornitore> ServiziFornitore { get; set; }
    }
}
