using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ProjectWorkDemo.Areas.Identity.Data;

// Add profile data for application users by adding properties to the ProjectWorkDemoUser class
public class ProjectWorkDemoUser : IdentityUser
{
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

    //[DataType(DataType.PhoneNumber)]
    //[Display(Name = "Phone Number")]
    //[Required(ErrorMessage = "Telefono Required!")]
    //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered telefono format is not valid.")]
    public string Telefono { get; set; }
}

