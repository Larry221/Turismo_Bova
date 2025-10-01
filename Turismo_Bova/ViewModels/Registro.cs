using System.ComponentModel.DataAnnotations;

namespace Turismo_Bova.ViewModels
{
    public class Registro
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre debe contener menos de 50 caracteres.")]
        [Display(Name = "Nombre de usuario")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo no tiene un formato valido.")]
        [Display(Name = "Correo electronico")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        [MinLength(3, ErrorMessage = "La contraseña debe tener al menos 3 caracteres.")]
        [Display(Name = "Contraseña")]
        public string Contraseña { get; set; }

        [Required(ErrorMessage = "Debes confirmar la contrasena.")]
        [DataType(DataType.Password)]
        [Compare("Contraseña", ErrorMessage = "Las contraseñas no coinciden.")]
        [Display(Name = "Confirmar contraseña")]
        public string ConfirmarContraseña { get; set; }

        public int RolId { get; set; }
    }
}
