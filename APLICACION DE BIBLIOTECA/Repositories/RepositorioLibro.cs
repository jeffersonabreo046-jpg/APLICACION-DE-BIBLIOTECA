using APLICACION_DE_BIBLIOTECA.Data;
using APLICACION_DE_BIBLIOTECA.Models;

namespace APLICACION_DE_BIBLIOTECA.Repositories
{
    public class RepositorioLibro : IRepositorioLibro
    {
        private readonly BibliotecaContext _context;

        public RepositorioLibro(BibliotecaContext context)
        {
            _context = context;
        }

        public IEnumerable<Libro> ObtenerTodos()
        {
            return _context.Libros.ToList();
        }

        public void Agregar(Libro libro)
        {
            _context.Libros.Add(libro);
            _context.SaveChanges();
        }
    }
}