using System.ComponentModel.DataAnnotations;

namespace Turismo_Bova.ViewModels
{
    public class OlvidarContraseña
    {
        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Formato inválido.")]
        public string Correo { get; set; }
    }
}
