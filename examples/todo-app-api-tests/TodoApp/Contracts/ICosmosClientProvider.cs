using Microsoft.Azure.Cosmos;

namespace TodoApp.Contracts;

public interface ICosmosClientProvider
{
    CosmosClient GetClient();
}
