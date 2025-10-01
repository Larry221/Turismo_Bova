using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Turismo_Bova.Models
{
    public class Mantenimiento
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El tipo de mantenimiento es obligatorio.")]
        [StringLength(50, ErrorMessage = "El tipo de mantenimiento no debe exceder los 50 caracteres.")]
        public string TipoMantenimiento { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
        public DateOnly FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de fin es obligatoria.")]
        public DateOnly FechaFin { get; set; }

        [Required(ErrorMessage = "Debe seleccionarse un vehículo.")]
        [ForeignKey("Vehiculo")]
        public int VehiculoId { get; set; }
        public Vehiculo vehiculo { get; set; }

        [Required(ErrorMessage = "Debe seleccionarse un empleado.")]
        [ForeignKey("Empleado")]
        public int EmpleadoId { get; set; }
        public Empleado empleado { get; set; }

        [Required(ErrorMessage = "Debe seleccionarse un proveedor.")]
        [ForeignKey("Proveedor")]
        public int ProveedorId { get; set; }
        public Proveedor proveedor { get; set; }

        public ICollection<Tarea> tarea { get; set; }
    }
}
