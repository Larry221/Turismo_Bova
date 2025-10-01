using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Turismo_Bova.Models
{
    public class Factura
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "La fecha de emisión es obligatoria.")]
        public DateOnly FechaEmision { get; set; }

        [Required(ErrorMessage = "El IGV es obligatorio.")]
        [Range(0, 18, ErrorMessage = "El IGV debe ser un valor positivo.")]
        public double Igv {  get; set; }

        [Required(ErrorMessage = "El total es obligatorio.")]
        [Range(0.01, 10000, ErrorMessage = "El total debe ser mayor a cero.")]
        public double Total { get; set; }

        [Required(ErrorMessage = "El estado de pago es obligatorio.")]
        [StringLength(20, ErrorMessage = "El estado de pago no debe exceder los 20 caracteres.")]
        public string EstadoPago { get; set; }

        [Required(ErrorMessage = "Debe seleccionarse un contrato.")]
        [ForeignKey("Contrato")]
        public int ContratoId { get; set; }
        public Contrato contrato { get; set; }
    }
}
