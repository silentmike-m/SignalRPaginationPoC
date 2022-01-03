namespace SignalRPoc.Server.Infrastructure.SignalR.Hubs;

using Microsoft.AspNetCore.SignalR;

public sealed class CustomersHub : Hub
{
    public static readonly string Pattern = "/customerHub";
}
