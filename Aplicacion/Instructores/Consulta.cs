using System.Net;
using Aplicacion.ManejadorError;
using Dominio;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Instructores
{
    public class Consulta
    {
        public class GetAllInstructores : IRequest<List<Instructor>> { }

        public class HandlerGetAllInstructores : IRequestHandler<GetAllInstructores, List<Instructor>>
        {
            private readonly CursosOnlineContext _context;
            public HandlerGetAllInstructores(CursosOnlineContext context)
            {
                this._context = context;
            }
            public async Task<List<Instructor>> Handle(GetAllInstructores request, CancellationToken cancellationToken)
            {
                var instructores = await _context.instructor!.ToListAsync();
                return instructores;
            }
        }

        public class GetInstructor : IRequest<Instructor>
        {
            public Guid InstructorID { get; set; }
        }

        public class HandlerGetInstructor : IRequestHandler<GetInstructor, Instructor>
        {
            private readonly CursosOnlineContext _context;
            public HandlerGetInstructor(CursosOnlineContext context)
            {
                this._context = context;
            }
            public async Task<Instructor> Handle(GetInstructor request, CancellationToken cancellationToken)
            {
                var instructor = await _context.instructor!.FindAsync(request.InstructorID);

                if(instructor == null)
                {
                    //throw new Exception("El curso no existe");
                    throw new ErrorCOntrol(HttpStatusCode.NotFound, new {Codigo = HttpStatusCode.NotFound, Mensaje = "El instructor no existe"});
                }

                if(instructor is not null)
                {
                    return instructor;
                }
                
                throw new ErrorCOntrol(HttpStatusCode.NotFound, new {codigo = HttpStatusCode.NotFound, mensaje = "El instructor no existe"});
            }
        }
    }
}