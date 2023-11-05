using Microsoft.AspNetCore.Mvc;
using Notebooks.Api.Controllers.Models;
using Notebooks.Application.NotebookStories;

namespace Notebooks.Api.Controllers;

[ApiController]
[Route("notebooks")]
public class NotebookController : ControllerBase
{
    private readonly IGetNotebookListStory _getNotebookListStory;
    private readonly IGetNotebookStory _getNotebookStory;
    private readonly ICreateNotebookStory _createNotebookStory;
    private readonly IUpdateNotebookStory _updateNotebookStory;

    public NotebookController(IGetNotebookListStory getNotebookListStory, IGetNotebookStory getNotebookStory, ICreateNotebookStory createNotebookStory, IUpdateNotebookStory updateNotebookStory)
    {
        _getNotebookListStory = getNotebookListStory;
        _getNotebookStory = getNotebookStory;
        _createNotebookStory = createNotebookStory;
        _updateNotebookStory = updateNotebookStory;
    }

    [HttpGet("")]
    public async Task<IActionResult> GetNotebookListAsync([FromQuery] GetNotebookListRequest request, CancellationToken cancellationToken)
    {
        var command = new GetNotebookListCommand(request.Title, request.PageNumber, request.PageSize);
        GetNotebookListCommandResult commandResult = await _getNotebookListStory.ExecuteAsync(command, cancellationToken);
        GetNotebookListResponse response = GetNotebookListResponse.From(commandResult);

        return StatusCode(StatusCodes.Status200OK, response);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetNotebookAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new GetNotebookCommand(id);
        GetNotebookCommandResult commandResult = await _getNotebookStory.ExecuteAsync(command, cancellationToken);
        GetNotebookResponse response = GetNotebookResponse.From(commandResult);

        return StatusCode(StatusCodes.Status200OK, response);
    }

    [HttpPost("")]
    public async Task<IActionResult> CreateNotebookAsync([FromBody] CreateNotebookRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateNotebookCommand(request.Title, request.Content);
        CreateNotebookCommandResult commandResult = await _createNotebookStory.ExecuteAsync(command, cancellationToken);
        CreateNotebookResponse response = CreateNotebookResponse.From(commandResult);

        return StatusCode(StatusCodes.Status201Created, response);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> PutNotebookAsync([FromRoute] Guid id, [FromBody] UpdateNotebookRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateNotebookCommand(id, request.Title, request.Content);
        await _updateNotebookStory.ExecuteAsync(command, cancellationToken);

        return StatusCode(StatusCodes.Status200OK);
    }
}