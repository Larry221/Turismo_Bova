using System.ComponentModel.DataAnnotations;

namespace Turismo_Bova.ViewModels
{
    public class CambiarContraseña
    {
        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Formato invalido")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        [MinLength(3, ErrorMessage = "La contraseña debe contener como mínimo 3 caracteres.")]
        public string NuevaContraseña { get; set; }

        [Required(ErrorMessage = "Confirme la nueva contraseña.")]
        [DataType(DataType.Password)]
        [Compare("NuevaContraseña", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmarContraseña { get; set; }
    }
}
