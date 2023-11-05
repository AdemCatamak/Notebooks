namespace Notebooks.Application.NotebookStories;

public interface IGetNotebookListStory
{
    Task<GetNotebookListCommandResult> ExecuteAsync(GetNotebookListCommand command, CancellationToken cancellationToken = default);
}

public record GetNotebookListCommand(string? Title, int PageNumber, int PageSize);

public record GetNotebookListCommandResult(List<GetNotebookListCommandResultItem> Items, int TotalCount);

public record GetNotebookListCommandResultItem(Guid Id, string Title, DateTime CreatedOn, DateTime UpdatedOn);