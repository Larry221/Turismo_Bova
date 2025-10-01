using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Turismo_Bova.Models
{
    public class Tarea
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "La fecha de asignación es obligatoria.")]
        public DateOnly FechaAsignacion { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        [StringLength(20, ErrorMessage = "El estado no puede exceder los 20 caracteres.")]
        public string Estado { get; set; }

        [Required(ErrorMessage = "El empleado es obligatorio.")]
        [ForeignKey("Empleado")]
        public int EmpleadoId { get; set; }
        public Empleado empleado { get; set; }

        [Required(ErrorMessage = "El mantenimiento es obligatorio.")]
        [ForeignKey("Mantenimiento")]
        public int MantenimientoId { get; set; }
        public Mantenimiento mantenimiento { get; set; }
    }
}
