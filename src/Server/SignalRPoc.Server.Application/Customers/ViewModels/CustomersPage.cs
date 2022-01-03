namespace SignalRPoc.Server.Application.Customers.ViewModels;

using System.Text.Json.Serialization;

public sealed record CustomersPage
{
    [JsonPropertyName("customers")] public IReadOnlyList<Customer> Customers { get; init; } = new List<Customer>().AsReadOnly();
    [JsonPropertyName("page_size")] public int PageSize { get; init; } = default;
    [JsonPropertyName("total_count")] public int TotalCount { get; init; } = default;

}
