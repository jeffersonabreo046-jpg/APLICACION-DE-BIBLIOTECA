using Microsoft.AspNetCore.Mvc;
using APLICACION_DE_BIBLIOTECA.Models;
using Microsoft.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Authorization;

namespace APLICACION_DE_BIBLIOTECA.Controllers
{
    [Authorize]
    public class CategoriasController : Controller
    {
        private readonly string _connectionString;

        public CategoriasController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("BibliotecaDB");
        }
        public IActionResult Index()
        {
            var categorias = new List<Categoria>();

            using (var conexion = new SqlConnection(_connectionString))
            {
                var sql = "SELECT ID, Nombre, Descripcion FROM Categorias";
                using(var comando = new SqlCommand(sql, conexion))
                {
                    conexion.Open();
                    using (var lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            categorias.Add(new Categoria
                            {
                                ID = lector.GetInt32(0),
                                Nombre = lector.GetString(1),
                                Descripcion = lector.IsDBNull(2) ? null : lector.GetString(2)
                            });
                        }
                    }
                }
                        
            }
                return View(categorias);
        }
        //categorias create

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Create (Categoria categoria)
        {
            if (categoria == null || string.IsNullOrWhiteSpace(categoria.Nombre))
            {
                ModelState.AddModelError("Nombre", "El nombre  es obligatorio");
                return View(categoria);
            }
            using (var conexion = new SqlConnection(_connectionString))
            {
                var sql = "INSERT INTO Categorias (Nombre, Descripcion) VALUES (@Nombre, @Descripcion)";
                using (var comando = new SqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("@Nombre", categoria.Nombre);
                    comando.Parameters.AddWithValue("@Descripcion", (object)categoria.Descripcion ?? DBNull.Value);

                    conexion.Open();
                    comando.ExecuteNonQuery();
                }
            }
                TempData["SuccessMessage"] = "Categoría creada exitosamente.";
            return RedirectToAction("Index");
        }

        // obtener (get) categorias editar
        public IActionResult Edit(int id)
        {
            Categoria categoria = ObtenerPorId(id);
            if (categoria == null) return NotFound();
            return View(categoria);
        }

        //(post) Categorias editar
        [HttpPost]
        public IActionResult Edit(Categoria categoria)
        {
            if (string.IsNullOrWhiteSpace(categoria.Nombre))
            {
                ModelState.AddModelError("Nombre", "El nombre es obligatorio");
                return View(categoria);
            }

            using (var conexion = new SqlConnection(_connectionString))
            {
                var sql = "UPDATE Categorias SET Nombre = @Nombre, Descripcion = @Descripcion WHERE ID = @Id";
                using (var comando = new SqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("@Id", categoria.ID);
                    comando.Parameters.AddWithValue("@Nombre", categoria.Nombre);
                    comando.Parameters.AddWithValue("@Descripcion", (object)categoria.Descripcion ?? DBNull.Value);

                    conexion.Open();
                    comando.ExecuteNonQuery();
                }
            }

            TempData["SuccessMessage"] = "Categoría actualizada exitosamente.";
            return RedirectToAction("Index");
        }

        // (get) categorias eliminar
        public IActionResult Delete(int id)
        {
            Categoria categoria = ObtenerPorId(id);
            if (categoria == null) return NotFound();
            return View(categoria);
        }

        // (post) Categorias eliminar
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            using (var conexion = new SqlConnection(_connectionString))
            {
                var sql = "DELETE FROM Categorias WHERE ID = @Id";
                using (var comando = new SqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("@Id", id);

                    conexion.Open();
                    comando.ExecuteNonQuery();
                }
            }

            TempData["SuccessMessage"] = "Categoría eliminada exitosamente.";
            return RedirectToAction("Index");
        }

        // Método auxiliar reutilizable
        private Categoria ObtenerPorId(int id)
        {
            Categoria categoria = null;
            using (var conexion = new SqlConnection(_connectionString))
            {
                var sql = "SELECT ID, Nombre, Descripcion FROM Categorias WHERE ID = @Id";
                using (var comando = new SqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("@Id", id);
                    conexion.Open();
                    using (var reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            categoria = new Categoria
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                Nombre = reader["Nombre"].ToString(),
                                Descripcion = reader["Descripcion"] != DBNull.Value ? reader["Descripcion"].ToString() : null
                            };
                        }
                    }
                }
            }
            return categoria;
        }
    }
}
