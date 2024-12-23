using System.Net;
using Microsoft.Azure.Cosmos;

namespace Cosmium.EmbeddedServer.Tests.SdkCompatibilityTests;

public class CollectionTests : TestBase
{
    private Database databaseClient;
    
    [SetUp]
    public void Setup()
    {
        var databaseId = Guid.NewGuid().ToString();
        databaseClient = cosmosClient.GetDatabase(databaseId);

        serverInstance.CreateDatabase(databaseId);
    }

    [TearDown]
    public void TearDown()
    {
        serverInstance.DeleteDatabase(databaseClient.Id);
    }

    [Test]
    public async Task CreateContainerAsync_ShouldCreateCollection()
    {
        var collectionId = Guid.NewGuid().ToString();
        var partitionKeyPaths = new [] { "/_partitionKey" };

        var response = await databaseClient
            .CreateContainerAsync(new ContainerProperties(collectionId, partitionKeyPaths));
        var serverState = serverInstance.GetServerState();
        
        Assert.That(response, Is.Not.Null);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        
        Assert.That(serverState, Is.Not.Null);
        Assert.That(serverState.Collections[databaseClient.Id].ContainsKey(collectionId), Is.True);
    }

    [Test]
    public void CreateContainerAsync_ShouldFail_WhenCollectionAlreadyExists()
    {
        var collectionId = Guid.NewGuid().ToString();
        var partitionKeyPaths = new [] { "/_partitionKey" };
        serverInstance.GetDatabase(databaseClient.Id).CreateCollection(collectionId);

        var exception = Assert.ThrowsAsync<CosmosException>(() => databaseClient
            .CreateContainerAsync(new ContainerProperties(collectionId, partitionKeyPaths)));
        var serverState = serverInstance.GetServerState();
        
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
        
        Assert.That(serverState, Is.Not.Null);
        Assert.That(serverState.Collections[databaseClient.Id].ContainsKey(collectionId), Is.True);
    }

    [Test]
    public void CreateContainerAsync_ShouldFail_WhenDatabaseDoesNotExist()
    {
        var databaseId = Guid.NewGuid().ToString();
        var collectionId = Guid.NewGuid().ToString();
        var partitionKeyPaths = new [] { "/_partitionKey" };
        
        var exception = Assert.ThrowsAsync<CosmosException>(() => cosmosClient
            .GetDatabase(databaseId)
            .CreateContainerAsync(new ContainerProperties(collectionId, partitionKeyPaths)));
        var serverState = serverInstance.GetServerState();
        
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
        
        Assert.That(serverState, Is.Not.Null);
        Assert.That(serverState.Databases.ContainsKey(databaseId), Is.False);
        Assert.That(serverState.Collections.ContainsKey(databaseId), Is.False);
    }

    [Test]
    public async Task DeleteContainerAsync_ShouldDeleteCollection()
    {
        var collectionId = Guid.NewGuid().ToString();
        serverInstance.GetDatabase(databaseClient.Id).CreateCollection(collectionId);

        var response = await databaseClient.GetContainer(collectionId).DeleteContainerAsync();
        var serverState = serverInstance.GetServerState();
        
        Assert.That(response, Is.Not.Null);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        
        Assert.That(serverState, Is.Not.Null);
        Assert.That(serverState.Collections[databaseClient.Id].ContainsKey(collectionId), Is.False);
    }

    [Test]
    public void DeleteContainerAsync_ShouldFail_WhenCollectionDoesNotExist()
    {
        var collectionId = Guid.NewGuid().ToString();
        
        var exception = Assert.ThrowsAsync<CosmosException>(() => databaseClient.GetContainer(collectionId).DeleteContainerAsync());
        var serverState = serverInstance.GetServerState();
        
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        
        Assert.That(serverState, Is.Not.Null);
        Assert.That(serverState.Collections[databaseClient.Id].ContainsKey(collectionId), Is.False);
    }

    [Test]
    public void DeleteContainerAsync_ShouldFail_WhenDatabaseDoesNotExist()
    {
        var databaseId = Guid.NewGuid().ToString();
        var collectionId = Guid.NewGuid().ToString();
        
        var exception = Assert.ThrowsAsync<CosmosException>(() => cosmosClient.GetDatabase(databaseId).GetContainer(collectionId).DeleteContainerAsync());
        var serverState = serverInstance.GetServerState();
        
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        
        Assert.That(serverState, Is.Not.Null);
        Assert.That(serverState.Databases.ContainsKey(databaseId), Is.False);
    }

    [Test]
    public async Task ReadContainerAsync_ShouldReadCollection()
    {
        var collectionId = Guid.NewGuid().ToString();
        serverInstance.GetDatabase(databaseClient.Id).CreateCollection(collectionId);
        
        var result = await databaseClient.GetContainer(collectionId).ReadContainerAsync();
        var serverState = serverInstance.GetServerState();
        var actualCollection = serverState?.Collections[databaseClient.Id].GetValueOrDefault(collectionId);
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
        Assert.That(actualCollection, Is.Not.Null);
        Assert.That(result.Resource.Id, Is.EqualTo(actualCollection.Id));
        Assert.That(result.Resource.ETag, Is.EqualTo(actualCollection.ETag));
    }
}
