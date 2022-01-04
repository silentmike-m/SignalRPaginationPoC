namespace SignalRPoc.Server.Infrastructure.Hangfire;

using System.Data.SqlClient;
using global::Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

internal static class DependencyInjection
{
    public static void AddHangfire(this IServiceCollection services, IConfiguration configuration, string hangFireServerName)
    {
        var hangfireConnectionString = configuration.GetConnectionString("HangfireConnection");

        services.AddHangfire(config =>
        {
            config.UseSqlServerStorage(hangfireConnectionString);
        });

        services.AddHangfireServer(options =>
        {
            options.ServerName = hangFireServerName;
            options.WorkerCount = 1;
        });
    }

    public static void UseHangfire(IConfiguration configuration, string hangFireServerName, IServiceScope serviceScope)
    {
        var hangfireConnectionString = configuration.GetConnectionString("HangfireConnection");

        ProvideHangfireDatabase(hangfireConnectionString);

        ClearHangfireServers(hangFireServerName, serviceScope);
    }

    private static void ProvideHangfireDatabase(string connectionString)
    {
        var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
        var catalog = connectionStringBuilder.InitialCatalog;
        connectionStringBuilder.InitialCatalog = string.Empty;

        using var connection = new SqlConnection(connectionStringBuilder.ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = $"IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '{catalog}') CREATE DATABASE {catalog}";
        command.ExecuteNonQuery();
    }

    private static void ClearHangfireServers(string hangFireServerName, IServiceScope serviceScope)
    {
        var jobStorage = serviceScope.ServiceProvider.GetRequiredService<JobStorage>();

        var serversToDelete = jobStorage
            .GetMonitoringApi()
            .Servers()
            .Where(i => !i.Name.Contains(hangFireServerName, StringComparison.InvariantCultureIgnoreCase));

        foreach (var server in serversToDelete)
        {
            JobStorage.Current.GetConnection().RemoveServer(server.Name);
        }
    }
}
