using Dominio.Seguridad;
using Microsoft.AspNetCore.Identity;

namespace Persistencia
{
    public class AdminUsuarios
    {
        public static async Task InsertarUsuario(CursosOnlineContext context, UserManager<Usuario> usuarioManager)
        {
            if(!usuarioManager.Users.Any())
            {
                var usuario = new Usuario
                {
                    NombreCompleto = "Diego Quiñonez",
                    UserName = "daqinonezr",
                    Email = "dquinonezrodriguez@gmail.com"
                };

                Console.WriteLine(usuario);

                await usuarioManager.CreateAsync(usuario, "Password123$");
            }
        }
    }
}