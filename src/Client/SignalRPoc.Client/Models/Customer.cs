namespace SignalRPoc.Client.Models;

using System.Text.Json.Serialization;

public sealed record Customer
{
    [JsonPropertyName("first_name")] public string FirstName { get; init; } = string.Empty;
    [JsonPropertyName("last_name")] public string LastName { get; init; } = string.Empty;
    [JsonPropertyName("company_name")] public string CompanyName { get; init; } = string.Empty;
    [JsonPropertyName("address")] public string Address { get; init; } = string.Empty;
    [JsonPropertyName("city")] public string City { get; init; } = string.Empty;
    [JsonPropertyName("county")] public string County { get; init; } = string.Empty;
    [JsonPropertyName("phone")] public string Phone { get; init; } = string.Empty;
    [JsonPropertyName("email")] public string Email { get; init; } = string.Empty;
}
