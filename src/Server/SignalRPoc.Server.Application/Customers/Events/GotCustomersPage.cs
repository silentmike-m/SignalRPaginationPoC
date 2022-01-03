namespace SignalRPoc.Server.Application.Customers.Events;

using System.Text.Json.Serialization;
using MediatR;
using SignalRPoc.Server.Application.Customers.ViewModels;

public sealed record GotCustomersPage : INotification
{
    [JsonPropertyName("customers_page")] public CustomersPage CustomersPage { get; init; } = new();
}
