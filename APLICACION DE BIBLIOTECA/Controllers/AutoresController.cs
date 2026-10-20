using Microsoft.AspNetCore.Mvc;
using APLICACION_DE_BIBLIOTECA.Models;
using APLICACION_DE_BIBLIOTECA.Services;
using APLICACION_DE_BIBLIOTECA.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace APLICACION_DE_BIBLIOTECA.Controllers
{
    [Authorize]
    public class AutoresController : Controller
    {
        private readonly BibliotecaContext _context;

        public AutoresController(BibliotecaContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Autores.ToListAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            var autor = await _context.Autores.FindAsync(id);
            if (autor == null) return NotFound();
            return View(autor);
        }

        [HttpGet]
        [Authorize(Roles = "Administrador, Bibliotecario")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador, Bibliotecario")]
        public async Task<IActionResult> Create(Autor autor)
        {
            if (!ModelState.IsValid) return View(autor);

            _context.Add(autor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Administrador, Bibliotecario")]
        public async Task<IActionResult> Edit(int id)
        {
            var autor = await _context.Autores.FindAsync(id);
            if (autor == null)
            {
                return NotFound();
            }

            return View(autor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Autor autor)
        {
            if (id != autor.ID)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(autor);
            }

            var exists = await _context.Autores.AnyAsync(a => a.ID == id);
            if (!exists)
            {
                return NotFound();
            }

            _context.Update(autor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var autor = await _context.Autores.FindAsync(id);
            if (autor == null)
            {
                return NotFound();
            }

            return View(autor);
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