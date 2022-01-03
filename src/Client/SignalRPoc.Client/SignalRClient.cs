namespace SignalRPoc.Client;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Serilog;
using SignalRPoc.Client.Models;

public sealed class SignalRClient
{
    private const string MESSAGE = "GotCustomersPage";

    private readonly List<Customer> customers;
    private readonly SignalROptions options;

    public SignalRClient(IConfiguration configuration)
    {
        this.customers = new List<Customer>();
        this.options = configuration.GetSection(SignalROptions.SectionName).Get<SignalROptions>();
    }

    public async Task StartConnection()
    {
        await this.StartHubConnection();
    }

    private async Task StartHubConnection()
    {
        var connection = new HubConnectionBuilder()
            .WithUrl(this.options.Url)
            .WithAutomaticReconnect()
            .Build();

        await connection.StartAsync();

        Log.Information("SignalR hub connected");

        connection.On<BaseResponse<CustomersPage>>(MESSAGE, HandleCustomersPage);
    }

    private void HandleCustomersPage(BaseResponse<CustomersPage> response)
    {
        if (string.IsNullOrEmpty(response.Error))
        {
            var customersPage = response.Response;

            customers.AddRange(customersPage!.Customers);

            Console.WriteLine($"Receive {this.customers.Count} of {customersPage.TotalCount}");

            if (this.customers.Count == customersPage.TotalCount)
            {
                Console.WriteLine("Received last page");
            }
        }
        else
        {
            Console.WriteLine($"{response.Code} : {response.Error}");
        }
    }
}
