using System.Net;
using System.Net.Http.Json;
using Cosmium.EmbeddedServer.Clients;
using Cosmium.EmbeddedServer.Contracts;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using TodoApp.Contracts;
using TodoApp.Models;

namespace TodoApp.Tests;

[TestFixture]
public class TodoControllerTests
{
    private HttpClient _client;
    private CosmosClient _cosmosClient;
    private WebApplicationFactory<Program> _factory;
    private ServerInstance _serverInstance;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        // If you plan on running tests in parallel you will want the database to have a unique name and port for each instance
        var serverName = Guid.NewGuid().ToString();
        var randomPort = new Random().Next(10000, 20000);
        var serverConfiguration = new ServerConfiguration
        {
            Port = randomPort,
        };

        // Initialize cosmium database emulator
        _serverInstance = new ServerInstance(serverName, serverConfiguration);
        
        // Initialize cosmos client and point to emulator
        _cosmosClient = (new CosmosClientBuilder(_serverInstance.Endpoint, _serverInstance.AccountKey))
            .WithLimitToEndpoint(true)
            .WithConnectionModeGateway() // Cosmium currently only supports connection mode 'Gateway'
            .WithHttpClientFactory(() => new HttpClient(new HttpClientHandler()
            {
                // Since cosmium runs on a self-signed certificate, we need to bypass certificate validation
                ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
            }))
            .Build();
        
        // Setup a cosmos provider mock to return a cosmos client that points to our emulator
        var cosmosClientProviderMock = new Mock<ICosmosClientProvider>();
        cosmosClientProviderMock.Setup(x => x.GetClient())
            .Returns(() => _cosmosClient);

        // Create the test factory
        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Replace the real CosmosClientProvider with our emulator provider
                    services.RemoveAll<ICosmosClientProvider>();
                    services.AddSingleton<ICosmosClientProvider>(_ => cosmosClientProviderMock.Object);
                });
            });

        _client = _factory.CreateClient();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _cosmosClient?.Dispose();
        _serverInstance?.Dispose();
        _factory?.Dispose();
        _client?.Dispose();
    }

    [SetUp]
    public virtual async Task SetUp()
    {
        // Clean the database before each test
        var container = _cosmosClient.GetDatabase("TodoDb").GetContainer("TodoItems");

        var query = container.GetItemQueryIterator<TodoItem>("SELECT * FROM c");
        var items = new List<TodoItem>();
        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            items.AddRange(response);
        }

        foreach (var item in items)
        {
            await container.DeleteItemAsync<TodoItem>(item.id, new PartitionKey(item.id));
        }
    }

    [Test]
    public async Task CreateTodo_ValidItem_ReturnsCreatedItem()
    {
        // Arrange
        var newItem = new TodoItem
        {
            Title = "Test Todo",
            Description = "Test Description",
            IsCompleted = false
        };

        // Act
        var response = await _client.PostAsJsonAsync("/todos", newItem);
        var created = await response.Content.ReadFromJsonAsync<TodoItem>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        created.Should().NotBeNull();
        created.id.Should().NotBeNullOrEmpty();
        created.Title.Should().Be(newItem.Title);
        created.Description.Should().Be(newItem.Description);
        created.IsCompleted.Should().BeFalse();
        created.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Test]
    public async Task GetTodo_ExistingItem_ReturnsItem()
    {
        // Arrange
        var newItem = new TodoItem
        {
            Title = "Test Todo",
            Description = "Test Description",
            IsCompleted = false
        };
        var createResponse = await _client.PostAsJsonAsync("/todos", newItem);
        var created = await createResponse.Content.ReadFromJsonAsync<TodoItem>();

        // Act
        var response = await _client.GetAsync($"/todos/{created.id}");
        var retrieved = await response.Content.ReadFromJsonAsync<TodoItem>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        retrieved.Should().NotBeNull();
        retrieved.Should().BeEquivalentTo(created);
    }

    [Test]
    public async Task UpdateTodo_ExistingItem_UpdatesAndReturnsItem()
    {
        // Arrange
        var newItem = new TodoItem
        {
            Title = "Test Todo",
            Description = "Test Description",
            IsCompleted = false
        };
        var createResponse = await _client.PostAsJsonAsync("/todos", newItem);
        var created = await createResponse.Content.ReadFromJsonAsync<TodoItem>();

        // Update the item
        created.IsCompleted = true;
        created.CompletedAt = DateTime.UtcNow;

        // Act
        var response = await _client.PutAsJsonAsync($"/todos/{created.id}", created);
        var updated = await response.Content.ReadFromJsonAsync<TodoItem>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        updated.Should().NotBeNull();
        updated.IsCompleted.Should().BeTrue();
    }

    [Test]
    public async Task DeleteTodo_ExistingItem_RemovesItem()
    {
        // Arrange
        var newItem = new TodoItem
        {
            Title = "Test Todo",
            Description = "Test Description",
            IsCompleted = false
        };
        var createResponse = await _client.PostAsJsonAsync("/todos", newItem);
        var created = await createResponse.Content.ReadFromJsonAsync<TodoItem>();

        // Act
        var deleteResponse = await _client.DeleteAsync($"/todos/{created.id}");
        var getResponse = await _client.GetAsync($"/todos/{created.id}");

        // Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task GetAllTodos_MultipleItems_ReturnsAllItems()
    {
        // Arrange
        var items = new[]
        {
            new TodoItem { Title = "Todo 1", Description = "Description 1" },
            new TodoItem { Title = "Todo 2", Description = "Description 2" },
            new TodoItem { Title = "Todo 3", Description = "Description 3" }
        };

        foreach (var item in items)
        {
            await _client.PostAsJsonAsync("/todos", item);
        }

        // Act
        var response = await _client.GetAsync("/todos");
        var retrieved = await response.Content.ReadFromJsonAsync<IEnumerable<TodoItem>>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        retrieved.Should().NotBeNull();
        retrieved.Should().HaveCount(3);
    }
}