namespace SignalRPoc.Server.Infrastructure.Hangfire.Customers.QueryHandlers;

using global::Hangfire;
using MediatR;
using Microsoft.Extensions.Logging;

internal sealed class GetPagedCustomersHandler : IRequestHandler<Application.Customers.Queries.GetPagedCustomers>
{
    private readonly IBackgroundJobClient jobClient;
    private readonly ILogger<GetPagedCustomersHandler> logger;

    public GetPagedCustomersHandler(IBackgroundJobClient jobClient, ILogger<GetPagedCustomersHandler> logger)
    {
        this.jobClient = jobClient;
        this.logger = logger;
    }

    public async Task<Unit> Handle(Application.Customers.Queries.GetPagedCustomers request, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Try to get paged customers");

        this.jobClient.Enqueue((Jobs.GetPagedCustomers i) => i.Run());

        return await Task.FromResult(Unit.Value);
    }
}
