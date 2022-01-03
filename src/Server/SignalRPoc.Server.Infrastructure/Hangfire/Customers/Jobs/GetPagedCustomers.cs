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

    [DisplayName("Get customers by parts")]
    [AutomaticRetry(Attempts = 0)]
    public async Task Run()
    {
        this.logger.LogInformation("Getting customers by page");

        var customers = await this.service.GetCustomers();

        for (var i = 0; i < customers.CustomersList.Count; i += PAGE_SIZE)
        {
            var part = customers.CustomersList
                .Skip(i)
                .Take(PAGE_SIZE)
                .ToList();

            var customersPage = new CustomersPage
            {
                Customers = part,
                PageSize = PAGE_SIZE,
                TotalCount = customers.CustomersList.Count,
            };

            var notification = new GotCustomersPage
            {
                CustomersPage = customersPage,
            };

            await this.mediator.Publish(notification, CancellationToken.None);
        }
    }
}
