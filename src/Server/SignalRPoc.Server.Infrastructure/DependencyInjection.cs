namespace SignalRPoc.Server.Infrastructure;

using System.Reflection;
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
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JsonStorageOptions>(configuration.GetSection(JsonStorageOptions.SectionName));

        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddSingleton<IFileProvider>(new ManifestEmbeddedFileProvider(Assembly.GetExecutingAssembly()));

        services.AddSingleton<ICustomerReadService, CustomerReadService>();
        services.AddSingleton<IMigrationService, MigrationService>();

        return services;
    }

    public static void UseInfrastructure(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope();
        var migrationService = serviceScope.ServiceProvider.GetRequiredService<IMigrationService>();
        migrationService.MigrateStorage().Wait();
    }

}
