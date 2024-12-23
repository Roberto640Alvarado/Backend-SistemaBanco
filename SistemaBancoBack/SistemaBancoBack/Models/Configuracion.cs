using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SistemaBancoBack.Models
{
    public class Configuracion
    {
        [Key]
        public int CodigoConfiguracion { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal PorcentajeInteres { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal PorcentajeSaldoMinimo { get; set; }
    }
}
