using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);


var host = new HostBuilder()
    .ConfigureAppConfiguration(cfg =>
    {
        cfg.AddJsonFile("local.settings.json", optional: true)
           .AddEnvironmentVariables();
    })
    .ConfigureServices((ctx, services) =>
    {
        var c = ctx.Configuration.GetSection("Cosmos");
        var endpoint = c["AccountEndpoint"]!;
        var key = c["Key"]!;
        var database = c["Database"]!;
        var container = c["Container"]!;

        // CosmosClient singleton
        var client = new CosmosClient(endpoint, key, new CosmosClientOptions
        {
            ConnectionMode = ConnectionMode.Gateway
        });
        services.AddSingleton(client);

        // Container singleton
        services.AddSingleton(sp =>
            sp.GetRequiredService<CosmosClient>().GetContainer(database, container));

        //services.AddSingleton<IClaimRepository, ClaimRepository>();
    })
    .ConfigureFunctionsWorkerDefaults()
    .Build();

await host.RunAsync();

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

builder.Build().Run();
