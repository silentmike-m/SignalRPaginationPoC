namespace SignalRPoc.Server.Infrastructure.SignalR.Customers;

using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SignalRPoc.Server.Application.Common;
using SignalRPoc.Server.Application.Customers.Events;
using SignalRPoc.Server.Application.Customers.ViewModels;
using SignalRPoc.Server.Infrastructure.SignalR.Hubs;

internal sealed class GotCustomersPageHandler : INotificationHandler<GotCustomersPage>
{
    private readonly IHubContext<CustomersHub> hubContext;
    private readonly ILogger<GotCustomersPageHandler> logger;

    public GotCustomersPageHandler(IHubContext<CustomersHub> hubContext, ILogger<GotCustomersPageHandler> logger)
    {
        this.hubContext = hubContext;
        this.logger = logger;
    }

    public async Task Handle(GotCustomersPage notification, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Sending customers page");

        var response = new BaseResponse<CustomersPage>
        {
            Response = notification.CustomersPage,
        };

        await this.hubContext.Clients.All
            .SendAsync("GotCustomersPage", response, cancellationToken)
            .ConfigureAwait(false);
    }
}
