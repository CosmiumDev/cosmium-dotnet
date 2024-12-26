using Microsoft.Azure.Cosmos;
using TodoApp.Contracts;

namespace TodoApp.Services;

public class CosmosClientProvider(string connectionString) : ICosmosClientProvider
{
    private CosmosClient _client;

    public CosmosClient GetClient()
    {
        return _client ??= new CosmosClient(connectionString);
    }
}
