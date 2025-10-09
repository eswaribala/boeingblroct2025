using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Storage.Queues;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PolicyHolderFunction.Data;
using System.ComponentModel;

//var builder = FunctionsApplication.CreateBuilder(args);


var host = new HostBuilder()
    .ConfigureAppConfiguration(cfg =>
    {
        cfg.AddJsonFile("local.settings.json", optional: true)
           .AddEnvironmentVariables();
        var built = cfg.Build();
        var kvUri = built["KEYVAULT_URI"];
        if (!string.IsNullOrWhiteSpace(kvUri))
        {
            var cred = new DefaultAzureCredential();
            var sc = new SecretClient(new Uri(kvUri), cred);
            // Load Key Vault secrets as configuration keys (Cosmos--X -> Cosmos:X)
            cfg.AddAzureKeyVault(sc, new KeyVaultSecretManager());
        }
    })
    .ConfigureServices((ctx, services) =>
    {
        //var c = ctx.Configuration.GetSection("Cosmos");
        //var endpoint  = c["AccountEndpoint"]!;
        //var key       = c["Key"]!;
        //var database  = c["Database"]!;
        //var container = c["Container"]!;
        var cosmosOpts = new CosmosOptions();
        ctx.Configuration.GetSection("Cosmos").Bind(cosmosOpts);

        // Validate early (helpful in cold start)
        if (string.IsNullOrWhiteSpace(cosmosOpts.AccountEndpoint) ||
            string.IsNullOrWhiteSpace(cosmosOpts.Key) ||
            string.IsNullOrWhiteSpace(cosmosOpts.Database) ||
            string.IsNullOrWhiteSpace(cosmosOpts.Container))
        {
            throw new InvalidOperationException("Cosmos configuration is incomplete. Ensure Key Vault secrets exist and access is granted.");
        }

        // CosmosClient singleton
        var client = new CosmosClient(
            cosmosOpts.AccountEndpoint,
            cosmosOpts.Key,
            new CosmosClientOptions
            {
                ConnectionMode = ConnectionMode.Gateway,
                SerializerOptions = new CosmosSerializationOptions
                {
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                }

            } // or Direct


            );
       
        services.AddSingleton(client);
        var conn = ctx.Configuration["AzureWebJobsStorage"];
        services.AddSingleton(new QueueClient(conn, "policyholderqueue",
            new QueueClientOptions { MessageEncoding = QueueMessageEncoding.Base64 }));

        // 2) Container singleton (sync) — safe to register even if not created yet
        services.AddSingleton(sp =>
            sp.GetRequiredService<CosmosClient>().GetContainer(cosmosOpts.Database, cosmosOpts.Container));

        // app services
        services.AddScoped<IPolicyHolderRepository, PolicyHolderRepository>();
    })
    .ConfigureFunctionsWebApplication()   // for .NET isolated + ASP.NET Core integration
    .Build();

//
// 3) One-time startup initialization: create DB & container if missing
//
using (var scope = host.Services.CreateScope())
{
    var cfg = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    //var c = cfg.GetSection("Cosmos");
    //var database  = c["Database"]!;
    //var container = c["Container"]!;
    var cosmosOpts = new CosmosOptions();
    cfg.Bind(cosmosOpts);

    // Validate early (helpful in cold start)
    if (string.IsNullOrWhiteSpace(cosmosOpts.AccountEndpoint) ||
        string.IsNullOrWhiteSpace(cosmosOpts.Key) ||
        string.IsNullOrWhiteSpace(cosmosOpts.Database) ||
        string.IsNullOrWhiteSpace(cosmosOpts.Container))
    {
        throw new InvalidOperationException("Cosmos configuration is incomplete. Ensure Key Vault secrets exist and access is granted.");
    }
    var cosmos = scope.ServiceProvider.GetRequiredService<CosmosClient>();
    var dbResp = await cosmos.CreateDatabaseIfNotExistsAsync(cosmosOpts.Database);
    var db     = dbResp.Database;

    // Adjust partition key path to your model if needed
    await db.CreateContainerIfNotExistsAsync(cosmosOpts.Container, "/id");
}

await host.RunAsync();

