namespace SistemaBancoFront.Models.DTO
{
    public class PagoDTO
    {
        public int CodigoCliente { get; set; }
        public int CodigoTipoTransaccion { get; set; } = 2;
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; } = "Pago de tarjeta";
        public decimal Monto { get; set; }
    }
}
