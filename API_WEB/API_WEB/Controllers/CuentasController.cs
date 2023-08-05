using API_WEB.DTOs;
using API_WEB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_WEB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CuentasController : ControllerBase
    {
        private readonly ApiDbContext _context;
        private readonly ILogger<CuentasController> _logger;

        public CuentasController(ApiDbContext context, ILogger<CuentasController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<CuentaDTO>> CreateCuenta(CuentaDTO cuentaDTO, string nombreCliente)
        {
            try
            {
                // Se verifica si el cliente existe en la base de datos
                var clienteExistente = await _context.Cliente.FirstOrDefaultAsync(c => c.Persona.Nombre == nombreCliente);
                if (clienteExistente == null)
                {
                    return NotFound("El cliente especificado no existe.");
                }

                // Se verifica si ya existe una cuenta del mismo tipo para el cliente
                var cuentaExistente = await _context.Cuenta
                    .FirstOrDefaultAsync(c => c.ClienteId == clienteExistente.ClienteId && c.TipoCuenta == cuentaDTO.TipoCuenta);

                if (cuentaExistente != null)
                {
                    return BadRequest("Ya existe una cuenta del mismo tipo para el cliente.");
                }

                var cuenta = new Cuenta
                {
                    NumeroCuenta = cuentaDTO.NumeroCuenta,
                    TipoCuenta = cuentaDTO.TipoCuenta,
                    SaldoInicial = cuentaDTO.Saldo,
                    Estado = cuentaDTO.Estado,
                    ClienteId = clienteExistente.ClienteId
                };

                _context.Cuenta.Add(cuenta);
                await _context.SaveChangesAsync();

                cuentaDTO.CuentaId = cuenta.CuentaId;

                return cuentaDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear una nueva cuenta.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al procesar la solicitud.");
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCuenta(int id)
        {
            try
            {
                var cuenta = await _context.Cuenta.FindAsync(id);
                if (cuenta == null)
                {
                    return NotFound("Cuenta no Existe");
                }

                _context.Cuenta.Remove(cuenta);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la cuenta.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al procesar la solicitud.");
            }
        }
    }
}
