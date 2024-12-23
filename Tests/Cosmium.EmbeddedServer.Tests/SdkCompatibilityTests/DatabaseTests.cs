using System.Net;
using Microsoft.Azure.Cosmos;

namespace Cosmium.EmbeddedServer.Tests.SdkCompatibilityTests;

public class DatabaseTests : TestBase
{
    [Test]
    public async Task CreateDatabaseAsync_ShouldCreateDatabase()
    {
        var databaseId = Guid.NewGuid().ToString();

        var response = await cosmosClient.CreateDatabaseAsync(databaseId);
        var serverState = serverInstance.GetServerState();

        Assert.That(response, Is.Not.Null);
        Assert.That(response.Resource.Id, Is.EqualTo(databaseId));
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        
        Assert.That(serverState, Is.Not.Null);
        Assert.That(serverState.Databases.ContainsKey(databaseId), Is.True);
    }

    [Test]
    public void CreateDatabaseAsync_ShouldFail_WhenDatabaseAlreadyExists()
    {
        var databaseId = Guid.NewGuid().ToString();
        serverInstance.CreateDatabase(databaseId);
        
        var exception = Assert.ThrowsAsync<CosmosException>(() => cosmosClient.CreateDatabaseAsync(databaseId));
        var serverState = serverInstance.GetServerState();
        
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
        
        Assert.That(serverState, Is.Not.Null);
        Assert.That(serverState.Databases.ContainsKey(databaseId), Is.True);
    }

    [Test]
    public async Task DeleteAsync_ShouldDeleteDatabase()
    {
        var databaseId = Guid.NewGuid().ToString();
        serverInstance.CreateDatabase(databaseId);

        var response = await cosmosClient.GetDatabase(databaseId).DeleteAsync();
        var serverState = serverInstance.GetServerState();
        
        Assert.That(response, Is.Not.Null);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));

        Assert.That(serverState, Is.Not.Null);
        Assert.That(serverState.Databases.ContainsKey(databaseId), Is.False);
    }

    [Test]
    public void DeleteAsync_ShouldFail_WhenDatabaseDoesNotExist()
    {
        var databaseId = Guid.NewGuid().ToString();
        
        var exception = Assert.ThrowsAsync<CosmosException>(() => cosmosClient.GetDatabase(databaseId).DeleteAsync());
        var serverState = serverInstance.GetServerState();
        
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        
        Assert.That(serverState, Is.Not.Null);
        Assert.That(serverState.Databases.ContainsKey(databaseId), Is.False);
    }

    [Test]
    public async Task ReadAsync_ShouldReturnDatabase()
    {
        var databaseId = Guid.NewGuid().ToString();
        serverInstance.CreateDatabase(databaseId);
        var serverState = serverInstance.GetServerState();
        var actualDatabase = serverState?.Databases.GetValueOrDefault(databaseId);
        
        var response = await cosmosClient.GetDatabase(databaseId).ReadAsync();
        
        Assert.That(response, Is.Not.Null);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        Assert.That(actualDatabase, Is.Not.Null);
        Assert.That(response.Resource.Id, Is.EqualTo(databaseId));
        Assert.That(response.Resource.ETag, Is.EqualTo(actualDatabase.ETag));
    }

    [Test]
    public void ReadAsync_ShouldFail_WhenDatabaseDoesNotExist()
    {
        var databaseId = Guid.NewGuid().ToString();
        
        var exception = Assert.ThrowsAsync<CosmosException>(() => cosmosClient.GetDatabase(databaseId).ReadAsync());
        var serverState = serverInstance.GetServerState();
        
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        
        Assert.That(serverState, Is.Not.Null);
        Assert.That(serverState.Databases.ContainsKey(databaseId), Is.False);
    }
}
