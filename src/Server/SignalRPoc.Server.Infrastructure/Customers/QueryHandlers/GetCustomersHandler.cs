namespace SignalRPoc.Server.Infrastructure.Customers.QueryHandlers;

using MediatR;
using Microsoft.Extensions.Logging;
using SignalRPoc.Server.Application.Customers.Queries;
using SignalRPoc.Server.Application.Customers.ViewModels;
using SignalRPoc.Server.Infrastructure.JsonStorage.Interfaces;

internal sealed class GetCustomersHandler : IRequestHandler<GetCustomers, Customers>
{
    private readonly ILogger<GetCustomersHandler> logger;
    private readonly ICustomerReadService service;

    public GetCustomersHandler(ILogger<GetCustomersHandler> logger, ICustomerReadService service)
    {
        this.logger = logger;
        this.service = service;
    }

    public async Task<Customers> Handle(GetCustomers request, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Try to get all customers");

        return await this.service.GetCustomers(cancellationToken);
    }
}
