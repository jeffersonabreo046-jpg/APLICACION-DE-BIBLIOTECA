using Microsoft.AspNetCore.Mvc;
using APLICACION_DE_BIBLIOTECA.Models;
using APLICACION_DE_BIBLIOTECA.Services;
using APLICACION_DE_BIBLIOTECA.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace APLICACION_DE_BIBLIOTECA.Controllers
{
    [Authorize]
    public class AutoresController : Controller
    {
        private readonly BibliotecaContext _context;
        private readonly  ILogger<AutoresController> _logger;

        public AutoresController(BibliotecaContext context, ILogger<AutoresController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Consultando el listado de autores ");
            try
            {

                return View(await _context.Autores.ToListAsync());


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Se tuvo un error al momento de consultar el listado de autores");
                throw;
    
            }
          
        }

        public async Task<IActionResult> Details(int id)
        {
            _logger.LogInformation("Consultando los detalles del autor con identificador: {AutorId}", id);

            try
            {
                var autor = await _context.Autores.FindAsync(id);

                if (autor == null)
                {
                    _logger.LogWarning("No se pudo encontrar el autor con identificador: {AutorId}", id);
                    return NotFound();
                }

                return View(autor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Se tuvo un error al momento de consultar los detalles del autor con identificador: {AutorId}", id);
                throw;


            }
        }

        [HttpGet]
        [Authorize(Roles = "Administrador, Bibliotecario")]
        public IActionResult Create()
        {
            _logger.LogInformation("iniciando a la vista de creación de un nuevo autor");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador, Bibliotecario")]
        public async Task<IActionResult> Create(Autor autor)
        {
            _logger.LogInformation("iniciando a la vista de creación de un nuevo autor");
            if (!ModelState.IsValid)
            {
                _logger.LogError("El autor no cumple con los requisitos minimos");
                return View(autor);
            }
            try
            {
                _context.Add(autor);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Se tuvo un error al momento de crear el autor: {AutorId}", autor.ID);
                throw;
            }
        }

        [HttpGet]
        [Authorize(Roles = "Administrador, Bibliotecario")]
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Iniciando la ediciOn del autor con identificador: {AutorId}", id);
            try
            {
                var autor = await _context.Autores.FindAsync(id);
                if (autor == null)
                {
                    return NotFound();
                }
                return View(autor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Se tuvo un error al momento de consultar el autor con identificador: {AutorId}", id);
                throw;
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Autor autor)
        {
            _logger.LogInformation("Iniciando la operacion de edicion del autor con identificador: {AutorId}", id);
            if (id != autor.ID)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(autor);
            }
            try
            {
                var exists = await _context.Autores.AnyAsync(a => a.ID == id);
            if (!exists)
            {
                    _logger.LogWarning("No se pudo encontrar el autor con identificador: {AutorId}", id);
                    return NotFound();
            }

            _context.Update(autor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Se tuvo un error al momento de consultar el autor con identificador: {AutorId}", id);
                throw;
            }

            
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Iniciando la operacion de eliminacion del autor con identificador: {AutorId}", id);
            try
            {
                var autor = await _context.Autores.FindAsync(id);
                if (autor == null)
                {
                    _logger.LogWarning("No se pudo encontrar el autor para eliminar con identificador: {AutorId}", id);
                    return NotFound();
                }

                return View(autor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Se tuvo un error al momento de consultar el autor con identificador: {AutorId}", id);
                throw;
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var autor = await _context.Autores.FindAsync(id);
            if (autor != null)
            {
                _context.Autores.Remove(autor);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}