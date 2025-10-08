// See https://aka.ms/new-console-template for more information
using Microsoft.Azure.Cosmos;

using System;
using System.Collections.Concurrent;

using System.Threading.Tasks;

class Program
{
    private static readonly string EndpointUri = "";
    private static readonly string PrimaryKey = "";

    private CosmosClient cosmosClient;

    private Database database;
    private  Container container;

    private string databaseId = "CustomerDB";
    private string containerId = "CustomerContainer";

    static async Task Main(string[] args)
    {
        Program p = new Program();
        await p.RunAsync();
    }

    private async Task RunAsync()
    {
        cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);

        // Create DB and container if not exists
        database = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
        Console.WriteLine($"Database ready: {database.Id}");

        container = await database.CreateContainerIfNotExistsAsync(containerId, "/id");
         Console.WriteLine($"Container ready: {container.Id}");

        // Create sample item
        var item = new { id = Guid.NewGuid().ToString(), Name = "Genius", Type = "Microservices" };
        await container.CreateItemAsync(item, new PartitionKey(item.id));
        Console.WriteLine("✅ Inserted item!");

        // Query items
        var sql = "SELECT * FROM c";
        var iterator = container.GetItemQueryIterator<dynamic>(sql);
        var results = await iterator.ReadNextAsync();

        foreach (var doc in results)
        {
            Console.WriteLine($"Found: {doc}");
        }
    }
}
