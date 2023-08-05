using API_WEB.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_WEB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportesController : ControllerBase
    {
        private readonly ApiDbContext _context;
        private readonly ILogger<ReportesController> _logger;

        public ReportesController(ApiDbContext context, ILogger<ReportesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Implementa la operación GET para generar el reporte de Estado de Cuenta
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReporteDTO>>> GetReporte([FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin, [FromQuery] string nombreUsuario)
        {
            try
            {
                var cliente = await _context.Cliente.FirstOrDefaultAsync(c => c.Persona.Nombre == nombreUsuario);
                if (cliente == null)
                {
                    return NotFound("El cliente especificado no existe.");
                }

                var reporte = await _context.Movimientos
                    .Where(m => m.Fecha >= fechaInicio && m.Fecha <= fechaFin && m.Cuenta.ClienteId == cliente.ClienteId)
                    .GroupBy(m => m.Cuenta)
                    .Select(group => new ReporteDTO
                    {
                        ClienteId = (int)group.Key.ClienteId,
                        NumeroCuenta = group.Key.NumeroCuenta,
                        NombreCliente = nombreUsuario,
                        TipoCuenta = group.Key.TipoCuenta,
                        Saldo = group.Key.SaldoInicial + group.Sum(m => m.Valor),
                        TotalCreditos = group.Sum(m => m.TipoMovimiento == "S" ? m.Valor : 0),
                        TotalDebitos = group.Sum(m => m.TipoMovimiento == "R" ? m.Valor : 0)
                    })
                    .ToListAsync();

                return Ok(reporte);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar el reporte de Estado de Cuenta.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al procesar la solicitud.");
            }
        }


    }

}
