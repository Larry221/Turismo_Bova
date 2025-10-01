using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Turismo_Bova.Models
{
    public class Pago
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "La fecha de pago es obligatoria.")]
        [DataType(DataType.Date, ErrorMessage = "Debe ingresar una fecha válida.")]
        public DateTime FechaPago { get; set; }

        [Required(ErrorMessage = "El monto pagado es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto pagado debe ser mayor a cero.")]
        public double MontoPagado { get; set; }

        [Required(ErrorMessage = "El método de pago es obligatorio.")]
        [StringLength(50, ErrorMessage = "El método de pago no debe exceder los 50 caracteres.")]
        public string MetodoPago { get; set; }

        [Required(ErrorMessage = "Debe seleccionarse un contrato.")]
        [ForeignKey("Contrato")]
        public int ContratoId { get; set; }
        public Contrato contrato { get; set; }
    }
}
