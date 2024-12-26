namespace SistemaBancoFront.Models
{
    public class Transaccion
    {
        public int CodigoTransaccion { get; set; }
        public int CodigoCliente { get; set; }
        public int CodigoTipoTransaccion { get; set; }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }
        public decimal Monto { get; set; }
    }
}
