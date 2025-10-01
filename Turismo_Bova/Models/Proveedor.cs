using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Turismo_Bova.Models
{
    public class Proveedor
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "La razón social es obligatoria.")]
        [StringLength(100, ErrorMessage = "La razón social no puede superar los 100 caracteres.")]
        public string RazonSocial { get; set; }

        [Required(ErrorMessage = "El RUC es obligatorio.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "El RUC debe tener 11 dígitos.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "El RUC debe contener solo 11 dígitos numéricos.")]
        public string Ruc { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo no tiene un formato valido.")]
        [Display(Name = "Correo electronico")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "El teléfono debe tener 9 dígitos.")]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "El teléfono debe contener solo 9 dígitos numéricos.")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        [StringLength(50, ErrorMessage = "La dirección no puede superar los 50 caracteres.")]
        public string Direccion { get; set; }

        public ICollection<Mantenimiento> mantenimiento { get; set; }
        public ICollection<Pedido> pedido { get; set; }
        public ICollection<Producto> producto { get; set; }
    }
}
