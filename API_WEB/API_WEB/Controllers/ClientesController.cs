using API_WEB.DTOs;
using API_WEB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_WEB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly ApiDbContext _context;
        private readonly ILogger<ClientesController> _logger;

        public ClientesController(ApiDbContext context, ILogger<ClientesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<ClienteDTO>> CreateCliente(ClienteDTO clienteDTO)
        {
            try
            {
                var persona = new Persona // Creamos una nueva entidad Persona
                {
                    Nombre = clienteDTO.Persona.Nombre,
                    Genero = clienteDTO.Persona.Genero,
                    Edad = clienteDTO.Persona.Edad,
                    Identificacion = clienteDTO.Persona.Identificacion,
                    Direccion = clienteDTO.Persona.Direccion,
                    Telefono = clienteDTO.Persona.Telefono
                };

                var cliente = new Cliente
                {
                    Contraseña = clienteDTO.contraseña,
                    Estado = clienteDTO.Estado,
                    Persona = persona // Asociamos la entidad Persona al cliente
                };

                _context.Cliente.Add(cliente);
                await _context.SaveChangesAsync();

                clienteDTO.ClienteId = cliente.ClienteId;
                clienteDTO.PersonaId = cliente.Persona.Id; // Asignamos el ID de persona al clienteDTO

                return clienteDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo cliente.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al procesar la solicitud.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            try
            {
                var cliente = await _context.Cliente.FindAsync(id);
                if (cliente == null)
                {
                    return NotFound();
                }

                // Se busca y elimina los movimientos asociados a las cuentas del cliente
                var cuentas = await _context.Cuenta.Where(c => c.ClienteId == cliente.ClienteId).ToListAsync();
                foreach (var cuenta in cuentas)
                {
                    var movimientos = await _context.Movimientos.Where(m => m.CuentaId == cuenta.CuentaId).ToListAsync();
                    _context.Movimientos.RemoveRange(movimientos);
                }

                // Se eliminan las cuentas asociadas al cliente
                _context.Cuenta.RemoveRange(cuentas);

                await _context.SaveChangesAsync();

                // Se elimina la persona asociada al cliente
                var persona = await _context.Persona.FindAsync(cliente.PersonaId);
                if (persona != null)
                {
                    _context.Persona.Remove(persona);
                }

                // Por ultimo se elimina al cliente
                _context.Cliente.Remove(cliente);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el cliente.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al procesar la solicitud.");
            }
        }
    }
}
