using Notebooks.Application.NotebookStories;

namespace Notebooks.Api.Controllers.Models;

public record CreateNotebookResponse(Guid Id)
{
    public static CreateNotebookResponse From(CreateNotebookCommandResult commandResult)
    {
        return new CreateNotebookResponse(commandResult.Id);
    }
}