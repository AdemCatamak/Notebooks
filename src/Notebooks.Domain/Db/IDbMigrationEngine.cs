namespace Notebooks.Domain.Db;

public interface IDbMigrationEngine
{
    void Migrate();
}