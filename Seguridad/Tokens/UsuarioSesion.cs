using System.Security.Claims;
using Aplicacion.Contratos;
using Microsoft.AspNetCore.Http;

namespace Seguridad.Tokens
{
    public class UsuarioSesion : IUsuarioSesion
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UsuarioSesion(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
        }
        public string ObtenerUsuarioSesion()
        {
            var userName = _httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            return userName!;
        }
    }
}