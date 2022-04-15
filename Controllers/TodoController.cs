using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Application.Commands;
using TodoApi.Application.Constants;
using TodoApi.Application.Requests;
using TodoApi.Application.Responses;
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
    [ProducesResponseType(typeof(CreateTodoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status503ServiceUnavailable)]
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
    [ProducesResponseType(typeof(CreateTodoResponse), StatusCodes.Status202Accepted)]
    [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status503ServiceUnavailable)]
[HttpPut("{id}")]
    public async Task<IActionResult> UpdateTodo([FromBody] UpdateTodoRequest request, int id,
        CancellationToken cancellationToken)
    {
        var command =
            new UpdateTodoCommand(id, HttpContext.CorrelationHeader(), request.Description, request.IsCompleted);
        var outcome = await _mediator.Send(command, cancellationToken);
        return outcome.Match(
            success => StatusCode((int) HttpStatusCode.Accepted, success),
            notFound => StatusCode((int) HttpStatusCode.NotFound,
                ErrorOutcome.createFailureResult(HttpContext.CorrelationHeader(), ErrorType.HardDecline,
                    new[] {ErrorReason.todoNotFound})),
            error=> StatusCode((int) HttpStatusCode.ServiceUnavailable, error)
        );
    }
}