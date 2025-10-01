using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Turismo_Bova.Models
{
    public class Producto
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "La cantidad en stock es obligatoria")]
        [Range(0, int.MaxValue, ErrorMessage = "La cantidad en stock no puede ser negativa")]
        public int CantidadStock { get; set; }

        [Required(ErrorMessage = "El precio por unidad es obligatorio")]
        [Range(0.01, 1000, ErrorMessage = "El precio por unidad debe ser mayor a 0")]
        public double PrecioUnidad { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un proveedor.")]
        [ForeignKey("Proveedor")]
        public int ProveedorId { get; set; }
        public Proveedor proveedor { get; set; }


        public ICollection<Pedido_Producto> pedido_producto { get; set; }
    }
}
