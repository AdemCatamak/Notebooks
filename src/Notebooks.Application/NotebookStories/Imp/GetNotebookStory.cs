using Notebooks.Domain.Db;
using Notebooks.Domain.Services.BinaryStorageService;

namespace Notebooks.Application.NotebookStories.Imp;

public class GetNotebookStory : IGetNotebookStory
{
    private readonly IRepositoryContext _repositoryContext;
    private readonly IBinaryStorageService _binaryStorageService;

    public GetNotebookStory(IRepositoryContext repositoryContext, IBinaryStorageService binaryStorageService)
    {
        _repositoryContext = repositoryContext;
        _binaryStorageService = binaryStorageService;
    }

    public async Task<GetNotebookCommandResult> ExecuteAsync(GetNotebookCommand command, CancellationToken cancellationToken = default)
    {
        var notebookRepository = _repositoryContext.GetNotebookRepository();
        var notebook = await notebookRepository.GetAsync(command.Id, cancellationToken);
        
        var content = await _binaryStorageService.ReadContentAsync(notebook.Path, cancellationToken);
        
        GetNotebookCommandResult result = new(notebook.Id, notebook.Title, content, notebook.CreatedOn, notebook.UpdatedOn);
        return result;
    }
}