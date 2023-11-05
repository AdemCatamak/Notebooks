using Testcontainers.Azurite;
using Testcontainers.MsSql;

namespace Notebooks.IntegrationTest;

internal class NotebooksInfra : IDisposable
{
    private readonly AzuriteContainer? _azuriteMock;
    private readonly MsSqlContainer? _msSqlContainer;

    public NotebooksInfra()
    {
        _azuriteMock = new AzuriteBuilder()
            .WithImage("mcr.microsoft.com/azure-storage/azurite:3.27.0")
            .WithPortBinding(10000, 10000)
            .Build();
        _azuriteMock.StartAsync().GetAwaiter().GetResult();

        _msSqlContainer = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2019-latest")
            .WithPassword("Passw0rd")
            .WithPortBinding(1433, 1433)
            .Build();
        _msSqlContainer.StartAsync().GetAwaiter().GetResult();
    }


    public void Dispose()
    {
        _azuriteMock?.DisposeAsync().GetAwaiter().GetResult();
        _msSqlContainer?.DisposeAsync().GetAwaiter().GetResult();
    }
}