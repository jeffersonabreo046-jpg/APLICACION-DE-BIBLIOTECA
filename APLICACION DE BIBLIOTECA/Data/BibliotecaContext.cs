using APLICACION_DE_BIBLIOTECA.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace APLICACION_DE_BIBLIOTECA.Data
{
    public class BibliotecaContext : IdentityDbContext<IdentityUser>
    {
        public BibliotecaContext(DbContextOptions<BibliotecaContext> options) : base (options) { }      
        public DbSet<Autor> Autores { get; set; }
        public DbSet<Libro> Libros { get; set; }
    }

}
