using System.ComponentModel.DataAnnotations;

namespace Turismo_Bova.Models
{
    public class Comprobante
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El número es obligatorio")]
        [StringLength(20)]
        public string Numero { get; set; } = string.Empty;

        [Required(ErrorMessage = "El tipo de comprobante es obligatorio")]
        [StringLength(50)]
        public string Tipo { get; set; } = string.Empty; // Factura, Boleta, etc.

        [Required(ErrorMessage = "La fecha de emisión es obligatoria")]
        public DateTime FechaEmision { get; set; }

        [Required(ErrorMessage = "El total es obligatorio")]
        [Range(0, double.MaxValue, ErrorMessage = "El total debe ser mayor o igual a 0")]
        public decimal Total { get; set; }

        // Relación con cliente
        [Required]
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }
    }
}
