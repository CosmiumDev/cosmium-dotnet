# Cosmium .NET – Effortless CosmosDB Emulation for Testing

## Introduction

`cosmium-dotnet` is a lightweight .NET library designed to make testing with CosmosDB effortless. It wraps and extends [Cosmium](https://github.com/pikami/cosmium), a CosmosDB emulator, enabling you to spin up an embedded, in-memory CosmosDB instance directly in your .NET applications and tests.

## Motivation

When working on applications that use CosmosDB, running integration or unit tests against a real database can be impractical or cumbersome. While the original Cosmium project provides a great way to emulate CosmosDB, it still requires you to start and manage the emulator process externally - whether in CI pipelines or local environments.

`cosmium-dotnet` solves this by embedding the emulator into your application with a single method call. This approach offers several key benefits:

- **Simplified Test Setup:** Start an in-memory CosmosDB emulator directly in your test code without any external processes.
- **Practical for CI:** No need to manage emulator processes during CI builds - Cosmium can run entirely in-memory.
- **Faster Local Development:** Run tests locally without relying on a real database or managing external dependencies.

By combining the powerful emulation capabilities of Cosmium with the simplicity of .NET, `cosmium-dotnet` helps developers focus on testing, not infrastructure.

## Getting Started

To start using `cosmium-dotnet`, follow these steps:

### Installation

Add the `Cosmium.EmbeddedServer` NuGet package to your project:

```bash
dotnet add package Cosmium.EmbeddedServer
```

### Quick Setup

Here’s a simple example to get you started with an in-memory CosmosDB emulator in your tests:

```csharp
using Cosmium.EmbeddedServer;

// Initialize a new server instance
var serverInstance = new ServerInstance(
    Guid.NewGuid().ToString(), // Unique server name
    new ServerConfiguration { Port = 8082 } // If you're already running an emulator insance on 8081
);

// Create and configure the Cosmos client
var cosmosClient = (new CosmosClientBuilder(serverInstance.Endpoint, serverInstance.AccountKey))
    .WithLimitToEndpoint(true)
    .WithConnectionModeGateway() // Cosmium currently only supports connection mode 'Gateway'
    .WithHttpClientFactory(() => new HttpClient(new HttpClientHandler()
    {
        // Since cosmium runs on a self-signed certificate, we need to bypass certificate validation
        ServerCertificateCustomValidationCallback =
            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
    }))
    .Build();

// Use the client in your tests or application setup

// You might also want to create some databases and collections,
// The ServerInstance object exposes some useful methods for that:
var database = serverInstance.CreateDatabase("test-db");
database.CreateCollection("coll-1");
database.CreateCollection("coll-2");
```

In the real world you might want to make a base class for your integration tests that does this setup for you, you can take a look at [Tests/Cosmium.EmbeddedServer.Tests/TestBase.cs](Tests/Cosmium.EmbeddedServer.Tests/TestBase.cs) for inspiration.

#### Custom json serialization

While inserting data into the emulator you might want to use a custom serializer, for that you can implement the interface `IDocumentSerializer` and pass it to the constructor while creating the `ServerInstance`.

### Example Project

For a complete working example of how to use `cosmium-dotnet`, check out the **Todo App Example** in the repository:

📂 [examples/TodoApp](examples/todo-app-api-tests)

This project demonstrates how to:

1. Integrate `cosmium-dotnet` into a simple TODO API project.
2. Write integration tests using the in-memory CosmosDB emulator.

# Contributing and Development

This project uses native libraries to run the CosmosDB emulator, so it requires some additional setup to build and run the tests.

## Prerequisites

- Create a `cosmium_dist` directory in the root of the repository.
- Download the Cosmium native libraries (cosmium\_<version>\_shared-libraries.tar.gz) from the [Cosmium releases page](https://github.com/pikami/cosmium/releases) and place the .dll/.so/.dylib files in the `cosmium_dist` directory.

# License

This project is [MIT licensed](./LICENSE).
