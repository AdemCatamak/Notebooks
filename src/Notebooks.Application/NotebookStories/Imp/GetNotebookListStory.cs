using Notebooks.Domain.Db;
using Notebooks.Domain.Db.Repositories;

namespace Notebooks.Application.NotebookStories.Imp;

public class GetNotebookListStory : IGetNotebookListStory
{
    private readonly IRepositoryContext _repositoryContext;

    public GetNotebookListStory(IRepositoryContext repositoryContext)
    {
        _repositoryContext = repositoryContext;
    }

    public async Task<GetNotebookListCommandResult> ExecuteAsync(GetNotebookListCommand command, CancellationToken cancellationToken = default)
    {
        INotebookRepository notebookRepository = _repositoryContext.GetNotebookRepository();
        var (totalCount, notebooks) = await notebookRepository.GetListAsync(command.Title, command.PageNumber, command.PageSize, cancellationToken);

        var commandResultItems = notebooks
            .Select(x => new GetNotebookListCommandResultItem(x.Id, x.Title, x.CreatedOn, x.UpdatedOn))
            .ToList();

        var result = new GetNotebookListCommandResult(commandResultItems, totalCount);
        return result;
    }
}