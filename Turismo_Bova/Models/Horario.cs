using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Turismo_Bova.Models
{
    public class Horario
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "La hora de salida es obligatoria.")]
        [DataType(DataType.DateTime, ErrorMessage = "Debe ingresar una fecha y hora válida.")]
        public DateTime HoraSalida { get; set; }


        [Required(ErrorMessage = "La hora de llegada es obligatoria.")]
        [DataType(DataType.DateTime, ErrorMessage = "Debe ingresar una fecha y hora válida.")]

        public DateTime HoraLlegada { get; set; }

        [Required(ErrorMessage = "Debe seleccionarse una ruta.")]
        [ForeignKey("Ruta")]
        public int RutaId { get; set; }
        public Ruta ruta { get; set; }

        public ICollection<Asignacion_Ruta_Vehiculo_Horario> asignacion_Ruta_Vehiculo_Horario { get; set; }

    }
}
