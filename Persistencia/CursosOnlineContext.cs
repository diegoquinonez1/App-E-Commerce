using Microsoft.EntityFrameworkCore;
using Dominio;
using Dominio.Seguridad;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Persistencia
{
    public class CursosOnlineContext : IdentityDbContext<Usuario>
    {
        public CursosOnlineContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //crea archivo de migracion que va a contener la logica para crear las tablas
            base.OnModelCreating(modelBuilder);

            //para poder crear el archivo de migracion dentro de persistencia es necesario instalar
            //una herramienta de asp net core llamada dotnet ef
            //dotnet tool install --global dotnet-ef --version 6.0.3

            //comando para generar el archivo de configuracion
            //dotnet ef migrations add IdentityCoreInitial -p Persistencia/ -s WebAPI/

            modelBuilder.Entity<CursoInstructor>().HasKey(ci => new {ci.CursoID, ci.InstructorID});
        }

        public DbSet<Comentario>? comentario { get; set; }
        public DbSet<Curso>? curso { get; set; }
        public DbSet<CursoInstructor>? cursoInstructor { get; set; }
        public DbSet<Instructor>? instructor { get; set; }
        public DbSet<Precio>? precio { get; set; }
    }
}