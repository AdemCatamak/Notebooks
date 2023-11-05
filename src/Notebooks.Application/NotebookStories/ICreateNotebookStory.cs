namespace Notebooks.Application.NotebookStories;

public interface ICreateNotebookStory
{
    Task<CreateNotebookCommandResult> ExecuteAsync(CreateNotebookCommand command, CancellationToken cancellationToken = default);
}

public record CreateNotebookCommand(string Title, string Content);

public record CreateNotebookCommandResult(Guid Id, string Title, string Content, DateTime CreatedOn, DateTime UpdatedOn);