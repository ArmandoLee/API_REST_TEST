namespace API_WEB.DTOs
{
    using System.ComponentModel.DataAnnotations;

    public class CuentaDTO
    {
        public int CuentaId { get; set; }

        [MaxLength(11, ErrorMessage = "El campo NumeroCuenta no puede tener más de 11 dígitos.")]
        public string NumeroCuenta { get; set; }

        [RegularExpression("Ahorro|Corriente", ErrorMessage = "El campo TipoCuenta solo puede ser 'Ahorro' o 'Corriente'.")]
        public string TipoCuenta { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El campo Saldo debe ser un número no negativo.")]
        public decimal Saldo { get; set; }

        [RegularExpression("Activa|Inactiva", ErrorMessage = "El campo Estado solo puede ser 'Activa' o 'Inactiva'.")]
        public string Estado { get; set; }

        public int ClienteId { get; set; }
    }

}
