namespace Notebooks.Application.NotebookStories;

public interface IUpdateNotebookStory
{
    Task ExecuteAsync(UpdateNotebookCommand command, CancellationToken cancellationToken = default);
}

public record UpdateNotebookCommand(Guid Id, string? Title, string? Content);