using Notebooks.Domain.Db.Repositories;

namespace Notebooks.Domain.Db;

public interface IRepositoryContext
{
    INotebookRepository GetNotebookRepository();
    Task CommitAsync(CancellationToken cancellationToken);
}