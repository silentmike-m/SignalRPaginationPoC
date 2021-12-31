namespace SignalRPoc.Server.Application.Customers.Queries;

using MediatR;
using SignalRPoc.Server.Application.Customers.ViewModels;

public sealed record GetCustomers : IRequest<Customers>
{

}
