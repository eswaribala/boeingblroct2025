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
    })
    .ConfigureServices((ctx, services) =>
    {
        var c = ctx.Configuration.GetSection("Cosmos");
        var endpoint  = c["AccountEndpoint"]!;
        var key       = c["Key"]!;
        var database  = c["Database"]!;
        var container = c["Container"]!;

        // 1) CosmosClient singleton
        var client = new CosmosClient(endpoint, key, new CosmosClientOptions
        {
            ConnectionMode = ConnectionMode.Gateway,
            SerializerOptions = new CosmosSerializationOptions
            {
                PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
            }
        });
        services.AddSingleton(client);
        var conn = ctx.Configuration["AzureWebJobsStorage"];
        services.AddSingleton(new QueueClient(conn, "policyholderqueue",
            new QueueClientOptions { MessageEncoding = QueueMessageEncoding.Base64 }));

        // 2) Container singleton (sync) — safe to register even if not created yet
        services.AddSingleton(sp =>
            sp.GetRequiredService<CosmosClient>().GetContainer(database, container));

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
    var c = cfg.GetSection("Cosmos");
    var database  = c["Database"]!;
    var container = c["Container"]!;

    var cosmos = scope.ServiceProvider.GetRequiredService<CosmosClient>();
    var dbResp = await cosmos.CreateDatabaseIfNotExistsAsync(database);
    var db     = dbResp.Database;

    // Adjust partition key path to your model if needed
    await db.CreateContainerIfNotExistsAsync(container, "/id");
}

await host.RunAsync();

