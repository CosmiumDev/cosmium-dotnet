using System.Net;

namespace Cosmium.EmbeddedServer.Tests;

public class Tests : TestBase
{
    [Test]
    public async Task CreateDatabaseAsync_ShouldCreateDatabase()
    {
        var databaseId = Guid.NewGuid().ToString();

        var createDatabaseResponse = await cosmosClient.CreateDatabaseAsync(databaseId);
        var serverState = serverInstance.GetServerState();

        Assert.That(createDatabaseResponse, Is.Not.Null);
        Assert.That(createDatabaseResponse.Resource.Id, Is.EqualTo(databaseId));
        Assert.That(createDatabaseResponse.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        
        Assert.That(serverState, Is.Not.Null);
        Assert.That(serverState.Databases.Count, Is.EqualTo(1));

        Assert.That(serverState.Databases.ContainsKey(databaseId), Is.True);
    }
}
