using MediatR;
using Dominio;
using Persistencia;
using Microsoft.EntityFrameworkCore;
using Aplicacion.ManejadorError;
using System.Net;
using Dominio.DTO;
using AutoMapper;

namespace Aplicacion.Cursos
{
    public class Consulta
    {
        public class GetAllCursos : IRequest<List<CursoDto>> { }

        //Handler = Manejador
        public class HandlerGetAllCursos : IRequestHandler<GetAllCursos, List<CursoDto>>
        {
            private readonly CursosOnlineContext _context;
            private readonly IMapper _mapper;
            public HandlerGetAllCursos(CursosOnlineContext context, IMapper mapper)
            {
                this._context = context;
                this._mapper = mapper;
            }
            public async Task<List<CursoDto>> Handle(GetAllCursos request, CancellationToken cancellationToken)
            {
                var cursos = await _context.curso!
                    .Include(x => x.ComentarioLista)
                    .Include(x => x.PrecioPromocion)
                    .Include(x => x.InstructorLista)!
                    .ThenInclude(x => x.Instructor).ToListAsync();

                var cursosDto = _mapper.Map<List<Curso>, List<CursoDto>>(cursos);

                return cursosDto;
            }
        }

        public class GetCurso : IRequest<CursoDto>
        {
            public Guid CursoID { get; set; }
        }

        public class HandlerGetCurso : IRequestHandler<GetCurso, CursoDto>
        {
            private readonly CursosOnlineContext _context;
            private readonly IMapper _mapper;
            public HandlerGetCurso(CursosOnlineContext context, IMapper mapper)
            {
                this._context = context;
                this._mapper = mapper;
            }
            
            public async Task<CursoDto> Handle(GetCurso request, CancellationToken cancellationToken)
            {
                var curso = await _context.curso!
                    .Include(x => x.ComentarioLista)
                    .Include(x => x.PrecioPromocion)
                    .Include(x => x.InstructorLista)!
                    .ThenInclude(y => y.Instructor)
                    .FirstOrDefaultAsync(z => z.CursoID == request.CursoID);

                if(curso == null)
                {
                    //throw new Exception("El curso no existe");
                    throw new ErrorCOntrol(HttpStatusCode.NotFound, new {Codigo = HttpStatusCode.NotFound, Mensaje = "El curso no existe"});
                }

                if(curso is not null)
                {
                    var cursoDto = _mapper.Map<Curso, CursoDto>(curso);
                    return cursoDto;
                }
                
                throw new ErrorCOntrol(HttpStatusCode.NotFound, new {codigo = HttpStatusCode.NotFound, mensaje = "El curso no existe"});
            }
        }
    }
}