namespace SignalRPoc.Server.Domain.Entities;

public sealed class CustomerEntity
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Company { get; init; } = string.Empty;
    public string Address { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string County { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
}
