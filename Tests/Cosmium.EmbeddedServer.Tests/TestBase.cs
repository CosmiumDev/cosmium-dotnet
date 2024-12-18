using Cosmium.EmbeddedServer.Contracts;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;

namespace Cosmium.EmbeddedServer.Tests;

public class TestBase
{
    private string _serverName;
    private ServerConfiguration _serverConfiguration;
    private string CosmiumEndpoint => $"https://{_serverConfiguration.Host}:{_serverConfiguration.Port}/";
    private string CosmiumAccountKey => _serverConfiguration.AccountKey;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _serverName = Guid.NewGuid().ToString();
        var randomPort = new Random().Next(10000, 20000);

        _serverConfiguration = new ServerConfiguration
        {
            Port = randomPort,
        };

        var createResult = CosmiumServer.CreateInstance(_serverName, _serverConfiguration);

        Assert.That(createResult, Is.EqualTo(0));
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        var stopResult = CosmiumServer.StopInstance(_serverName);

        Assert.That(stopResult, Is.EqualTo(0));
    }

    protected CosmosClient GetCosmosClient()
    {
        return (new CosmosClientBuilder(CosmiumEndpoint, CosmiumAccountKey))
            .WithLimitToEndpoint(true)
            .WithConnectionModeGateway()
            .WithHttpClientFactory(() => new HttpClient(new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
            }))
            .Build();
    }

    protected ServerState? GetServerState()
    {
        return CosmiumServer.GetInstanceState(_serverName);
    }
}
