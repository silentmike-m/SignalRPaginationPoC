namespace SignalRPoc.Server.WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using SignalRPoc.Server.Application.Common;
using SignalRPoc.Server.Application.Customers.Queries;
using SignalRPoc.Server.Application.Customers.ViewModels;

[ApiController, Route("[controller]/[action]")]
public sealed class CustomersController : ApiControllerBase
{
    [HttpPost(Name = "GetCustomers")]
    public async Task<BaseResponse<Customers>> GetCustomers()
    {
        var request = new GetCustomers();

        var response = await this.Mediator.Send(request, CancellationToken.None);

        return new BaseResponse<Customers>(response);
    }
}