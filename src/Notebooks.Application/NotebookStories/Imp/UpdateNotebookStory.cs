using Notebooks.Domain;
using Notebooks.Domain.Db;
using Notebooks.Domain.Db.Repositories;
using Notebooks.Domain.Services.BinaryStorageService;

namespace Notebooks.Application.NotebookStories.Imp;

public class UpdateNotebookStory : IUpdateNotebookStory
{
    private readonly IRepositoryContext _repositoryContext;
    private readonly IBinaryStorageService _binaryStorageService;

    public UpdateNotebookStory(IRepositoryContext repositoryContext, IBinaryStorageService binaryStorageService)
    {
        _repositoryContext = repositoryContext;
        _binaryStorageService = binaryStorageService;
    }

    public async Task ExecuteAsync(UpdateNotebookCommand command, CancellationToken cancellationToken = default)
    {
        INotebookRepository notebookRepository = _repositoryContext.GetNotebookRepository();
        Notebook notebook = await notebookRepository.GetAsync(command.Id, cancellationToken);

        if (command.Content is not null)
        {
            await _binaryStorageService.PutContentAsync(notebook.Path, command.Content, cancellationToken);
            notebook.ContentChanged();
            await notebookRepository.UpdateAsync(notebook, cancellationToken);
        }
        
        if (command.Title is not null)
        {
            notebook.ChangeTitle(command.Title);
            await notebookRepository.UpdateAsync(notebook, cancellationToken);
        }
        
        await _repositoryContext.CommitAsync(cancellationToken);
    }
}