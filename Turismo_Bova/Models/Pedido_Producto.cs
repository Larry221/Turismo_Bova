using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Turismo_Bova.Models
{
    public class Pedido_Producto
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "El precio unitario es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio unitario debe ser mayor a 0.")]
        public double PrecioUnitario { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un producto.")]
        [ForeignKey("Producto")]
        public int? ProductoId { get; set; }
        public Producto? producto { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un pedido.")]
        [ForeignKey("Pedido")]
        public int PedidoId { get; set; }
        public Pedido pedido { get; set; }

        public string? NombreProducto { get; set; }

    }
}
