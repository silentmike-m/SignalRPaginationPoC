namespace SignalRPoc.Server.Infrastructure;

using System.Data.SqlClient;
using System.Reflection;
using global::Hangfire;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using SignalRPoc.Server.Infrastructure.JsonStorage;
using SignalRPoc.Server.Infrastructure.JsonStorage.Interfaces;
using SignalRPoc.Server.Infrastructure.JsonStorage.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, string hangFireServerName)
    {
        var hangfireConnectionString = configuration.GetConnectionString("HangfireConnection");

        services.Configure<JsonStorageOptions>(configuration.GetSection(JsonStorageOptions.SectionName));

        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddSingleton<IFileProvider>(new ManifestEmbeddedFileProvider(Assembly.GetExecutingAssembly()));

        services.AddSignalR();

        services.AddHangfire(config =>
        {
            config.UseSqlServerStorage(hangfireConnectionString);
        });

        services.AddHangfireServer(options =>
        {
            options.ServerName = hangFireServerName;
            options.WorkerCount = 1;
        });

        services.AddSingleton<ICustomerReadService, CustomerReadService>();
        services.AddSingleton<IMigrationService, MigrationService>();

        return services;
    }

    public static void UseInfrastructure(this IApplicationBuilder app, IConfiguration configuration, string hangFireServerName)
    {
        ProvideHangfireDatabase(configuration.GetConnectionString("HangfireConnection"));

        using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope();

        ClearHangfireServers(hangFireServerName, serviceScope);

        var migrationService = serviceScope.ServiceProvider.GetRequiredService<IMigrationService>();
        migrationService.MigrateStorage().Wait();
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
