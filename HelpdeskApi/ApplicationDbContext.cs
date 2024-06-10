using HelpdeskApi.Controllers;
using HelpdeskApi.entidades;
using Microsoft.EntityFrameworkCore;

namespace HelpdeskApi
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Usuario> usuarios { get; set; }

        public DbSet<NewUsuario> newUsuarios { get; set; }

        public DbSet<IncidenciaAdd> incidenciaAdds { get; set; }

        public DbSet<incidenciasUsuarios> incidenciasUsuarios { get; set; }

        public DbSet<UsuarioSesion> usuarioSesion { get; set; }

        public DbSet<UsuarioPassword> usuarioPassword { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IncidenciaAdd>().HasNoKey();
            modelBuilder.Entity<UsuarioSesion>().HasNoKey();
        }
    }
}
