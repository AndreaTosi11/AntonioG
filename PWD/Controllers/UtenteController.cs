using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace ProjectWorkDemo.Controllers
{
    public class UtenteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CreateUser()
        {
            return View();
        }
        public IActionResult DetailsUser()
        {
            return View();
        }
        public IActionResult EditUser()
        {
            return View();
        }
    }
}
