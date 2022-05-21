using System.Net;
using Aplicacion.Contratos;
using Aplicacion.ManejadorError;
using Dominio.Seguridad;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Seguridad
{
    public class Registrar
    {
        public class Registro : IRequest<UsuarioData>
        {
            public string? Nombre { get; set; }
            public string? Apellidos { get; set; }
            public string? Email { get; set; }
            public string? Password { get; set; }
            public string? UserName { get; set; }
        }

        public class RegistroVAlidador : AbstractValidator<Registro>
        {
            public RegistroVAlidador()
            {
                RuleFor(x => x.Nombre).NotEmpty();
                RuleFor(x => x.Apellidos).NotEmpty();
                RuleFor(x => x.Email).NotEmpty().EmailAddress();
                RuleFor(x => x.Password).NotEmpty();
                RuleFor(x => x.UserName).NotEmpty();
            }
        }

        public class HandlerRegistro : IRequestHandler<Registro, UsuarioData>
        {
            private readonly CursosOnlineContext _context;
            private readonly UserManager<Usuario> _userManager;
            private readonly IJwtGenerador _jwtGenerador;
            public HandlerRegistro(CursosOnlineContext context, UserManager<Usuario> userManager, IJwtGenerador jwtGenerador)
            {
                this._context = context;
                this._userManager = userManager;
                this._jwtGenerador = jwtGenerador;
            }
            public async Task<UsuarioData> Handle(Registro request, CancellationToken cancellationToken)
            {
                var existe = await _context.Users.Where(x => x.Email == request.Email).AnyAsync();

                if(existe)
                {
                    throw new ErrorCOntrol(HttpStatusCode.BadRequest, new {Codigo = HttpStatusCode.Forbidden, Mensaje = "Existe ya un usuario registrado con este Email"});
                }

                existe = await _context.Users.Where(x => x.UserName == request.UserName).AnyAsync();

                if(existe)
                {
                    throw new ErrorCOntrol(HttpStatusCode.BadRequest, new {Codigo = HttpStatusCode.Forbidden, Mensaje = "Existe ya un usuario registrado con este UserName"});
                }

                var usuario = new Usuario
                {
                    NombreCompleto = request.Nombre + " " + request.Apellidos,
                    Email = request.Email,
                    UserName = request.UserName
                };

                var resultado = await _userManager.CreateAsync(usuario);

                if(resultado.Succeeded)
                {
                    return new UsuarioData
                    {
                        NombreCompleto = usuario.NombreCompleto,
                        Token = _jwtGenerador.CrearToken(usuario),
                        UserName = usuario.UserName,
                        Email = usuario.Email
                    };
                }

                throw new Exception("No se pudo agregr el nuevo usuario");
            }
        }
    }
}