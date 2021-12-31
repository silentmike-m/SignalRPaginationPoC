namespace SignalRPoc.Server.Infrastructure.JsonStorage.Services;

using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SignalRPoc.Server.Infrastructure.JsonStorage.Constants;
using SignalRPoc.Server.Infrastructure.JsonStorage.Interfaces;

public sealed class MigrationService : IMigrationService
{
    private readonly IFileProvider fileProvider;
    private readonly ILogger<MigrationService> logger;
    private readonly JsonStorageOptions options;

    public MigrationService(IFileProvider fileProvider, ILogger<MigrationService> logger, IOptions<JsonStorageOptions> options)
    {
        this.fileProvider = fileProvider;
        this.logger = logger;
        this.options = options.Value;
    }

    public async Task MigrateStorage(CancellationToken cancellationToken = default)
    {
        this.logger.LogInformation("Try to migrate json storage");

        if (!Directory.Exists(this.options.Path))
        {
            Directory.CreateDirectory(this.options.Path);
        }

        await this.MigrateCustomers(cancellationToken);

        this.logger.LogInformation("Migrated json storage");
    }

    private async Task MigrateCustomers(CancellationToken cancellationToken)
    {
        var filePath = Path.Combine(this.options.Path, JsonStorageFileNames.CUSTOMERS_FILE_NAME);

        if (File.Exists(filePath))
        {
            this.logger.LogInformation("Customers json storage exists");
            return;
        }


        await using var stream = this.fileProvider.GetFileInfo(JsonStorageFileNames.CUSTOMERS_FILE_NAME).CreateReadStream();
        await using var file = new FileStream(filePath, FileMode.Create, FileAccess.Write);

        await stream.CopyToAsync(file, cancellationToken);

        this.logger.LogInformation("Migrated customers json storage");
    }
}
