using System.ComponentModel.DataAnnotations;

namespace SistemaBancoBack.Models.DTO
{
    public class PagoDTO
    {
        [Required]
        public int CodigoCliente { get; set; }

        [Required]
        public int CodigoTipoTransaccion { get; set; } = 2; //2 para pagos

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        [StringLength(255)]
        public string Descripcion { get; set; } = "Pago";

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
        public decimal Monto { get; set; }
    }
}
