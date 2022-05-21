using System.Net;

namespace Aplicacion.ManejadorError
{
    public class ErrorCOntrol : Exception
    {
        public HttpStatusCode Codigo { get; }
        public object? Errores { get; }
        public ErrorCOntrol(HttpStatusCode codigo, object? errores = null)
        {
            this.Codigo = codigo;
            this.Errores = errores;
        }
    }
}