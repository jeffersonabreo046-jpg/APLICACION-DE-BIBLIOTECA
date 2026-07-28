using Microsoft.AspNetCore.Mvc;
using APLICACION_DE_BIBLIOTECA.Models;
using System.Linq;

namespace APLICACION_DE_BIBLIOTECA.Controllers
{
    public class AutoresController : Controller
    {
        private static List<Autor> _autores = new List<Autor>()
        {
            new Autor
            {
                ID = 1,
                Nombre = " Jefferson",
                Apellido = "Abrego",
                Nacionalidad ="Salvadoreño",
                Fecha_De_Nacimiento = new DateTime(2007,8,5),
                Activo = false,
            },
            new Autor
            {
                ID = 2,
                Nombre = "Genesis",
                Apellido = "Marquez",
                Nacionalidad ="Salvadoreño",
                Fecha_De_Nacimiento = new DateTime(2001,2,3),
                Activo = true,
            },
            new Autor
            {
                ID = 3,
                Nombre = " Silvia",
                Apellido = "Escamilla",
                Nacionalidad ="Mexicana",
                Fecha_De_Nacimiento = new DateTime(1997,5,3),
                Activo = true,
            },
            new Autor
            {
                ID = 4,
                Nombre = "Kai",
                Apellido = "Cenat",
                Nacionalidad ="Philipino",
                Fecha_De_Nacimiento = new DateTime(1988,7,30),
                Activo = false,
            },
            new Autor
            {
                ID = 5,
                Nombre = "Duke",
                Apellido = "Dennis",
                Nacionalidad ="Estadounidense",
                Fecha_De_Nacimiento = new DateTime(2002,9,21),
                Activo = true,
            },
        };

        public IActionResult Index()
        {
            return View(_autores);
        }

        public IActionResult Details(int id)
        {

            var autor = _autores.FirstOrDefault(x => x.ID == id);

            if (autor == null)
            {
                return NotFound();
            }

            return View(autor);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Autor autor)
        {
            if (!ModelState.IsValid)
            {
                return View(autor);
            }

            if (_autores.Any())
            {
                autor.ID = _autores.Max(x => x.ID) + 1;
            }
            else
            {
                autor.ID = 1;
            }

            _autores.Add(autor);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {

            var autor = _autores.FirstOrDefault(x => x.ID == id);
            if (autor == null)
                return NotFound();
            return View(autor);

        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Editar(Autor autor)
        {
            if (!ModelState.IsValid) return View(autor);

            var autorExistente = _autores.FirstOrDefault(x => x.ID == autor.ID);
            if (autorExistente != null)
            {
                autorExistente.Nombre = autor.Nombre;
                autorExistente.Apellido = autor.Apellido;
                autorExistente.Nacionalidad = autor.Nacionalidad;
                autorExistente.Fecha_De_Nacimiento = autor.Fecha_De_Nacimiento;
                autorExistente.Activo = autor.Activo;
            }
            return RedirectToAction(nameof(Index));

        }
        
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var autor = _autores.FirstOrDefault(x => x.ID == id);
            if (autor == null) return NotFound();
            return View(autor);
        }

      
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var autor = _autores.FirstOrDefault(x => x.ID == id);
            if (autor != null)
            {
                _autores.Remove(autor);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}