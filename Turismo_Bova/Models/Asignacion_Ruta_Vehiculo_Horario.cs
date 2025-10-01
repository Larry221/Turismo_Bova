using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Turismo_Bova.Models
{
    public class Asignacion_Ruta_Vehiculo_Horario
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "La fecha de asignación es obligatoria.")]
        [DataType(DataType.Date, ErrorMessage = "Formato de fecha inválido.")]
        [Display(Name = "Fecha de Asignación")]
        public DateOnly FechaAsignacion {  get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        [StringLength(20, ErrorMessage = "El estado no debe exceder los 20 caracteres.")]
        [RegularExpression("^(Por salir|En ruta|Completada)$", ErrorMessage = "El estado debe ser 'Por salir', 'En ruta' o 'Completada'.")]
        public string Estado { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una ruta.")]
        [ForeignKey("Ruta")]
        public int RutaId { get; set; }
        public Ruta ruta { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un vehículo.")]
        [ForeignKey("Vehiculo")]
        public int VehiculoId { get; set; }
        public Vehiculo vehiculo { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un horario.")]
        [ForeignKey("Horario")]
        public int HorarioId { get; set; }
        public Horario horario { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un empleado.")]
        [ForeignKey("Empleado")]
        public int EmpleadoId { get; set; }
        public Empleado empleado { get; set; }

    }
}
