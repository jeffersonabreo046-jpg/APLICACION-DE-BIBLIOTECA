using APLICACION_DE_BIBLIOTECA.Models;

namespace APLICACION_DE_BIBLIOTECA.Services
{
    public class AutorServiceEspecial : IAutorService
    {
        private static List<Autor> _autoresEspeciales = new List<Autor>()
        {
            new Autor
            {
                ID = 100,
                Nombre = "Gabriel",
                Apellido = "García Márquez",
                Nacionalidad = "Colombiano",
                Fecha_De_Nacimiento = new DateTime(1927, 3, 6),
                Activo = true
            }
        };

        public IEnumerable<Autor> ObtenerTodos()
        {
            return _autoresEspeciales;
        }

        public Autor? ObtenerPorId(int id)
        {
            return _autoresEspeciales.FirstOrDefault(x => x.ID == id);
        }

        public void Agregar(Autor autor)
        {
            autor.ID = _autoresEspeciales.Any() ? _autoresEspeciales.Max(x => x.ID) + 1 : 100;
            _autoresEspeciales.Add(autor);
        }

        public void Actualizar(Autor autor)
        {
            var existente = _autoresEspeciales.FirstOrDefault(x => x.ID == autor.ID);
            if (existente != null)
            {
                existente.Nombre = autor.Nombre;
                existente.Apellido = autor.Apellido;
                existente.Nacionalidad = autor.Nacionalidad;
                existente.Fecha_De_Nacimiento = autor.Fecha_De_Nacimiento;
                existente.Activo = autor.Activo;
            }
        }

        public void Eliminar(int id)
        {
            var autor = _autoresEspeciales.FirstOrDefault(x => x.ID == id);
            if (autor != null)
            {
                _autoresEspeciales.Remove(autor);
            }
        }
    }
}