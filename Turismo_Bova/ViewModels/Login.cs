using System.ComponentModel.DataAnnotations;

namespace Turismo_Bova.ViewModels
{
    public class Login
    {
        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo no tiene un formato valido.")]
        [Display(Name = "Correo electronico")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "La contrasena es obligatoria.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Contraseña { get; set; }
    }
}
