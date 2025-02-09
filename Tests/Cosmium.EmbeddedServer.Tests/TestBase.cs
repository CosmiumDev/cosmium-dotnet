using Cosmium.EmbeddedServer.Clients;
using Cosmium.EmbeddedServer.Contracts;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;

namespace Cosmium.EmbeddedServer.Tests;

public class TestBase
{
    protected ServerInstance serverInstance;
    protected CosmosClient cosmosClient;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var serverName = Guid.NewGuid().ToString();
        var randomPort = new Random().Next(10000, 20000);
        var serverConfiguration = new ServerConfiguration
        {
            Port = randomPort,
        };
        
        serverInstance = new ServerInstance(serverName, serverConfiguration);
        
        cosmosClient = (new CosmosClientBuilder(serverInstance.Endpoint, serverInstance.AccountKey))
            .WithLimitToEndpoint(true)
            .WithConnectionModeGateway()
            .WithHttpClientFactory(() => new HttpClient(new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
            }))
            .Build();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        cosmosClient.Dispose();
        serverInstance.Dispose();
    }
}
