using Microsoft.AspNetCore.Mvc;
using APLICACION_DE_BIBLIOTECA.Models;
using APLICACION_DE_BIBLIOTECA.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace APLICACION_DE_BIBLIOTECA.Controllers
{
    [Authorize] //Protegers todo el controlador 
    public class LibrosController : Controller
    {
        private readonly BibliotecaContext _context;

        public LibrosController(BibliotecaContext context)
        {
            _context = context;
        }

        //Permitido para todos los roles autenticados Usuario, Bibliotecario, Administrador
        public async Task<IActionResult> Index()
        {
            return View(await _context.Libros.ToListAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro == null) return NotFound();
            return View(libro);
        }

        //Restringido a Administrador y Bibliotecario
        [Authorize(Roles = "Administrador,Bibliotecario")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Administrador,Bibliotecario")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Libro libro, IFormFile? imagenFile, [FromServices] IWebHostEnvironment env)
        {
            if (ModelState.IsValid)
            {
                if (imagenFile != null && imagenFile.Length > 0)
                {
                    string folder = Path.Combine(env.WebRootPath, "images");
                    Directory.CreateDirectory(folder);

                    string fileName = Guid.NewGuid().ToString() + "_" + imagenFile.FileName;
                    string filePath = Path.Combine(folder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imagenFile.CopyToAsync(stream);
                    }

                    libro.ImagenUrl = "/images/" + fileName;
                }

                _context.Libros.Add(libro);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(libro);
        }

        //Restringido a Administrador y Bibliotecario
        [Authorize(Roles = "Administrador,Bibliotecario")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro == null) return NotFound();
            return View(libro);
        }

        [Authorize(Roles = "Administrador,Bibliotecario")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Libro libro, IFormFile? imagenFile, [FromServices] IWebHostEnvironment env)
        {
            if (id != libro.ID) return BadRequest();

            if (ModelState.IsValid)
            {
                var libroExistente = await _context.Libros.FindAsync(id);
                if (libroExistente == null) return NotFound();

                //Actualizar propiedades
                libroExistente.Titulo = libro.Titulo;
                libroExistente.Autor = libro.Autor;
                libroExistente.Categoria = libro.Categoria;
                libroExistente.Precio = libro.Precio;
                libroExistente.Disponible = libro.Disponible;

                //imagen
                if (imagenFile != null && imagenFile.Length > 0)
                {
                    string folder = Path.Combine(env.WebRootPath, "images");
                    Directory.CreateDirectory(folder);

                    string fileName = Guid.NewGuid().ToString() + "_" + imagenFile.FileName;
                    string filePath = Path.Combine(folder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imagenFile.CopyToAsync(stream);
                    }

                    libroExistente.ImagenUrl = "/images/" + fileName;
                }

                _context.Libros.Update(libroExistente);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(libro);
        }

        //Restringido a Administrador y Bibliotecario
        [Authorize(Roles = "Administrador,Bibliotecario")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro == null) return NotFound();
            return View(libro);
        }

        [Authorize(Roles = "Administrador,Bibliotecario")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro != null)
            {
                _context.Libros.Remove(libro);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}