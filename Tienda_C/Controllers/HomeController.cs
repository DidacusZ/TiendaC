using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Tienda_C.Fichero;
using Tienda_C.Models;

namespace Tienda_C.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// vista inicio app
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            try
            {
                FicheroLog.Log("[INFO] [HomeController-Index]");

                HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return View();
            }
            catch (Exception)
            {
                ViewData["error"] = "error";
                FicheroLog.Log("[ERROR] [HomeController-Index]");
                return View();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
