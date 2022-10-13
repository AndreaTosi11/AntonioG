using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal.ExternalLoginModel;
using Microsoft.AspNetCore.Authentication;

namespace ProjectWorkDemo.Models
{
    public class FornitoriRegister
    {

        public int IdFornitori { get; set; }   
        public string RoleId { get; set; } = null!;
        [Required(ErrorMessage = "Inserire il nome della ditta")]
        public string NomeDitta { get; set; } = null!;
        //[Required(ErrorMessage = "Inserire la data di creazione")]
        //[YearRange(1900, ErrorMessage = "Inserire una data valida")]
        //[Display(Name = "Data Creazione")]
        public DateTime DataCreazione { get; set; }
        [Required(ErrorMessage = "Inserire il numero di telefono")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Il numero di Telefono deve avere 10 numeri")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Inserire solo numeri da 0 a 9")]

        public string Telefono { get; set; } = null!;
        [Required(ErrorMessage = "Inserire l'indirizzo")]
        public string Indirizzo { get; set; } = null!;

        [Required(ErrorMessage = "Inserire la partita IVA")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Il numero della Partita iva deve avere 11 numeri")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Inserire solo numeri da 0 a 9")]
        [Display(Name = "P. Iva")]
        public string Piva { get; set; } = null!;
        [Required(ErrorMessage = "Selezionare un punto sulla mappa")]
        public string Latitudine { get; set; } = null!;
        [Required(ErrorMessage = "Selezionare un punto sulla mappa")]
        public string Longitudine { get; set; } = null!;
        public int IdUtente { get; set; }
        [Required(ErrorMessage = "Please enter Nome")]
        [Display(Name = "Nome")]
        public string Nome { get; set; } = null!;
        [Required(ErrorMessage = "Please enter Cognome")]
        [Display(Name = "Cognome")]
        public string Cognome { get; set; } = null!;
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(100, ErrorMessage = "min length 6 letters, at least 1 upper letter, a number and a non-alphanumeric symbol", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfermaPw { get; set; }

    }

    //public class YearRangeAttribute : RangeAttribute
    //{
    //    public YearRangeAttribute(int minimum) : base(minimum, DateTime.Now) { }

    //}
}
