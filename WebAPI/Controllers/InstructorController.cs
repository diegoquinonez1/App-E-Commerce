using Dominio;
using Aplicacion.Instructores;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace WebAPI.Controllers
{
    [ApiController]
    public class InstructorController : BaseController
    {
        public InstructorController()
        { }

        [HttpGet]
        public async Task<List<Instructor>> GetAll()
        {
            return await mediator.Send(new Consulta.GetAllInstructores());
        }

        [HttpGet("{InstructorID}")]
        public async Task<Instructor> GetInstructor(Guid InstructorID)
        {
            return await mediator.Send(
                new Consulta.GetInstructor()
                {
                    InstructorID = InstructorID
                }
            );
        }

        [HttpPost]
        public async Task<Unit> Post(Creacion.InsertInstructor data)
        {
            return await mediator.Send(data);
        }
    }
}