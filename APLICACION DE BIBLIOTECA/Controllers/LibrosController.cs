using Microsoft.AspNetCore.Mvc;
using APLICACION_DE_BIBLIOTECA.Models;

namespace APLICACION_DE_BIBLIOTECA.Controllers
{
    public class LibrosController : Controller
    {
        private readonly IWebHostEnvironment _env;

        public LibrosController(IWebHostEnvironment env)
        {
            _env = env;
        }


        private static List<Libro> _libros = new List<Libro>()
{
    new Libro
    {
        ID = 1,
        Titulo = "Clean Code",
        Autor = "Kvicha Lars",
        Categoria = "Programacion",
        Precio = 32.5M,
        Disponible = true,
        ImagenUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQgic6vSj28uG1ptqwnFuUwFPVKH3bnovlHJrh-C5fmtw&s=10"
    },
    new Libro
    {
        ID = 2,
        Titulo = "Open Source",
        Autor = "Conor Phillip",
        Categoria = "Programacion Web 1",
        Precio = 37.90M,
        Disponible = false,
        ImagenUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTJECMHNxKL1x8PtY6Ex12E8ZXqEqTRLm0gKgGaTs-6ZA&s=10"
    }
};


        public IActionResult Index()
        {
            return View(_libros);
        }

      
        public IActionResult Details(int id)
        {
            var libro = _libros.FirstOrDefault(x => x.ID == id);
            if (libro == null) return NotFound();
            return View(libro);
        }

     
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Libro libro, IFormFile? imagenFile)
        {
            if (ModelState.IsValid)
            {
                if (imagenFile != null && imagenFile.Length > 0)
                {
                    string folder = Path.Combine(_env.WebRootPath, "images");
                    Directory.CreateDirectory(folder);

                    string fileName = Guid.NewGuid().ToString() + "_" + imagenFile.FileName;
                    string filePath = Path.Combine(folder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imagenFile.CopyToAsync(stream);
                    }

                    libro.ImagenUrl = "/images/" + fileName;
                }

                libro.ID = _libros.Any() ? _libros.Max(x => x.ID) + 1 : 1;
                _libros.Add(libro);
                return RedirectToAction(nameof(Index));
            }

            return View(libro);
        }

      
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var libro = _libros.FirstOrDefault(x => x.ID == id);
            if (libro == null) return NotFound();
            return View(libro);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Libro libro, IFormFile? imagenFile)
        {
            if (ModelState.IsValid)
            {
                var libroExistente = _libros.FirstOrDefault(x => x.ID == libro.ID);
                if (libroExistente != null)
                {
                    libroExistente.Titulo = libro.Titulo;
                    libroExistente.Autor = libro.Autor;
                    libroExistente.Categoria = libro.Categoria;
                    libroExistente.Precio = libro.Precio;
                    libroExistente.Disponible = libro.Disponible;

                    if (imagenFile != null && imagenFile.Length > 0)
                    {
                        string folder = Path.Combine(_env.WebRootPath, "images");
                        Directory.CreateDirectory(folder);

                        string fileName = Guid.NewGuid().ToString() + "_" + imagenFile.FileName;
                        string filePath = Path.Combine(folder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imagenFile.CopyToAsync(stream);
                        }

                        libroExistente.ImagenUrl = "/images/" + fileName;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(libro);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var libro = _libros.FirstOrDefault(x => x.ID == id);
            if (libro == null) return NotFound();
            return View(libro);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var libro = _libros.FirstOrDefault(x => x.ID == id);
            if (libro != null)
            {
                _libros.Remove(libro);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}