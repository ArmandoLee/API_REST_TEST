﻿namespace API_WEB.DTOs
{
    public class MovimientosDTO
    {
        public int MovimientoId { get; set; }
        public DateTime Fecha { get; set; }
        public string TipoMovimiento { get; set; }
        public decimal Valor { get; set; }
        public decimal Saldo { get; set; }
        public int CuentaId { get; set; }
    }
}
