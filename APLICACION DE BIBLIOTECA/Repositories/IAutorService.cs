using APLICACION_DE_BIBLIOTECA.Models;

namespace APLICACION_DE_BIBLIOTECA.Services
{
    public interface IAutorService
    {
        IEnumerable<Autor> ObtenerTodos();
        Autor? ObtenerPorId(int id);
        void Agregar(Autor autor);
        void Actualizar(Autor autor);
        void Eliminar(int id);
    }
}