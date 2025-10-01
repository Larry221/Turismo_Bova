using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Turismo_Bova.Models
{
    public class Reporte
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Tipo { get; set; }

        [Required]
        public string Contenido { get; set; }

        [Required]
        public DateTime FechaGeneracion { get; set; }

        [ForeignKey("Empleado")]
        public int EmpleadoId { get; set; }
        public Empleado empleado { get; set; }

    }
}
