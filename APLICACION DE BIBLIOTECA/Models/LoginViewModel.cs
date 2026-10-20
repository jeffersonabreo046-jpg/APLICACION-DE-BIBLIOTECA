using System.ComponentModel.DataAnnotations;

namespace APLICACION_DE_BIBLIOTECA.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="El usuario o correo electronico es obligatorio")]
        [Display(Name ="Usuario o correo electronico")]
        public string UserNameOrEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Contraseña es obligatorio")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Recuerdame")]

        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }

    }
}
