namespace Notebooks.Application.NotebookStories;

public interface IGetNotebookStory
{
    Task<GetNotebookCommandResult> ExecuteAsync(GetNotebookCommand command, CancellationToken cancellationToken = default);
}

public record GetNotebookCommand(Guid Id);

public record GetNotebookCommandResult(Guid Id, string Title, string Content, DateTime CreatedOn, DateTime UpdatedOn);