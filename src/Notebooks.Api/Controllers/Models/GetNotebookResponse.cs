using Notebooks.Application.NotebookStories;

namespace Notebooks.Api.Controllers.Models;

public record GetNotebookResponse(Guid Id, string Title, string Content, DateTime CreatedOn, DateTime UpdatedOn)
{
    public static GetNotebookResponse From(GetNotebookCommandResult commandResult)
    {
        return new GetNotebookResponse(commandResult.Id, commandResult.Title, commandResult.Content, commandResult.CreatedOn, commandResult.UpdatedOn);
    }
}