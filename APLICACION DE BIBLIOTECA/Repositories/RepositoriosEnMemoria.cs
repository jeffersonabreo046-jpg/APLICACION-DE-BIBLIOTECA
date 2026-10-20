using APLICACION_DE_BIBLIOTECA.Models;
using System.Collections.Generic;
using System.Linq;

namespace APLICACION_DE_BIBLIOTECA.Repositories
{
    public class RepositoriosEnMemoria : IRepositorioLibro
    {
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
            },
            new Libro
            {
                ID = 3,
                Titulo = "Hardware para ti",
                Autor = "Conor Phillip",
                Categoria = "Hardware 4 You",
                Precio = 29.99M,
                Disponible = false,
                ImagenUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTJECMHNxKL1x8PtY6Ex12E8ZXqEqTRLm0gKgGaTs-6ZA&s=10"
            }
        };

        public IEnumerable<Libro> ObtenerTodos()
        {
            return _libros;
        }

        public void Agregar(Libro libro)
        {
            if (_libros.Any())
            {
                libro.ID = _libros.Max(x => x.ID) + 1;
            }
            else
            {
                libro.ID = 1;
            }

            _libros.Add(libro);
        }
    }
}