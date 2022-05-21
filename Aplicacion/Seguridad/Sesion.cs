using System.Net;
using Aplicacion.Contratos;
using Aplicacion.ManejadorError;
using Dominio.Seguridad;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Aplicacion.Seguridad
{
    public class Sesion
    {
        public class UsuarioActual : IRequest<UsuarioData>
        {

        }

        public class HandlerUsuarioActual : IRequestHandler<UsuarioActual, UsuarioData>
        {
            private readonly UserManager<Usuario> _userManager;
            private readonly IJwtGenerador _jwtGenerador;
            private readonly IUsuarioSesion _usuarioSesion;
            public HandlerUsuarioActual(UserManager<Usuario> userManager, IJwtGenerador jwtGenerador, IUsuarioSesion usuarioSesion)
            {
                this._userManager = userManager;
                this._jwtGenerador = jwtGenerador;
                this._usuarioSesion = usuarioSesion;
            }
            public async Task<UsuarioData> Handle(UsuarioActual request, CancellationToken cancellationToken)
            {
                var usuario = await _userManager.FindByEmailAsync(_usuarioSesion.ObtenerUsuarioSesion());

                if(usuario == null)
                {
                    throw new ErrorCOntrol(HttpStatusCode.NotFound, new {Codigo = HttpStatusCode.NotFound, Mensaje = "El usuario no existe o el token no es valido"});
                }

                return new UsuarioData
                {
                    NombreCompleto = usuario.NombreCompleto,
                    Email = usuario.Email,
                    UserName = usuario.UserName,
                    Token = _jwtGenerador.CrearToken(usuario),
                    Imagen = null
                };
            }
        }
    }
}