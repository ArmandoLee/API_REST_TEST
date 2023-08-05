using System.ComponentModel.DataAnnotations;

namespace API_WEB.Models
{
    public class Movimientos
    {
        [Key]
        public int MovimientoId { get; set; }
        public DateTime Fecha { get; set; }
        public string TipoMovimiento { get; set; }
        public decimal Valor { get; set; }
        public decimal Saldo { get; set; }
        public int CuentaId { get; set; }
        public Cuenta Cuenta { get; set; }
    }
}
