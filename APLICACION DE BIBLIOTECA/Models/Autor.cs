using System.ComponentModel.DataAnnotations;
using System;
namespace APLICACION_DE_BIBLIOTECA.Models
{
    public class Autor
    {
        public int ID { get; set; }
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }
        [StringLength(50)]
        public string Apellido { get; set; }        
        public string Nacionalidad { get; set; }
        [DataType(DataType.Date)]
        public DateTime Fecha_De_Nacimiento { get; set; }
        public bool Activo { get; set; }


    }
}
