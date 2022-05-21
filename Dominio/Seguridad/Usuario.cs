using Microsoft.AspNetCore.Identity;

namespace Dominio.Seguridad
{
    public class Usuario : IdentityUser
    {
        public string? NombreCompleto { get; set; }
    }
}