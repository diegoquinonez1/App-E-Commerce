using Dominio;
using FluentValidation;
using MediatR;
using Persistencia;

namespace Aplicacion.Instructores
{
    public class Creacion
    {
        public class InsertInstructor : IRequest
        {
            public string? Nombre  { get; set; }
            public string? Apellidos { get; set; }
            public string? Grado { get; set; }
            public byte[]? FotoPerfil { get; set; }
        }

        public class InsertInstructorValidcion : AbstractValidator<InsertInstructor>
        {
            public InsertInstructorValidcion()
            {
                RuleFor(x => x.Apellidos).NotEmpty();
                RuleFor(x => x.Grado).NotEmpty();
                RuleFor(x => x.Nombre).NotEmpty();
            }
        }

        public class HandlerInsertInstrutor : IRequestHandler<InsertInstructor>
        {
            private readonly CursosOnlineContext _context;
            public HandlerInsertInstrutor(CursosOnlineContext context)
            {
                this._context = context;
            }

            public async Task<Unit> Handle(InsertInstructor request, CancellationToken cancellationToken)
            {
                Guid _InstructorID = Guid.NewGuid();

                var instructor = new Instructor
                {
                    Apellidos = request.Apellidos,
                    Grado = request.Grado,
                    InstructorID = _InstructorID,
                    Nombre = request.Nombre
                };

                _context.instructor!.Add(instructor);

                var valor = await _context.SaveChangesAsync();

                if(valor > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo insertar el instructor");
            }
        }
    }
}