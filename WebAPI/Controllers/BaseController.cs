using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private IMediator? _mediator;

        //se tiene que instanciar _mediator
        protected IMediator mediator => _mediator ?? (_mediator = HttpContext.RequestServices.GetService<IMediator>()!);
    }
}