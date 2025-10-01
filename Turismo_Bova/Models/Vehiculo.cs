using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Turismo_Bova.Models
{
    public class Vehiculo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "La marca es obligatoria.")]
        [StringLength(50, ErrorMessage = "La marca no puede exceder los 50 caracteres.")]
        public string Marca { get; set; }

        [Required(ErrorMessage = "La placa es obligatoria.")]
        [StringLength(50, ErrorMessage = "La placa no puede exceder los 50 caracteres.")]
        public string Placa { get; set; }

        [Required(ErrorMessage = "El modelo es obligatorio.")]
        [StringLength(50, ErrorMessage = "El modelo no puede exceder los 50 caracteres.")]
        public string Modelo { get; set; }

        [Required(ErrorMessage = "La capacidad es obligatoria.")]
        [Range(1, 100, ErrorMessage = "La capacidad debe ser un valor positivo.")]
        public int Capacidad { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        [StringLength(20, ErrorMessage = "El estado no puede exceder los 20 caracteres.")]
        public string Estado { get; set; }

        [Required(ErrorMessage = "La fecha de adquisición es obligatoria.")]
        public DateOnly FechaAdquisicion { get; set; }

        public ICollection<Asignacion_Ruta_Vehiculo_Horario> asignacion_Ruta_Vehiculo_Horario { get; set; }
        public ICollection<Mantenimiento> mantenimiento { get; set; }
    }
}
