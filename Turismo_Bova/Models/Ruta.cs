using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Turismo_Bova.Models
{
    public class Ruta
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El origen es obligatorio.")]
        [StringLength(50, ErrorMessage = "El origen no puede exceder los 50 caracteres.")]
        public string Origen { get; set; }

        [Required(ErrorMessage = "El destino es obligatorio.")]
        [StringLength(50, ErrorMessage = "El destino no puede exceder los 50 caracteres.")]
        public string Destino { get; set; }

        [Required(ErrorMessage = "La distancia es obligatoria.")]
        [Range(0.1, double.MaxValue, ErrorMessage = "La distancia debe ser mayor a 0.")]
        public double DistanciaKm { get; set; }

        [Required(ErrorMessage = "La duración estimada es obligatoria.")]
        [RegularExpression(@"^\d{1,2}:\d{2}$", ErrorMessage = "La duración debe estar en formato HH:MM.")]
        [Display(Name = "Duración Estimada (HH:MM)")]
        public string DuracionEstimada { get; set; }

        [ForeignKey("Servicio")]
        [Required(ErrorMessage = "El servicio es obligatorio.")]
        public int ServicioId { get; set; }
        public Servicio servicio { get; set; }

        public ICollection<Asignacion_Ruta_Vehiculo_Horario> asignacion_Ruta_Vehiculo_Horario { get; set; }
        public ICollection<Horario> horario { get; set; }
    }
}
