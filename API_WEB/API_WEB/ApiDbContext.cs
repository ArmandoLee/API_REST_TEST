using API_WEB.Models;
using Microsoft.EntityFrameworkCore;

namespace API_WEB
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {
        }

        public DbSet<Persona> Persona { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Cuenta> Cuenta { get; set; }
        public DbSet<Movimientos> Movimientos { get; set; }
    }
}
