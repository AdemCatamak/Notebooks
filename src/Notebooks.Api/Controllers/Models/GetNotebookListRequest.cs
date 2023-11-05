namespace Notebooks.Api.Controllers.Models;

public record GetNotebookListRequest
{
    public string? Title { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}