using System.Net;
using Aplicacion.ManejadorError;
using Newtonsoft.Json;

namespace WebAPI.Middleware
{
    //interceptador de requerimientos
    public class ManejadorErrorMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ManejadorErrorMiddleware> _logger;

        //recibe dos parametros para poder manejar los estados de respuesta hacia el cliente
        public ManejadorErrorMiddleware(RequestDelegate next, ILogger<ManejadorErrorMiddleware> logger)
        {
            this._next = next;
            this._logger = logger;
        }

        //metodo pra manejar request en el contexto
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await ManejadorExcepcionAsincrono(context, ex, _logger);
            }
        }

        private async Task ManejadorExcepcionAsincrono(HttpContext context, Exception ex, ILogger<ManejadorErrorMiddleware> logger)
        {
            object? errores = null;

            /*
            2xx = Transaccion correcta
            3xx = No se modifico
            4xx = Errores en el frontend
            5xx = Errores en el servidor
            */

            switch (ex)
            {
                case ErrorCOntrol ec:
                    logger.LogError(ex, "Manejador Error");
                    errores = ec.Errores;
                    context.Response.StatusCode = (int)ec.Codigo;
                    break;
                case Exception e:
                    logger.LogError(ex, "Error de servidor");
                    errores = string.IsNullOrWhiteSpace(e.Message) ? "Error" : e.Message;
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;//puede ser de tipo 5xx
                    break;
            }

            //formato en que se devuelve el error
            //detiene la transaccion
            context.Response.ContentType = "application/json";

            if (errores != null)
            {
                var resultados = JsonConvert.SerializeObject(new { errores });
                await context.Response.WriteAsync(resultados);
            }
        }
    }
}