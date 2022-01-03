namespace SignalRPoc.Server.Infrastructure.JsonStorage.Interfaces;

public interface IMigrationService
{
    Task MigrateStorage(CancellationToken cancellationToken = default);
}
