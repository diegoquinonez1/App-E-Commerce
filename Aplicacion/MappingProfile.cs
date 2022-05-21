using AutoMapper;
using Dominio;
using Dominio.DTO;

namespace Aplicacion
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Curso, CursoDto>()
                .ForMember(x => x.Instructores, y => y.MapFrom(z => z.InstructorLista!.Select(a => a.Instructor).ToList()))
                .ForMember(x => x.Comentarios, y => y.MapFrom(z => z.ComentarioLista))
                .ForMember(x => x.Precio, y => y.MapFrom(z => z.PrecioPromocion));
            CreateMap<Instructor, InstructorDto>();
            CreateMap<CursoInstructor, CursoInstructorDto>();
            CreateMap<Comentario, ComentarioDto>();
            CreateMap<Precio, PrecioDto>();
        }
    }
}