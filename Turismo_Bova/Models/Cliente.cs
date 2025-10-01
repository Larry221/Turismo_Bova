using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Turismo_Bova.Models
{
    public class Cliente
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "La razón social es obligatoria.")]
        [StringLength(50, ErrorMessage = "La razón social no puede exceder los 50 caracteres.")]
        public string RazonSocial { get; set; }

        [Required(ErrorMessage = "El RUC es obligatorio.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "El RUC debe tener 11 dígitos.")]
        [RegularExpression(@"^[1-9][0-9]{10}$", ErrorMessage = "El RUC debe contener solo números del 1 al 9 y tener 11 dígitos.")]
        public string Ruc { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [StringLength(50, ErrorMessage = "El correo no puede exceder los 50 caracteres.")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido.")]
        [Display(Name = "Correo electrónico")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "El teléfono debe tener 9 dígitos.")]
        [RegularExpression(@"^[1-9][0-9]{8}$", ErrorMessage = "El teléfono debe contener solo números del 1 al 9 y tener 9 dígitos.")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        [StringLength(50, ErrorMessage = "La dirección no puede exceder los 50 caracteres.")]
        public string Direccion { get; set; }

      
        public Usuario usuario { get; set; }

        public ICollection<Contrato> contrato { get; set; }
        public ICollection<Cotizacion> cotizacion { get; set; }
    }
}
