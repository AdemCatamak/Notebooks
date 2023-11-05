using Microsoft.EntityFrameworkCore;
using Notebooks.Domain.Db;
using Notebooks.Domain.Db.Repositories;
using Notebooks.Infra.Db.Repositories;

namespace Notebooks.Infra.Db;

public class RepositoryContext : DbContext, IRepositoryContext
{
    public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }

    public INotebookRepository GetNotebookRepository()
    {
        NotebookRepository repository = new(this);
        return repository;
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        await SaveChangesAsync(cancellationToken);
    }
}