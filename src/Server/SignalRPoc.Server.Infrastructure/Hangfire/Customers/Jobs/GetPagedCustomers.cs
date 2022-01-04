namespace SignalRPoc.Server.Infrastructure.Hangfire.Customers.Jobs;

using System.ComponentModel;
using global::Hangfire;
using MediatR;
using Microsoft.Extensions.Logging;
using SignalRPoc.Server.Application.Customers.Events;
using SignalRPoc.Server.Application.Customers.ViewModels;
using SignalRPoc.Server.Infrastructure.JsonStorage.Interfaces;

internal sealed class GetPagedCustomers
{
    private const int PAGE_SIZE = 100;

    private readonly ILogger<GetPagedCustomers> logger;
    private readonly IMediator mediator;
    private readonly ICustomerReadService service;

    public GetPagedCustomers(ILogger<GetPagedCustomers> logger, IMediator mediator, ICustomerReadService service)
    {
        this.logger = logger;
        this.mediator = mediator;
        this.service = service;
    }

    [DisplayName("Get customers by pages")]
    [AutomaticRetry(Attempts = 0)]
    public async Task Run()
    {
        this.logger.LogInformation("Getting customers by page");

        var customers = await this.service.GetCustomers();

        var totalCount = customers.CustomersList.Count;

        var pages = customers.CustomersList.Chunk(PAGE_SIZE);

        foreach (var page in pages)
        {
            var customersPage = new CustomersPage
            {
                Customers = page,
                PageSize = PAGE_SIZE,
                TotalCount = totalCount,
            };

            var notification = new GotCustomersPage
            {
                CustomersPage = customersPage,
            };

            await this.mediator.Publish(notification, CancellationToken.None);
        }
    }
}
