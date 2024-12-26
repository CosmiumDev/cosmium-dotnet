using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TodoApp.Contracts;
using TodoApp.Repositories;
using TodoApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSingleton<ICosmosClientProvider>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("CosmosDb") 
                           ?? throw new InvalidOperationException("CosmosDB connection string is required");
    return new CosmosClientProvider(connectionString);
});

builder.Services.AddScoped<ITodoRepository, TodoRepository>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var cosmosProvider = scope.ServiceProvider.GetRequiredService<ICosmosClientProvider>();
    var client = cosmosProvider.GetClient();
    
    var database = await client.CreateDatabaseIfNotExistsAsync("TodoDb");
    
    var containerProperties = new ContainerProperties("TodoItems", "/id");
    await database.Database.CreateContainerIfNotExistsAsync(containerProperties);
}

await app.RunAsync();

// Make the Program class public for testing
public partial class Program { }
