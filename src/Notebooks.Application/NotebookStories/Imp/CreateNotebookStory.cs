using Notebooks.Domain;
using Notebooks.Domain.Db;
using Notebooks.Domain.Db.Repositories;
using Notebooks.Domain.Services.BinaryStorageService;

namespace Notebooks.Application.NotebookStories.Imp;

public class CreateNotebookStory : ICreateNotebookStory
{
    private readonly IBinaryStorageService _binaryStorageService;
    private readonly IRepositoryContext _repositoryContext;

    public CreateNotebookStory(IBinaryStorageService binaryStorageService, IRepositoryContext repositoryContext)
    {
        _binaryStorageService = binaryStorageService;
        _repositoryContext = repositoryContext;
    }


    public async Task<CreateNotebookCommandResult> ExecuteAsync(CreateNotebookCommand command, CancellationToken cancellationToken = default)
    {
        var id = Guid.NewGuid();
        var docPath = $"{id}.md";

        await _binaryStorageService.PutContentAsync(docPath, command.Content, cancellationToken);

        Notebook notebook = new(id, command.Title, docPath);
        INotebookRepository notebookRepository = _repositoryContext.GetNotebookRepository();
        await notebookRepository.AddAsync(notebook, cancellationToken);

        await _repositoryContext.CommitAsync(cancellationToken);

        var commandResult = new CreateNotebookCommandResult(notebook.Id, notebook.Title, command.Content, notebook.CreatedOn, notebook.UpdatedOn);
        return commandResult;
    }
}