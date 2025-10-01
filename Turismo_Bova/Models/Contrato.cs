using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Turismo_Bova.Models
{
    public class Contrato
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
        public DateOnly FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de fin es obligatoria")]
        public DateOnly FechaFin { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio")]
        [StringLength(20, ErrorMessage = "El estado no puede exceder los 20 caracteres")]
        [RegularExpression("^(Pendiente|En proceso|Aprobado)$", ErrorMessage = "El estado debe ser 'Pendiente', 'En proceso' o 'Aprobado'.")]
        public string Estado { get; set; }

        [Required(ErrorMessage = "El precio total es obligatorio")]
        [Range(0, double.MaxValue, ErrorMessage = "El precio total debe ser positivo")]
        public double PrecioTotal { get; set; }

        [Required(ErrorMessage = "El cliente es obligatorio")]
        [ForeignKey("Cliente")]
        public int ClienteId { get; set; }
        public Cliente cliente { get; set; }

        [Required(ErrorMessage = "El servicio es obligatorio")]
        [ForeignKey("Servicio")]
        public int ServicioId { get; set; }
        public Servicio servicio { get; set; }

        public Factura factura { get; set; }
        public ICollection<Pago> pago { get; set; }

    }
}
