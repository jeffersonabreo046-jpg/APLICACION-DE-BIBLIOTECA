using APLICACION_DE_BIBLIOTECA.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace APLICACION_DE_BIBLIOTECA.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
   

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Usuarios()
        {
            return View();
        }
        public IActionResult Prestamos()
        {
            return View();
        }
        public IActionResult AcercaDe()
        {
            return View();
        }

        [AllowAnonymous]


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
