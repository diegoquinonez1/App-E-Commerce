using System.ComponentModel.DataAnnotations;
using System.Net;
using Aplicacion.ManejadorError;
using Dominio;
using MediatR;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class Edicion
    {
        public class UpdateCurso : IRequest<Guid>
        {
            [Required(ErrorMessage="Es necesario indicar el CursoID a editar")]
            public Guid CursoID { get; set; }
            public string? Titulo { get; set; }
            public string? Descripcion { get; set; }
            public DateTime? FechaPublicacion { get; set; }
            public byte[]? FotoPortada { get; set; }
            public List<Guid>? InstructoresIDs { get; set; }
            public decimal? Precio { get; set; }
            public decimal? Promocion { get; set; } = 0;
        }

        public class HandlerUpdate : IRequestHandler<UpdateCurso, Guid>
        {
            private readonly CursosOnlineContext _context;
            public HandlerUpdate(CursosOnlineContext context)
            {
                this._context = context;
            }
            public async Task<Guid> Handle(UpdateCurso request, CancellationToken cancellationToken)
            {
                var curso = await _context.curso!.FindAsync(request.CursoID);

                if(curso == null)
                {
                    //throw new Exception("El curso no existe");
                    throw new ErrorCOntrol(HttpStatusCode.NotFound, new {Codigo = HttpStatusCode.NotFound, Mensaje = "El curso no existe"});
                }

                curso.Titulo = request.Titulo ?? curso.Titulo;
                curso.Descripcion = request.Descripcion ?? curso.Descripcion;
                curso.FechaPublicacion = request.FechaPublicacion ?? curso.FechaPublicacion;

                var precioEntidad = _context.precio!.Where(x => x.CursoID == request.CursoID).FirstOrDefault();
                
                if(precioEntidad != null)
                {
                    precioEntidad.PrecioActual = request.Precio ?? precioEntidad.PrecioActual;
                    precioEntidad.Promocion = request.Promocion ?? precioEntidad.Promocion;

                    _context.precio!.Update(precioEntidad);
                }else
                {
                    precioEntidad = new Precio
                    {
                        CursoID = curso.CursoID,
                        PrecioActual = request.Precio ?? 0,
                        Promocion = request.Promocion ?? 0,
                        PrecioID = Guid.NewGuid()
                    };
                    
                    await _context.precio!.AddAsync(precioEntidad);
                }

                if(request.InstructoresIDs != null && request.InstructoresIDs.Count > 0)
                {
                    var instructores = _context.cursoInstructor!.Where(x => x.CursoID == request.CursoID).ToList();
                    
                    foreach(var instructorID in instructores)
                    {
                        _context.cursoInstructor!.Remove(instructorID);
                    }

                    foreach(var instructorID in request.InstructoresIDs)
                    {
                        var cursoInstructor = new CursoInstructor
                        {
                            CursoID = request.CursoID,
                            InstructorID = instructorID
                        };
                        await _context.cursoInstructor!.AddAsync(cursoInstructor);
                    }
                }
                
                _context.curso.Update(curso);
                var valor = await _context.SaveChangesAsync();

                if(valor > 0)
                {
                    return curso.CursoID;
                }

                throw new Exception("No se pudieron guardar los cambios");
            }
        }
    }
}