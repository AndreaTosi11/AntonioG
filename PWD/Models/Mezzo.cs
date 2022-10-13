using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace ProjectWorkDemo.Models
{
    public partial class Mezzo
    {

        public Mezzo()
        {
            Lavorazione = new HashSet<Lavorazione>();
        }

        public int IdMezzo { get; set; }       
        [Display(Name = "Produttore")]
        public int IdProduttore { get; set; }
        [Required(ErrorMessage = "Inserire il modello dell'auto")]
        [Display(Name = "Modello")]
        public string TipoAuto { get; set; }
        [Required(ErrorMessage = "Inserire la targa")]
        [StringLength(7, ErrorMessage ="7 caratteri alfanumerici necessari")]
        [RegularExpression(@"^[a-zA-Z]{2}?[0-9]{3}?[a-zA-Z]{2}$", ErrorMessage = "Formato targa: 'AA000AA'")]
        public string Targa { get; set; }
        [Required(ErrorMessage = "Inserire la data d'immatricolazione")]
        [Display(Name = "Data Immatricolazione")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DataImmatricolazione { get; set; }
        [Display(Name = "Produttore")]
        public virtual Produttore IdProduttoreNavigation { get; set; }
        public virtual ICollection<Lavorazione> Lavorazione { get; set; }
    }
}
