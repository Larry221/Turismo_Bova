using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Turismo_Bova.Models
{
    public class Pedido
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "La descripción del pedido es obligatoria.")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "La fecha del pedido es obligatoria.")]
        public DateOnly FechaPedido { get; set; }

        [Required(ErrorMessage = "El estado del pedido es obligatorio.")]
        [StringLength(50, ErrorMessage = "El estado no puede superar los 50 caracteres.")]
        public string Estado { get; set; }

        [Required(ErrorMessage = "Debe seleccionarse un proveedor.")]
        [ForeignKey("Proveedor")]
        public int ProveedorId { get; set; }
        public Proveedor proveedor { get; set; }

        public ICollection<Pedido_Producto> pedido_producto { get; set; }
    }
}
