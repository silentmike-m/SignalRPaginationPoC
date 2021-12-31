namespace SignalRPoc.Server.WebApi.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController, Route("[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    private ISender? mediator;
    protected ISender Mediator => this.mediator ??= HttpContext.RequestServices.GetService<ISender>();
}