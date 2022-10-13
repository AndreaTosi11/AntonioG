using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace ProjectWorkDemo.Models
{
    public partial class Lavorazione
    {
        public Lavorazione()
        {
            Dettaglio = new HashSet<Dettaglio>();
        }

        [Display(Name = "Codice lavorazione")]
        public int IdLavorazione { get; set; }
        [Display(Name = "Fornitore")]
        public int IdFornitori { get; set; }
        [Display(Name = "Targa")]
        public int IdMezzo { get; set; }
        [Required(ErrorMessage = "Inserire Codice  Identificativo")]
        [StringLength(9)]
        [Display(Name = "Codice identificativo")]
        [RegularExpression(@"^[a-zA-Z]{4}?[0-9]{5}$", ErrorMessage = "Inserire un codice alfanumerico composto da le prime 4 lettere del Fornitore e 5 cifre.")]
        public string CodiceIdentificativo { get; set; }

        [Required(ErrorMessage = "Inserire la data della Lavorazione")]
        [Display(Name = "Data Lavorazione")]
        public DateTime DataLavorazione { get; set; }
        public bool Garanzia { get; set; }     
        [Display(Name = "Codice Garanzia")]
        [RegularExpression("[+-]?([0-9]+([.][0-9]*)?|[.][0-9]+)", ErrorMessage = "Il campo garanzia deve essere un numero")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "Il numero di garanzia deve avere 5 numeri")]
        //[RequiredIf("Garanzia", ErrorMessage = "Inserire il numero di garanzia")]
        public string NumGaranzia { get; set; }
        public decimal CostoTotaleLavorazione { get; set; }
        [Display(Name = "Fornitore")]
        public virtual Fornitori IdFornitoriNavigation { get; set; }
        [Display(Name = "Targa")]
        public virtual Mezzo IdMezzoNavigation { get; set; }
        public virtual ICollection<Dettaglio> Dettaglio { get; set; }
    }


    //public class RequiredIfAttribute : RequiredAttribute
    //{
    //    private String PropertyName { get; set; }

    //    public RequiredIfAttribute(String propertyName)
    //    {
    //        PropertyName = propertyName;
    //    }

    //    protected override ValidationResult IsValid(object value, ValidationContext context)
    //    {
    //        Object instance = context.ObjectInstance;
    //        Type type = instance.GetType();
    //        Object proprtyvalue = type.GetProperty(PropertyName).GetValue(instance, null);
    //        if (proprtyvalue.ToString() == "true")
    //        {
    //            ValidationResult result = base.IsValid(value, context);
    //            return result;
    //        }
    //        return ValidationResult.Success;
    //    }
    //}
}
