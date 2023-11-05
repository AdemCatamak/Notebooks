using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Notebooks.Domain;
using Notebooks.Domain.Db.Repositories;
using Notebooks.Domain.Exceptions;

namespace Notebooks.Infra.Db.Repositories;

internal class NotebookRepository : INotebookRepository
{
    private readonly RepositoryContext _repositoryContext;

    public NotebookRepository(RepositoryContext repositoryContext)
    {
        _repositoryContext = repositoryContext;
    }

    public async Task AddAsync(Notebook notebook, CancellationToken cancellationToken)
    {
        await _repositoryContext.AddAsync(notebook, cancellationToken);
    }

    public Task UpdateAsync(Notebook notebook, CancellationToken cancellationToken)
    {
        // EF Core tracks changes to entities while they are in memory.
        return Task.CompletedTask;
    }

    public async Task<(int totalCount, List<Notebook> notebooks)> GetListAsync(string? title, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var notebooks = _repositoryContext.Set<Notebook>().AsQueryable();

        if (!string.IsNullOrEmpty(title))
        {
            notebooks = notebooks.Where(x => x.Title.Contains(title));
        }

        var totalCount = await notebooks.CountAsync(cancellationToken: cancellationToken);
        var notebookList = await notebooks.OrderBy(x => x.CreatedOn)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken: cancellationToken);

        if (!notebookList.Any())
        {
            throw new NotFoundException(nameof(Notebook));
        }

        return (totalCount, notebookList);
    }

    public async Task<Notebook> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var notebook = await _repositoryContext.Set<Notebook>()
            .FirstOrDefaultAsync(n => n.Id == id, cancellationToken: cancellationToken);

        return notebook ?? throw new NotFoundException(nameof(Notebook));
    }
}