using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System;

#nullable disable

namespace ProjectWorkDemo.Models
{
    public partial class Fornitori
    {
        public Fornitori()
        {
            Lavorazione = new HashSet<Lavorazione>();
            ServiziFornitore = new HashSet<ServiziFornitore>();
        }

        [Key]
        public int IdFornitori { get; set; }
        public string UserId { get; set; }
        [Required(ErrorMessage = "Inserire il nome della ditta")]
        public string NomeDitta { get; set; }
        //[Required(ErrorMessage = "Inserire la data di creazione")]
        //[YearRange(1900, ErrorMessage = "Inserire una data valida")]
        [Display(Name = "Data Creazione")]
        public DateTime DataCreazione { get; set; }
        [Required(ErrorMessage = "Inserire il numero di telefono")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Il numero di Telefono deve avere 10 numeri")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Inserire solo numeri da 0 a 9")]
        public string Telefono { get; set; }
        [Required(ErrorMessage = "Inserire l'indirizzo")]
        public string Indirizzo { get; set; }

        [Required(ErrorMessage = "Inserire la partita IVA")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Il numero della Partita iva deve avere 11 numeri")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Inserire solo numeri da 0 a 9")]
        [Display(Name = "P. Iva")]
        public string Piva { get; set; }
        [Required(ErrorMessage = "Selezionare un punto sulla mappa")]
        public string Latitudine { get; set; }
        [Required(ErrorMessage = "Selezionare un punto sulla mappa")]
        public string Longitudine { get; set; }

        public virtual AspNetUser User { get; set; }
        public virtual ICollection<Lavorazione> Lavorazione { get; set; }
        public virtual ICollection<ServiziFornitore> ServiziFornitore { get; set; }
    }

    //public class YearRangeAttribute : RangeAttribute
    //{
    //    public YearRangeAttribute(int minimum) : base(minimum, DateTime.Now.Year) { }
      
    //}
}

      
