using System.ComponentModel.DataAnnotations;

namespace SistemaBancoBack.Models.DTO
{
    public class CrearClienteDTO
    {
        [Required]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(100, ErrorMessage = "El apellido no puede superar los 100 caracteres.")]
        public string Apellido { get; set; } = string.Empty;

        [Required]
        [StringLength(16, ErrorMessage = "El número de tarjeta debe tener exactamente 16 caracteres.")]
        [RegularExpression(@"^\d{16}$", ErrorMessage = "El número de tarjeta debe ser numérico y tener 16 dígitos.")]
        public string NumeroTarjeta { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El límite de crédito debe ser mayor a 0.")]
        public decimal LimiteCredito { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El saldo disponible debe ser mayor a 0.")]
        public decimal SaldoDisponible { get; set; }
    }
}
