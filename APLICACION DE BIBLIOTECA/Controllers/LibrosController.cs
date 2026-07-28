using Microsoft.AspNetCore.Mvc;
using APLICACION_DE_BIBLIOTECA.Models;

namespace APLICACION_DE_BIBLIOTECA.Controllers
{
    public class LibrosController : Controller
    {
        public IActionResult Index()
        {
            List<Libro> libros = new List<Libro>()
            {
                new Libro
                {
                    ID = 1,
                    Titulo = " Clean Code",
                    Autor = "Kvicha Lars",
                    Categoria =" Programacion",
                    Precio = 32.5M,
                    Disponible = true,

                },
                new Libro{
                
                     ID = 2,
                    Titulo = " Open Source",
                    Autor = "Conor Phillip",
                    Categoria =" Programacion Web 1 ",
                    Precio = 37.90M,
                    Disponible = false,

                },
            };

            return View(libros);
        }
    }
}
