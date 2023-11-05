using Notebooks.Application.NotebookStories;

namespace Notebooks.Api.Controllers.Models;

public record GetNotebookListResponse(List<GetNotebookListItemResponse> Items, int TotalCount)
{
    public static GetNotebookListResponse From(GetNotebookListCommandResult commandResult)
    {
        List<GetNotebookListItemResponse> items = commandResult.Items.Select(GetNotebookListItemResponse.From).ToList();
        return new GetNotebookListResponse(items, commandResult.TotalCount);
    }
}

public record GetNotebookListItemResponse(Guid Id, string Title, DateTime CreatedOn, DateTime UpdatedOn)
{
    public static GetNotebookListItemResponse From(GetNotebookListCommandResultItem commandResultItem)
    {
        GetNotebookListItemResponse response = new(
            commandResultItem.Id,
            commandResultItem.Title,
            commandResultItem.CreatedOn,
            commandResultItem.UpdatedOn
        );
        return response;
    }
}