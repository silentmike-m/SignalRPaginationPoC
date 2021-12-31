namespace SignalRPoc.Server.Infrastructure.JsonStorage.Interfaces;

using SignalRPoc.Server.Application.Customers.ViewModels;

internal interface ICustomerReadService
{
    Task<Customers> GetCustomers(CancellationToken cancellationToken = default);
}