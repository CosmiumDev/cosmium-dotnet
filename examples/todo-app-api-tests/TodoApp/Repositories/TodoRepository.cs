using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using TodoApp.Contracts;
using TodoApp.Models;

namespace TodoApp.Repositories;

public class TodoRepository : ITodoRepository
{
    private readonly ICosmosClientProvider _clientProvider;
    private readonly string _databaseId = "TodoDb";
    private readonly string _containerId = "TodoItems";

    public TodoRepository(ICosmosClientProvider clientProvider)
    {
        _clientProvider = clientProvider;
    }

    private Container GetContainer()
    {
        var client = _clientProvider.GetClient();
        var database = client.GetDatabase(_databaseId);
        return database.GetContainer(_containerId);
    }

    public async Task<TodoItem> CreateAsync(TodoItem item)
    {
        item.id = Guid.NewGuid().ToString();
        item.CreatedAt = DateTime.UtcNow;
        
        var container = GetContainer();
        var response = await container.CreateItemAsync(item);
        return response.Resource;
    }

    public async Task<TodoItem> GetByIdAsync(string id)
    {
        var container = GetContainer();
        try
        {
            var response = await container.ReadItemAsync<TodoItem>(id, new PartitionKey(id));
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<IEnumerable<TodoItem>> GetAllAsync()
    {
        var container = GetContainer();
        var query = container.GetItemQueryIterator<TodoItem>(new QueryDefinition("SELECT * FROM c"));
        
        var results = new List<TodoItem>();
        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            results.AddRange(response);
        }
        
        return results;
    }

    public async Task<TodoItem> UpdateAsync(TodoItem item)
    {
        var container = GetContainer();
        var response = await container.UpsertItemAsync(item, new PartitionKey(item.id));
        return response.Resource;
    }

    public async Task DeleteAsync(string id)
    {
        var container = GetContainer();
        await container.DeleteItemAsync<TodoItem>(id, new PartitionKey(id));
    }
}