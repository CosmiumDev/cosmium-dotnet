namespace Cosmium.EmbeddedServer.Contracts
{
    public interface IDatabaseClient
    {
        ICollectionClient CreateCollection(string collectionName);
        ICollectionClient GetCollection(string collectionName);
        bool DeleteCollection(string collectionName);
    }
}
