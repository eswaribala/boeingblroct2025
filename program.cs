using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PolicyHolderFunction.Data;
using PolicyHolderFunction.Models;
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
        // Bind options from configuration (which now includes Key Vault)
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
            new CosmosClientOptions {
                ConnectionMode = ConnectionMode.Gateway,
                 SerializerOptions = new CosmosSerializationOptions
                 {
                     PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                 }

            } // or Direct


            );

        // 1) CosmosClient singleton
        //var client = new CosmosClient(endpoint, key, new CosmosClientOptions
        //{
        //    ConnectionMode = ConnectionMode.Gateway,
        //    SerializerOptions = new CosmosSerializationOptions
        //    {
        //        PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
        //    }
        //});
        services.AddSingleton(client);

        // 2) Container singleton (sync) — safe to register even if not created yet
        services.AddSingleton(sp =>
            sp.GetRequiredService<CosmosClient>().GetContainer(cosmosOpts.Database,cosmosOpts.Container));

        // app services
        services.AddScoped<IPolicyHolderRepository, PolicyHolderRepository>();

        var jwt = new JwtOptions();
        ctx.Configuration.GetSection("JWT").Bind(jwt);

        if (string.IsNullOrWhiteSpace(jwt.Secret) ||
            string.IsNullOrWhiteSpace(jwt.Issuer) ||
            string.IsNullOrWhiteSpace(jwt.Audience))
        {
            throw new InvalidOperationException("JWT config missing. Set JWT:Issuer, JWT:Audience, JWT:Secret.");
        }

        services.AddSingleton(jwt);
        services.AddSingleton<IJwtIssuer, LocalJwtIssuer>();
        services.AddSingleton<IJwtValidator, LocalJwtValidator>();

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
    //var database  = c["Database"]!;
    //var container = c["Container"]!;
    var cosmosOpts = new CosmosOptions();
    c.Bind(cosmosOpts);

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
