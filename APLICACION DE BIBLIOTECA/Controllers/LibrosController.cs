using Microsoft.AspNetCore.Mvc;
using APLICACION_DE_BIBLIOTECA.Models;
using APLICACION_DE_BIBLIOTECA.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace APLICACION_DE_BIBLIOTECA.Controllers
{
    [Authorize]  
    public class LibrosController : Controller
    {
        private readonly BibliotecaContext _context;
        private readonly ILogger<LibrosController> _logger;

        public LibrosController(BibliotecaContext context, ILogger<LibrosController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // roles autenticados Usuario, Bibliotecario, Administrador
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Consultando el listado completo de libros");
            try
            {
                return View(await _context.Libros.ToListAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "hubo un error al consultar el listado de libros");
                throw;
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            _logger.LogInformation("Consultando los detalles del lo libro con identificador: {LibroId}", id);
            try
            {
                var libro = await _context.Libros.FindAsync(id);
                if (libro == null)
                {
                    _logger.LogWarning("No se encontro el libro con identificador: {LibroId}", id);
                    return NotFound();
                }
                return View(libro);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al ver los detalles del libro con identificador: {LibroId}", id);
                throw;
            }
        }

        //Administrador y Bibliotecario
        [Authorize(Roles = "Administrador,Bibliotecario")]
        [HttpGet]
        public IActionResult Create()
        {
            _logger.LogInformation("Iniciando la vista de creacion de un nuevo libro");
            return View();
        }

        [Authorize(Roles = "Administrador,Bibliotecario")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Libro libro, IFormFile? imagenFile, [FromServices] IWebHostEnvironment env)
        {
            _logger.LogInformation("Iniciando el proceso de guardado de un nuevo libro");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("El modelo de libro no cumple con los requisitos de validación");
                return View(libro);
            }

            try
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
                
                _logger.LogInformation("Libro creado exitosamente.");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "hubo un error al intentar crear el libro");
                throw;
            }
        }

        // Restringido a Administrador y Bibliotecario
        [Authorize(Roles = "Administrador,Bibliotecario")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Iniciando la vista de edicion del libro con identificador: {LibroId}", id);
            try
            {
                var libro = await _context.Libros.FindAsync(id);
                if (libro == null)
                {
                    _logger.LogWarning("No se encontró el libro a editar con identificador: {LibroId}", id);
                    return NotFound();
                }
                return View(libro);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar el libro para edicion con identificador: {LibroId}", id);
                throw;
            }
        }

        [Authorize(Roles = "Administrador,Bibliotecario")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Libro libro, IFormFile? imagenFile, [FromServices] IWebHostEnvironment env)
        {
            _logger.LogInformation("Iniciando la operación de guardado de edicion para el libro con identificador: {LibroId}", id);
            
            if (id != libro.ID)
            {
                _logger.LogWarning("El ID de la ruta ({RouteId}) no coincide con el ID del modelo ({ModelId})", id, libro.ID);
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(libro);
            }

            try
            {
                var libroExistente = await _context.Libros.FindAsync(id);
                if (libroExistente == null)
                {
                    _logger.LogWarning("No se encontro el libro en la base de datos con identificador: {LibroId}", id);
                    return NotFound();
                }

                // propiedades
                libroExistente.Titulo = libro.Titulo;
                libroExistente.Autor = libro.Autor;
                libroExistente.Categoria = libro.Categoria;
                libroExistente.Precio = libro.Precio;
                libroExistente.Disponible = libro.Disponible;

                //Imagen
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
                
                _logger.LogInformation("Libro con ID: {LibroId} actualizado exitosamente", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el libro con identificador: {LibroId}", id);
                throw;
            }
        }

        //Administrador y Bibliotecario
        [Authorize(Roles = "Administrador,Bibliotecario")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Iniciando la vista de eliminacion del libro con identificador: {LibroId}", id);
            try
            {
                var libro = await _context.Libros.FindAsync(id);
                if (libro == null)
                {
                    _logger.LogWarning("No se encontro el libro para eliminar con identificador: {LibroId}", id);
                    return NotFound();
                }
                return View(libro);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar el libro para eliminación con identificador: {LibroId}", id);
                throw;
            }
        }

        [Authorize(Roles = "Administrador,Bibliotecario")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation("Confirmacion de eliminacion recibida para el libro con identificador: {LibroId}", id);
            try
            {
                var libro = await _context.Libros.FindAsync(id);
                if (libro != null)
                {
                    _context.Libros.Remove(libro);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Libro con ID: {LibroId} eliminado", id);
                }
                else
                {
                    _logger.LogWarning("Se intentó eliminar el libro con ID: {LibroId}, pero  no existe", id);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante la eliminación del libro con identificador: {LibroId}", id);
                throw;
            }
        }
    }
}