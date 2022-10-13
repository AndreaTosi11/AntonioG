using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace ProjectWorkDemo.Models
{
    public partial class Dettaglio
    {
        public int IdDettaglio { get; set; }
        [Display(Name = "Codice lavorazione")]
        public int IdLavorazione { get; set; }
        [Display(Name = "Descrizione della lavorazione")]
        [Required(ErrorMessage = "Inserire la descrizione della lavorazione")]
        public string TipoLavorazione { get; set; }
        [Required(ErrorMessage = "Inserire la data d'inizio lavoro")]
        [Display(Name = "Inizio Lavoro")]
        public DateTime DataInizioLavoro { get; set; }
        [Required(ErrorMessage = "Inserire la data di fine lavoro")]
        [Display(Name = "Fine Lavoro")]
        public DateTime DataFineLavoro { get; set; }
        [Required(ErrorMessage = "Inserire Il nome del lavoratore")]
        [Display(Name = "Nome")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Il campo non supporta numeri o caratteri speciali")]
         [StringLength(20)]
        public string NomeLavoratore { get; set; }
        [Required(ErrorMessage = "Inserire Il Cognome del lavoratore")]
        [Display(Name = "Cognome")]
        [RegularExpression ("^[a-zA-Z]+$", ErrorMessage = "Il campo non supporta numeri o caratteri speciali")]
        [StringLength(20)]
        public string CognomeLavoratore { get; set; }
        [Required(ErrorMessage = "Inserire il costo netto")]
        [Display(Name = "Costo Netto")]
        public decimal CostoNetto { get; set; }
        [Required(ErrorMessage = "Inserire l'iva")]
        public int Iva { get; set; }
        [Display(Name = "Costo Totale")]
        public decimal? CostoTotDet { get; set; }
        [Display(Name = "Codice lavorazione")]
        public virtual Lavorazione IdLavorazioneNavigation { get; set; }
    }
}
