using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace ProjectWorkDemo.Models
{
    public partial class ServiziFornitore
    {
        public int IdServizio { get; set; }
        [Display(Name = "Nome Ditta")]
        public int IdFornitori { get; set; }
        [Display(Name = "Tipo fornitore")]
        public int IdTipoForn { get; set; }

        public virtual Fornitori IdFornitoriNavigation { get; set; }
        public virtual TipoForn IdTipoFornNavigation { get; set; }
    }
}
