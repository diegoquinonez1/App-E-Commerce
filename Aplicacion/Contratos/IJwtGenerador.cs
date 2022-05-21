using Dominio.Seguridad;

namespace Aplicacion.Contratos
{
    public interface IJwtGenerador
    {
         string CrearToken(Usuario usuario);
    }
}