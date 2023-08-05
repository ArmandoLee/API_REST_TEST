namespace API_WEB.DTOs
{
    public class ReporteDTO
    {
        public int ClienteId { get; set; }
        public string NombreCliente { get; set; }
        public string NumeroCuenta { get; set; }
        public string TipoCuenta { get; set; }
        public decimal Saldo { get; set; }
        public decimal TotalCreditos { get; set; }
        public decimal TotalDebitos { get; set; }
    }
}
