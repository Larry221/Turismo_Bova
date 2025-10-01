using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Turismo_Bova.Models
{
    public class Usuario
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio"), StringLength(50)]
        [EmailAddress(ErrorMessage = "Correo inválido!")]
        [Display(Name = "Correo electrónico")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        [MinLength(3)]
        public string Contraseña { get; set; }

        [ForeignKey("Rol")]
        public int RolId { get; set; }
        public Rol rol { get; set; }

        [ForeignKey("Empleado")]
        public int? EmpleadoId { get; set; }
        public Empleado Empleado { get; set; }

        [ForeignKey("Cliente")]
        public int? ClienteId { get; set; }
        public Cliente Cliente { get; set; }
    }
}
