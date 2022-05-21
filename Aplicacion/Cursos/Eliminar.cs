using System.Net;
using Aplicacion.ManejadorError;
using MediatR;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class Eliminar
    {
        public class DeleteCurso : IRequest
        {
            public Guid CursoID { get; set; }
        }

        public class HandlerDelete : IRequestHandler<DeleteCurso>
        {
            private readonly CursosOnlineContext _context;

            public HandlerDelete(CursosOnlineContext context)
            {
                this._context = context;
            }

            public async Task<Unit> Handle(DeleteCurso request, CancellationToken cancellationToken)
            {
                var curso = await _context.curso!.FindAsync(request.CursoID);
                var cursoInstructores = _context.cursoInstructor!.Where(x => x.CursoID == request.CursoID);
                var precioEntidad = _context.precio!.Where(x => x.CursoID == request.CursoID).FirstOrDefault();
                var comentarios = _context.comentario!.Where(x => x.CursoID == request.CursoID);

                if (curso == null)
                {
                    //throw new Exception("El curso no existe");
                    throw new ErrorCOntrol(HttpStatusCode.NotFound, new {Codigo = HttpStatusCode.NotFound, Mensaje = "El curso no existe"});
                }

                foreach(var instructor in cursoInstructores)
                {
                    _context.cursoInstructor!.Remove(instructor);
                }

                foreach(var comentario in comentarios)
                {
                    _context.comentario!.Remove(comentario);
                }

                _context.precio!.Remove(precioEntidad!);
                _context.curso.Remove(curso);
                var valor = await _context.SaveChangesAsync();

                if(valor > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudieron guardar los cambios");
            }
        }
    }
}