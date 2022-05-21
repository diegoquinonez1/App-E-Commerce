using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Aplicacion.Contratos;
using Dominio.Seguridad;
using Microsoft.IdentityModel.Tokens;

namespace Seguridad.Tokens
{
    public class JwtGenerador : IJwtGenerador
    {
        public string CrearToken(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, usuario.Email, usuario.UserName)
            };

            //variables de acceso
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MiPalabraSecreta"));
            var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescripcion = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(30),
                SigningCredentials = credenciales
            };

            var tokenManejador = new JwtSecurityTokenHandler();
            var token = tokenManejador.CreateToken(tokenDescripcion);

            return tokenManejador.WriteToken(token);
        }
    }
}