namespace SistemaBancoFront.Models.DTO
{
    public class CompraDTO
    {
        public int CodigoCliente { get; set; }
        public int CodigoTipoTransaccion { get; set; } = 1;
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }
        public decimal Monto { get; set; }
    }
}
