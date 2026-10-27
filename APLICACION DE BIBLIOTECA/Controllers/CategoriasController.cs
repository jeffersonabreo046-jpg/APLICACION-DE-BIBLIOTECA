using Microsoft.AspNetCore.Mvc;
using APLICACION_DE_BIBLIOTECA.Models;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace APLICACION_DE_BIBLIOTECA.Controllers
{
    [Authorize]
    public class CategoriasController : Controller
    {
        private readonly string _connectionString;
        private readonly ILogger<CategoriasController> _logger;

        public CategoriasController(IConfiguration configuration, ILogger<CategoriasController> logger)
        {
            _connectionString = configuration.GetConnectionString("BibliotecaDB");
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Consultando el listado de categorias");
            var categorias = new List<Categoria>();

            try
            {
                using (var conexion = new SqlConnection(_connectionString))
                {
                    var sql = "SELECT ID, Nombre, Descripcion FROM Categorias";
                    using (var comando = new SqlCommand(sql, conexion))
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "un error al consultar el listado de categorias.");
                throw;
            }
        }

        public IActionResult Create()
        {
            _logger.LogInformation("Iniciando la vista de creacion de una nueva categoria");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Categoria categoria)
        {
            _logger.LogInformation("Iniciando el proceso de guardado de una nueva categoria");
            if (categoria == null || string.IsNullOrWhiteSpace(categoria.Nombre))
            {
                _logger.LogWarning("El modelo de categoria no cumple con los requisitos mininos");
                ModelState.AddModelError("Nombre", "El nombre es obligatorio");
                return View(categoria);
            }

            try
            {
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
                TempData["SuccessMessage"] = "Categoria creada exitosamente";
                _logger.LogInformation("Categoría '{Nombre}' creada exitosamente", categoria.Nombre);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al intentar crear la categoria");
                throw;
            }
        }

        public IActionResult Edit(int id)
        {
            _logger.LogInformation("Iniciando la vista de edición de la categoría con identificador: {CategoriaId}", id);
            Categoria categoria = ObtenerPorId(id);

            if (categoria == null)
            {
                _logger.LogWarning("No se encontro la categoria   a editar con identificador: {CategoriaId}", id);
                return NotFound();
            }
            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] 
        public IActionResult Edit(Categoria categoria)
        {
            _logger.LogInformation("Iniciando la operación de guardado de edicion para la categoria con identificador: {CategoriaId}", categoria.ID);
            if (string.IsNullOrWhiteSpace(categoria.Nombre))
            {
                _logger.LogWarning("El modelo de categoria editado no cumple con los requisitos mínimos");
                ModelState.AddModelError("Nombre", "El nombre es obligatorio");
                return View(categoria);
            }

            try
            {
                using (var conexion = new SqlConnection(_connectionString))
                {
                    var sql = "UPDATE Categorias SET Nombre = @Nombre, Descripcion = @Descripcion WHERE ID = @Id";
                    using (var comando = new SqlCommand(sql, conexion))
                    {
                        comando.Parameters.AddWithValue("@Id", categoria.ID);
                        comando.Parameters.AddWithValue("@Nombre", categoria.Nombre);
                        comando.Parameters.AddWithValue("@Descripcion", (object)categoria.Descripcion ?? DBNull.Value);

                        conexion.Open();
                        int filasAfectadas = comando.ExecuteNonQuery();

                        if (filasAfectadas == 0)
                        {
                            _logger.LogWarning("No se actualizó ninguna fila pueda que la categoría {CategoriaId} no existe", categoria.ID);
                        }
                    }
                }

                TempData["SuccessMessage"] = "Categoria actualizada exitosamente";
                _logger.LogInformation("Categoria con ID: {CategoriaId} actualizada exitosamente", categoria.ID);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la categoria con identificador: {CategoriaId}", categoria.ID);
                throw;
            }
        }

        public IActionResult Delete(int id)
        {
            _logger.LogInformation("Iniciando la vista de eliminacion de la categoría con identificador: {CategoriaId}", id);
            Categoria categoria = ObtenerPorId(id);

            if (categoria == null)
            {
                _logger.LogWarning("No se encontró la categoria para eliminar con identificador: {CategoriaId}", id);
                return NotFound();
            }
            return View(categoria);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _logger.LogInformation("Confirmacion de eliminación recibida para la categoria con identificador: {CategoriaId}", id);
            try
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

                TempData["SuccessMessage"] = "Categoria eliminada exitosamente";
                _logger.LogInformation("Categoria con ID: {CategoriaId} eliminada exitosamente", id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante la eliminacion de la categoria con identificador: {CategoriaId}", id);
                throw;
            }
        }

        //auxiliar reutilizable
        private Categoria ObtenerPorId(int id)
        {
            Categoria categoria = null;
            try
            {
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
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el metodo auxiliar al intentar obtener la categoria por ID: {CategoriaId}", id);
                throw;
            }
            return categoria;
        }
    }
}