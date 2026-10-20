using APLICACION_DE_BIBLIOTECA.Data;
using APLICACION_DE_BIBLIOTECA.Models;
using System.Collections.Generic;
using System.Linq;

namespace APLICACION_DE_BIBLIOTECA.Services
{
    public class AutorService : IAutorService
    {
        private readonly BibliotecaContext _context;

        public AutorService(BibliotecaContext context)
        {
            _context = context;
        }

        public IEnumerable<Autor> ObtenerTodos()
        {
            return _context.Autores.ToList();
        }

        public Autor? ObtenerPorId(int id)
        {
            
            return _context.Autores.FirstOrDefault(x => x.ID == id);
        }

        public void Agregar(Autor autor)
        {
            _context.Autores.Add(autor);
            _context.SaveChanges();
        }

        public void Actualizar(Autor autor)
        {
            var autorExistente = _context.Autores.FirstOrDefault(x => x.ID == autor.ID);
            if (autorExistente != null)
            {
                autorExistente.Nombre = autor.Nombre;
                autorExistente.Apellido = autor.Apellido;
                autorExistente.Nacionalidad = autor.Nacionalidad;
                autorExistente.Fecha_De_Nacimiento = autor.Fecha_De_Nacimiento;
                autorExistente.Activo = autor.Activo;

                _context.SaveChanges(); 
            }
        }

        public void Eliminar(int id)
        {
            var autor = _context.Autores.FirstOrDefault(x => x.ID == id);
            if (autor != null)
            {
                _context.Autores.Remove(autor);
                _context.SaveChanges(); 
            }
        }
    }
}