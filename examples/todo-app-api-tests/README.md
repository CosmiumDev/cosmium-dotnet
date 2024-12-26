# Todo App Example with Cosmium

This example demonstrates how to use Cosmium to emulate CosmosDB in your integration tests.
The project consists of a simple TODO API backed by CosmosDB, with a full test suite showing how to use Cosmium for database emulation.

## Project Structure

```
./
├── TodoApp/                # Main API project
│   ├── Controllers/        # API endpoints
│   ├── Models/             # Data models
│   ├── Contracts/          # Interfaces
│   └── Program.cs          # Application setup
└── TodoApp.Tests/          # Test project
    └── TodoControllerTests.cs  # Integration tests with Cosmium
```

## Features

- Full CRUD operations for TODO items
- CosmosDB backend
- Integration tests using Cosmium
- Clean architecture with repository pattern

## Getting Started

1. Clone the repository
2. Install dependencies:
   ```bash
   dotnet restore
   ```
3. Run the tests:
   ```bash
   dotnet test
   ```

## How It Works

### Database Emulation

The example uses Cosmium to emulate CosmosDB during testing. Here's how it's set up:

```csharp
// Initialize cosmium database emulator
var serverName = Guid.NewGuid().ToString();
var randomPort = new Random().Next(10000, 20000);
var serverConfiguration = new ServerConfiguration
{
    Port = randomPort,
};

_serverInstance = new ServerInstance(serverName, serverConfiguration);
```

### Test Setup

The tests use `WebApplicationFactory` to create a test host and inject the Cosmium client:

```csharp
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
```

### Key Benefits

1. **No Real Database Required**: Tests run against an in-memory emulator
2. **Fast Execution**: No need to wait for real database operations
3. **Isolated Testing**: Each test gets a clean database state
4. **Parallel Test Execution**: Support for running tests in parallel with unique database instances

## Using Cosmium in Your Own Projects

1. Install the Cosmium NuGet package:

   ```bash
   dotnet add package Cosmium.EmbeddedServer
   ```

2. Configure Cosmium in your test setup:

   ```csharp
   // Initialize server instance
   var serverInstance = new ServerInstance(
       Guid.NewGuid().ToString(),
       new ServerConfiguration { Port = RandomPort() }
   );

   // Create and configure the cosmos client
   var cosmosClient = (new CosmosClientBuilder(_serverInstance.Endpoint, _serverInstance.AccountKey))
     .WithLimitToEndpoint(true)
     .WithConnectionModeGateway() // Cosmium currently only supports connection mode 'Gateway'
     .WithHttpClientFactory(() => new HttpClient(new HttpClientHandler()
     {
         // Since cosmium runs on a self-signed certificate, we need to bypass certificate validation
         ServerCertificateCustomValidationCallback =
             HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
     }))
     .Build();
   ```

3. Replace your real CosmosDB client with the Cosmium client in your test setup

## Tips for Testing with Cosmium

- Use unique database names for parallel test execution
- Clean up database state between tests
- Handle certificate validation for the emulator
- Use connection mode 'Gateway' as it's currently the only supported mode
