using System.ComponentModel.DataAnnotations;
using System.Net;
using Aplicacion.Contratos;
using Aplicacion.ManejadorError;
using Dominio.Seguridad;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Aplicacion.Seguridad
{
    public class Login
    {
        public class InicioSesion : IRequest<UsuarioData>
        {
            public string? Email { get; set; }
            public string? Password { get; set; }
        }

        public class LoginValidacion : AbstractValidator<InicioSesion>
        {
            public LoginValidacion()
            {
                RuleFor(x => x.Email).NotEmpty().EmailAddress();
                RuleFor(x => x.Password).NotEmpty();
            }
        }

        public class HandlerLogin : IRequestHandler<InicioSesion, UsuarioData>
        {
            private readonly UserManager<Usuario> _usuarioManager;
            private readonly SignInManager<Usuario> _signInManager;
            private readonly IJwtGenerador _jwtGenerador;

            public HandlerLogin(UserManager<Usuario> usuarioManager, SignInManager<Usuario> signInManager, IJwtGenerador jwtGenerador)
            {
                this._usuarioManager = usuarioManager;
                this._signInManager = signInManager;
                this._jwtGenerador = jwtGenerador;
            }
            public async Task<UsuarioData> Handle(InicioSesion request, CancellationToken cancellationToken)
            {
                var usuario = await _usuarioManager.FindByEmailAsync(request.Email);

                if(usuario == null)
                {
                    throw new ErrorCOntrol(HttpStatusCode.Unauthorized, new {Codigo = HttpStatusCode.Unauthorized, Mensaje = "El usuario no tiene acceso"});
                }

                var StatusLogin = await _signInManager.CheckPasswordSignInAsync(usuario, request.Password, false);

                if(StatusLogin.Succeeded)
                {
                    return new UsuarioData 
                    {
                        NombreCompleto = usuario.NombreCompleto,
                        Email = usuario.Email,
                        UserName = usuario.UserName,
                        Token = _jwtGenerador.CrearToken(usuario),
                        Imagen = null
                    };
                }else
                {
                    throw new ErrorCOntrol (HttpStatusCode.Forbidden, new {Codigo = HttpStatusCode.Forbidden, Mensaje = "Usuario y&o Password incorrectos"});
                }
            }
        }
    }
}