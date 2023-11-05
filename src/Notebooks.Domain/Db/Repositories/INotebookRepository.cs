namespace Notebooks.Domain.Db.Repositories;

public interface INotebookRepository
{
    Task AddAsync(Notebook notebook, CancellationToken cancellationToken);
    Task UpdateAsync(Notebook notebook, CancellationToken cancellationToken);
    Task<(int totalCount, List<Notebook> notebooks)> GetListAsync(string? title, int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<Notebook> GetAsync(Guid id, CancellationToken cancellationToken);
}