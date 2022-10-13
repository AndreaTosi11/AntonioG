using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using ProjectWorkDemo.Models;
using System.Diagnostics;
using System;

namespace ProjectWorkDemo.Controllers
{
    [Route("language")]
    public class LanguageController : Controller
    {
        [Route("change")]
        public IActionResult Change(string culture)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddMonths(1)
                });
            return RedirectToAction("index", "home");
        }
    }
}