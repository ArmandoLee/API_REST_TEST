using API_WEB.DTOs;
using API_WEB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_WEB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovimientosController : ControllerBase
    {
        private readonly ApiDbContext _context;
        private readonly ILogger<MovimientosController> _logger;

        public MovimientosController(ApiDbContext context, ILogger<MovimientosController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<MovimientosDTO>> CreateMovimiento(MovimientosDTO movimientoDTO, string numeroCuenta)
        {
            try
            {
                // Se busca la cuenta por su número de cuenta
                var cuenta = await _context.Cuenta.FirstOrDefaultAsync(c => c.NumeroCuenta == numeroCuenta);
                if (cuenta == null)
                {
                    return NotFound("La cuenta especificada no existe.");
                }

                // Se verifica que la cuenta tenga saldo positivo (> 0) para realizar el movimiento
                if (cuenta.SaldoInicial < 0)
                {
                    return BadRequest("La cuenta no tiene saldo suficiente para realizar el movimiento.");
                }

                // Se indica tipo de movimiento S para suma (Crédito), R para resta (Débito)
                if (movimientoDTO.TipoMovimiento == "S")
                {
                    cuenta.SaldoInicial += movimientoDTO.Valor;
                }
                else if (movimientoDTO.TipoMovimiento == "R")
                {
                    if (movimientoDTO.Valor > cuenta.SaldoInicial)
                    {
                        return BadRequest("Saldo insuficiente para realizar el movimiento de débito.");
                    }

                    // Se Obtienen los movimientos de tipo (débito) "R" realizados en el mismo día
                    var movimientosRestaDelDia = await _context.Movimientos
                        .Where(m => m.TipoMovimiento == "R" && m.Fecha.Date == movimientoDTO.Fecha.Date && m.CuentaId == cuenta.CuentaId)
                        .ToListAsync();

                    // Se calcular la suma de los valores de los movimientos de tipo (débito) "R" del día
                    decimal sumaMovimientosRestaDelDia = movimientosRestaDelDia.Sum(m => m.Valor);

                    // Se verifica si la suma supera el límite diario de 1000
                    if (sumaMovimientosRestaDelDia + movimientoDTO.Valor > 1000)
                    {
                        return BadRequest("Límite diario excedido para movimientos de débito.");
                    }

                    cuenta.SaldoInicial -= movimientoDTO.Valor;
                }
                else
                {
                    return BadRequest("Tipo de movimiento inválido.");
                }

                await _context.SaveChangesAsync();

                var movimiento = new Movimientos
                {
                    Fecha = movimientoDTO.Fecha,
                    TipoMovimiento = movimientoDTO.TipoMovimiento,
                    Valor = movimientoDTO.Valor,
                    Saldo = cuenta.SaldoInicial,
                    CuentaId = cuenta.CuentaId
                };

                _context.Movimientos.Add(movimiento);
                await _context.SaveChangesAsync();

                var movimientoDtoResponse = new MovimientosDTO
                {
                    MovimientoId = movimiento.MovimientoId,
                    Fecha = movimiento.Fecha,
                    TipoMovimiento = movimiento.TipoMovimiento,
                    Valor = movimiento.Valor,
                    Saldo = movimiento.Saldo,
                    CuentaId = movimiento.CuentaId
                };

                return movimientoDtoResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo movimiento.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al procesar la solicitud.");
            }
        }

    }
}
