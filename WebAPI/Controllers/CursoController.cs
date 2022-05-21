using Microsoft.AspNetCore.Mvc;
using MediatR;
using Dominio;
using Aplicacion.Cursos;
using Dominio.DTO;
//using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers
{
    [ApiController]
    public class CursoController : BaseController
    {
        public CursoController()
        {  }

        [HttpGet]
        //[Authorize]
        public async Task<List<CursoDto>> GetAll()
        {
            return await mediator.Send(new Consulta.GetAllCursos());
        }

        [HttpGet("{CursoID}")]
        public async Task<CursoDto> Get(Guid CursoID)
        {
            return await mediator.Send(
                new Consulta.GetCurso()
                {
                    CursoID=CursoID
                }
            );
        }

        [HttpPost]
        public async Task<CursoDto> Post(Creacion.InsertCurso data)
        {
            var CursoID =  await mediator.Send(data);
            return await mediator.Send(
                new Consulta.GetCurso()
                {
                    CursoID=CursoID
                }
            );
        }

        [HttpPut("{CursoID}")]
        public async Task<CursoDto> Put(Guid CursoID,  Edicion.UpdateCurso data)
        {
            data.CursoID = CursoID;
            var ID = await mediator.Send(data);
            return await mediator.Send(
                new Consulta.GetCurso()
                {
                    CursoID=ID
                }
            );
        }

        [HttpDelete("{CursoID}")]
        public async Task<Unit> Delete(Guid CursoID)
        {
            return await mediator.Send(
                new Eliminar.DeleteCurso()
                {
                    CursoID = CursoID
                }
            );
        }
    }
}