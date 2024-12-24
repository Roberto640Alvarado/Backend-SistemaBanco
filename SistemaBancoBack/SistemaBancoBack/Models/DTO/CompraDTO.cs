using System.ComponentModel.DataAnnotations;

namespace SistemaBancoBack.Models.DTO
{
    public class CompraDTO
    {
        [Required]
        public int CodigoCliente { get; set; }

        [Required]
        public int CodigoTipoTransaccion { get; set; } = 1; //1 para compras

        [Required]
        public DateTime Fecha { get; set; }

        [StringLength(255)]
        public string? Descripcion { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
        public decimal Monto { get; set; }
    }
}
