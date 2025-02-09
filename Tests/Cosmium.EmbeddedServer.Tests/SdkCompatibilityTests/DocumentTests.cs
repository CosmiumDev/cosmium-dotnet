using System.Net;
using Cosmium.EmbeddedServer.Tests.TestModels;
using Microsoft.Azure.Cosmos;

namespace Cosmium.EmbeddedServer.Tests.SdkCompatibilityTests;

public class DocumentTests : TestBase
{
    private Database databaseClient;
    private Container collectionClient;

    [SetUp]
    public void Setup()
    {
        var databaseId = Guid.NewGuid().ToString();
        databaseClient = cosmosClient.GetDatabase(databaseId);

        var collectionId = Guid.NewGuid().ToString();
        collectionClient = databaseClient.GetContainer(collectionId);

        serverInstance.CreateDatabase(databaseId).CreateCollection(collectionId);
    }

    [TearDown]
    public void TearDown()
    {
        serverInstance.DeleteDatabase(databaseClient.Id);
    }

    [Test]
    public async Task CreateItemAsync_ShouldCreateDocument()
    {
        var partitionKey = new PartitionKey(123);
        var document = new TestDocument
        {
            id = Guid.NewGuid().ToString(),
            name = "test",
        };

        var result = await collectionClient.CreateItemAsync(document, partitionKey);
        var serverState = serverInstance.GetServerState();
        var createdDocument = serverState?.Documents
            .GetValueOrDefault(databaseClient.Id)?
            .GetValueOrDefault(collectionClient.Id)?
            .GetValueOrDefault(document.id);
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        
        Assert.That(createdDocument, Is.Not.Null);
        Assert.That(createdDocument.Properties["id"].ToString(), Is.EqualTo(document.id));
        Assert.That(createdDocument.Properties["name"].ToString(), Is.EqualTo(document.name));
    }

    [Test]
    public void CreateItemAsync_ShouldFail_WhenDocumentAlreadyExists()
    {
        var partitionKey = new PartitionKey(123);
        var document = new TestDocument
        {
            id = Guid.NewGuid().ToString(),
            name = "test",
        };
        serverInstance.GetDatabase(databaseClient.Id)
            .GetCollection(collectionClient.Id)
            .CreateDocument(document.id, document);
        
        var exception = Assert.ThrowsAsync<CosmosException>(() => collectionClient.CreateItemAsync(document, partitionKey));

        Assert.That(exception, Is.Not.Null);
        Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
    }

    [Test]
    public void CreateItemAsync_ShouldFail_WhenCollectionDoesNotExist()
    {
        var partitionKey = new PartitionKey(123);
        var document = new TestDocument
        {
            id = Guid.NewGuid().ToString(),
            name = "test",
        };
        collectionClient = databaseClient.GetContainer(Guid.NewGuid().ToString());
        
        var exception = Assert.ThrowsAsync<CosmosException>(() => collectionClient.CreateItemAsync(document, partitionKey));
        
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
    }

    [Test]
    public void CreateItemAsync_ShouldFail_WhenDatabaseDoesNotExist()
    {
        var partitionKey = new PartitionKey(123);
        var document = new TestDocument
        {
            id = Guid.NewGuid().ToString(),
            name = "test",
        };
        databaseClient = cosmosClient.GetDatabase(Guid.NewGuid().ToString());
        collectionClient = databaseClient.GetContainer(collectionClient.Id);
        
        var exception = Assert.ThrowsAsync<CosmosException>(() => collectionClient.CreateItemAsync(document, partitionKey));
        
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
    }

    [Test]
    public async Task ReplaceItemAsync_ShouldUpdateDocument()
    {
        var partitionKey = new PartitionKey(123);
        var document = new TestDocument
        {
            id = Guid.NewGuid().ToString(),
            name = "test",
        };
        serverInstance.GetDatabase(databaseClient.Id)
            .GetCollection(collectionClient.Id)
            .CreateDocument(document.id, document);
        document.name = "updated";

        var result = await collectionClient.ReplaceItemAsync(document, document.id, partitionKey);
        var serverState = serverInstance.GetServerState();
        var updatedDocument = serverState?.Documents
            .GetValueOrDefault(databaseClient.Id)?
            .GetValueOrDefault(collectionClient.Id)?
            .GetValueOrDefault(document.id);
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        
        Assert.That(updatedDocument, Is.Not.Null);
        Assert.That(updatedDocument.Properties["id"].ToString(), Is.EqualTo(document.id));
        Assert.That(updatedDocument.Properties["name"].ToString(), Is.EqualTo(document.name));
    }

    [Test]
    public async Task ReadItemAsync_ShouldReturnItem()
    {
        var partitionKey = new PartitionKey(123);
        var document = new TestDocument
        {
            id = Guid.NewGuid().ToString(),
            name = "test",
        };
        serverInstance.GetDatabase(databaseClient.Id)
            .GetCollection(collectionClient.Id)
            .CreateDocument(document.id, document);
        
        var result = await collectionClient.ReadItemAsync<TestDocument>(document.id, partitionKey);
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(result.Resource.id, Is.EqualTo(document.id));
        Assert.That(result.Resource.name, Is.EqualTo(document.name));
    }

    [Test]
    public void ReadItemAsync_ShouldFail_WhenDocumentDoesNotExist()
    {
        var partitionKey = new PartitionKey(123);
        var documentId = Guid.NewGuid().ToString();

        var exception = Assert.ThrowsAsync<CosmosException>(() => collectionClient.ReadItemAsync<TestDocument>(documentId, partitionKey));
        
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public void ReadItemAsync_ShouldFail_WhenCollectionDoesNotExist()
    {
        var partitionKey = new PartitionKey(123);
        var documentId = Guid.NewGuid().ToString();
        collectionClient = databaseClient.GetContainer(Guid.NewGuid().ToString());
        
        var exception = Assert.ThrowsAsync<CosmosException>(() => collectionClient.ReadItemAsync<TestDocument>(documentId, partitionKey));
        
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public void ReadItemAsync_ShouldFail_WhenDatabaseDoesNotExist()
    {
        var partitionKey = new PartitionKey(123);
        var documentId = Guid.NewGuid().ToString();
        databaseClient = cosmosClient.GetDatabase(Guid.NewGuid().ToString());
        collectionClient = databaseClient.GetContainer(collectionClient.Id);
        
        var exception = Assert.ThrowsAsync<CosmosException>(() => collectionClient.ReadItemAsync<TestDocument>(documentId, partitionKey));
        
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
}
