using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Turismo_Bova.Models
{
    public class Cotizacion
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public double SubTotal { get; set; }

        [Required]
        public double Igv {  get; set; }

        [Required]
        public double Total { get; set; }

        [Required]
        public string Estado { get; set; }

        [ForeignKey("Cliente")]
        public int ClienteId { get; set; }
        public Cliente cliente { get; set; }
    }
}
