namespace APLICACION_DE_BIBLIOTECA.Models
{
    public class Libro
    {
        public int ID { get; set; }
        public string Titulo { get; set; }
        public string Autor {  get; set; }
        public string Categoria { get; set; }
        public decimal Precio { get; set; }
        public bool Disponible { get; set; }
        public string? ImagenUrl { get; set; }


    }
}
