using System.ComponentModel.DataAnnotations;


namespace APLICACION_DE_BIBLIOTECA.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="El nombre de usuario es obligatorio")]
        [Display(Name ="Usuario")]
        public string UserName { get; set; } = string.Empty;
        [Required(ErrorMessage = "El correo electronico es obligatorio")]
        [EmailAddress(ErrorMessage = "Introduce un coreo electronico valido")]
        [Display(Name = "Correo Electronico")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; } = string.Empty;
        [Required(ErrorMessage = "La confirmacion de la contraseña es obligatoria")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Las contraseñas no son iguales.")]
        [Display(Name = "Confirmacion de contraseña")]
        public string ConfirmPassword { get; set; } = string.Empty;

    }
}
