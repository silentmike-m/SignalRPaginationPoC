using Hangfire;
using Serilog;
using SignalRPoc.Server.Application;
using SignalRPoc.Server.Infrastructure;
using SignalRPoc.Server.Infrastructure.SignalR.Hubs;
using SignalRPoc.Server.WebApi.Filters;

var hangFireServerName = $"Default:{Guid.NewGuid()}";

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

try
{
    Log.Information("Starting host...");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host
        .UseSerilog()
        .ConfigureAppConfiguration((_, config) => { config.AddEnvironmentVariables(prefix: "CONFIG_"); })
        ;

    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration, hangFireServerName);

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    app.UseInfrastructure(builder.Configuration, hangFireServerName);

    app.UseHangfireDashboard("/hangfire", new DashboardOptions
    {
        Authorization = new[] { new HangfireAuthorizationFilter() },
    });

    app.UseSwagger();
    app.UseSwaggerUI();

    app.MapHub<CustomersHub>(CustomersHub.Pattern);

    app.MapControllers();

    await app.RunAsync();
}
catch (Exception exception)
{
    Log.Fatal(exception, "Host terminated unexpectedly.");
}
finally
{
    Log.CloseAndFlush();
}


