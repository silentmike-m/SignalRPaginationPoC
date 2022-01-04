namespace SignalRPoc.Server.Infrastructure;
using System.Reflection;
using global::Hangfire;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using SignalRPoc.Server.Infrastructure.Hangfire;
using SignalRPoc.Server.Infrastructure.JsonStorage;
using SignalRPoc.Server.Infrastructure.JsonStorage.Interfaces;
using SignalRPoc.Server.Infrastructure.JsonStorage.Services;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration, string hangFireServerName)
    {
        services.Configure<JsonStorageOptions>(configuration.GetSection(JsonStorageOptions.SectionName));

        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddSingleton<IFileProvider>(new ManifestEmbeddedFileProvider(Assembly.GetExecutingAssembly()));

        services.AddSignalR();

        services.AddHangfire(configuration, hangFireServerName);

        services.AddSingleton<ICustomerReadService, CustomerReadService>();
        services.AddSingleton<IMigrationService, MigrationService>();
    }

    public static void UseInfrastructure(this IApplicationBuilder app, IConfiguration configuration, string hangFireServerName)
    {
        using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope();

        Hangfire.DependencyInjection.UseHangfire(configuration, hangFireServerName, serviceScope);

        var migrationService = serviceScope.ServiceProvider.GetRequiredService<IMigrationService>();
        migrationService.MigrateStorage().Wait();
    }
}
