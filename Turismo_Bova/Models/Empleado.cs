using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Turismo_Bova.Models
{
    public class Empleado
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre no debe exceder los 50 caracteres.")]
        [RegularExpression(@"^[a-zA-Z\sáéíóúÁÉÍÓÚñÑ]+$", ErrorMessage = "El nombre solo puede contener letras y espacios.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El DNI es obligatorio.")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "El DNI debe tener 8 dígitos.")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "El DNI debe contener solo números y tener 8 dígitos.")]
        public string Dni { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [StringLength(50, ErrorMessage = "El correo no debe exceder los 50 caracteres.")]
        [EmailAddress(ErrorMessage = "El correo ingresado no es válido.")]
        [Display(Name = "Correo electrónico")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "El teléfono debe tener 9 dígitos.")]
        [RegularExpression(@"^[1-9]\d{8}$", ErrorMessage = "El teléfono debe tener solo números y 9 dígitos.")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "El cargo es obligatorio.")]
        [StringLength(50, ErrorMessage = "El cargo no debe exceder los 50 caracteres.")]
        [RegularExpression(@"^[a-zA-Z\sáéíóúÁÉÍÓÚñÑ]+$", ErrorMessage = "El cargo solo puede contener letras y espacios.")]
        public string Cargo { get; set; }

        public Usuario usuario { get; set; }

        public ICollection<Mantenimiento> mantenimiento { get; set; }
        public ICollection<Reporte> reporte { get; set; }
        public ICollection<Tarea> tarea { get; set; }
        public ICollection<Asignacion_Ruta_Vehiculo_Horario> asignacion_Ruta_Vehiculo_Horario { get; set; }
    }
}
