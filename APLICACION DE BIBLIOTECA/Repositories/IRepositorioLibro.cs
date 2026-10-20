using APLICACION_DE_BIBLIOTECA.Models;

namespace APLICACION_DE_BIBLIOTECA.Repositories
{
    public interface IRepositorioLibro
    {
        IEnumerable<Libro> ObtenerTodos();
        void Agregar(Libro libro);
    }
}