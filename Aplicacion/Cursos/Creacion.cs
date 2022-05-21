using System.ComponentModel.DataAnnotations;
using Dominio;
using FluentValidation;
using MediatR;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class Creacion
    {
        public class InsertCurso : IRequest<Guid>
        {
            //[Required(ErrorMessage="Por favor ingrese el titulo del curso")]
            public string? Titulo { get; set; }
            //[Required(ErrorMessage="Por favor ingrese la descripcion del curso")]
            public string? Descripcion { get; set; }
            public DateTime? FechaPublicacion { get; set; }
            public byte[]? FotoPortada { get; set; }
            public List<Guid>? InstructorIDs { get; set; }
            public decimal Precio { get; set; }
            public decimal Promocion { get; set; } = 0;
        }

        public class InsertCursoalidacion : AbstractValidator<InsertCurso>
        {
            public InsertCursoalidacion()
            {
                RuleFor(x => x.Titulo).NotEmpty();
                RuleFor(x => x.Descripcion).NotEmpty();
                RuleFor(x => x.FechaPublicacion).NotEmpty();
                RuleFor(x => x.Precio).NotEmpty();
            }
        }

        public class HandlerInsertCurso : IRequestHandler<InsertCurso, Guid>
        {
            private readonly CursosOnlineContext _context;
            public HandlerInsertCurso(CursosOnlineContext context)
            {
                this._context = context;
            }
            //CancellationToken funciona cuando un usuario cancela una peticion
            //por ejemplo: si una consunlta tarda demasiado y quiero cancelar, desde portman pueda dar clic en cancelar
            public async Task<Guid> Handle(InsertCurso request, CancellationToken cancellationToken)
            {
                Guid _cursoID = Guid.NewGuid();

                var curso = new Curso
                {
                    CursoID = _cursoID,
                    Titulo = request.Titulo,
                    Descripcion = request.Descripcion,
                    FechaPublicacion = request.FechaPublicacion,
                    FotoPortada = null   
                };

                _context.curso!.Add(curso);

                if(request.InstructorIDs != null)
                {
                    foreach (var id in request.InstructorIDs)
                    {
                        var cursoInstructor = new CursoInstructor
                        {
                            CursoID = _cursoID,
                            InstructorID = id
                        };
                        _context.cursoInstructor!.Add(cursoInstructor);
                    }
                }

                var precioEntidad = new Precio
                {
                    CursoID = _cursoID,
                    PrecioActual = request.Precio,
                    Promocion = request.Promocion,
                    PrecioID = Guid.NewGuid()
                };

                _context.precio!.Add(precioEntidad);
                var valor = await _context.SaveChangesAsync();
                //if 0 = no save curso in db
                //if 1 = curso save sucessfull

                if(valor > 0)
                {
                    return _cursoID;
                }

                throw new Exception("No se pudo insertar el curso");
            }
        }
    }
}