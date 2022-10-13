using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ProjectWorkDemo.Models
{
    public class Registrazione
    {
        //[Display(Name = "ID")]
        public int IdUtente { get; set; }
        //[Required(ErrorMessage = "Please enter Nome")]
        //[Display(Name = "Nome")]
        public string Nome { get; set; }
        //[Required(ErrorMessage = "Please enter Cognome")]
        //[Display(Name = "Cognome")]
        public string Cognome { get; set; }
        //[Required(ErrorMessage = "Please enter password")]
        //[DataType(DataType.Password)]
        //[StringLength(100, ErrorMessage = "Password \"{0}\" must have {2} character", MinimumLength = 8)]
        //[RegularExpression(@"^([a-zA-Z0-9@*#]{8,15})$", ErrorMessage = "Password must contain: Minimum 8 characters atleast 1 UpperCase Alphabet, 1 LowerCase Alphabet, 1 Number and 1 Special Character")]
        public string Password { get; set; }
        //[Display(Name = "Confirm password")]
        //[Required(ErrorMessage = "Please enter confirm password")]
        //[Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
        //[DataType(DataType.Password)]
        public string ConfermaPw { get; set; }
        //[Required]
        //[RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail id is not valid")]
        public string Email { get; set; }
        //[DataType(DataType.PhoneNumber)]
        //[Display(Name = "Phone Number")]
        //[Required(ErrorMessage = "Telefono Required!")]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered telefono format is not valid.")]
        public string Telefono { get; set; }
        public List<Registrazione> InfoIscrizione { get; set; }
    }
}
