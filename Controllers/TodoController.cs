using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Application.Commands;
using TodoApi.Application.Requests;
using TodoApi.BuildingBlocks.Core;
using TodoApi.BuildingBlocks.Logging;

namespace TodoApi.Controllers;
[ApiController]
[Route("todo")]
[Produces("application/json")]
public class TodoController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public TodoController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }
    [HttpPost]
    public async Task<IActionResult> CreateTodo([FromBody] CreateTodoRequest request)
    {
        var command = new CreateTodoCommand(   HttpContext.CorrelationHeader(), request.Description, request.IsCompleted);
        var outcome = await _mediator.Send(command);
        return outcome.Match( success=> StatusCode((int) HttpStatusCode.Created, success),
            error=> error.ErrorType.Equals(ErrorType.InternalError)
            ? StatusCode((int) HttpStatusCode.ServiceUnavailable, outcome.Value)
            : StatusCode((int) HttpStatusCode.Forbidden, outcome.Value)
            );
    }
}