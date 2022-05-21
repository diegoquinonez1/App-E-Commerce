using Aplicacion.Seguridad;
using Dominio.Seguridad;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class UsuarioController : BaseController
    {
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<UsuarioData>> login(Login.InicioSesion sesion)
        {
            return await mediator.Send(sesion);
        }

        [HttpPost("registrar")]
        public async Task<ActionResult<UsuarioData>> registrar(Registrar.Registro registro)
        {
            return await mediator.Send(registro);
        }

        [HttpGet]
        public async Task<ActionResult<UsuarioData>> obtenerUsuario()
        {
            return await mediator.Send(new Sesion.UsuarioActual());
        }
    }
}