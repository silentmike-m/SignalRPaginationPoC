using Microsoft.Extensions.Configuration;
using Serilog;
using SignalRPoc.Client;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

try
{
    Log.Information("Starting client...");

    await new SignalRClient(configuration).StartConnection();
}
catch (Exception exception)
{
    Log.Fatal(exception, "Client terminated unexpectedly.");
}
finally
{
    Log.CloseAndFlush();
    Console.ReadKey();
}