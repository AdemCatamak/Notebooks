using Microsoft.Data.SqlClient;
using Notebooks.Domain.Db;

namespace Notebooks.Api.HostedServices;

public class DbMigrationRunnerHostedService : IHostedService
{
    private readonly ILogger<DbMigrationRunnerHostedService> _logger;
    private readonly IDbMigrationEngine _dbMigrationEngine;

    public DbMigrationRunnerHostedService(ILogger<DbMigrationRunnerHostedService> logger, IDbMigrationEngine dbMigrationEngine)
    {
        _logger = logger;
        _dbMigrationEngine = dbMigrationEngine;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        TimeSpan sleepTime = TimeSpan.FromSeconds(5);
        const int maxTryCount = 5;
        var tryCount = 0;
        var success = false;
        do
        {
            tryCount++;
            try
            {
                _dbMigrationEngine.Migrate();
                success = true;
            }
            catch (Exception)
            {
                if (tryCount > maxTryCount) throw;
                await Task.Delay(sleepTime, cancellationToken);
            }
        }
        while (!success);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}