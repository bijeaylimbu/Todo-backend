using MediatR;
using TodoApi.Application.Commands;
using TodoApi.Application.Responses;
using TodoApi.BuildingBlocks.Core;
using TodoApi.Domain.Interfaces;
using TodoApi.Domain.Models;

namespace TodoApi.Application.CommandHandlers;
using Outcome=OneOf.OneOf<CreateTodoResponse, ErrorResult>;
public class CreateTodoCommandHandler : IRequestHandler<CreateTodoCommand, Outcome>
{
    private readonly ITodoRepository _todoRepository;

    public CreateTodoCommandHandler(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository ?? throw new ArgumentNullException(nameof(todoRepository));
    }

    public async Task<Outcome> Handle(CreateTodoCommand command, CancellationToken cancellationToken)
    {
        return await PersistTodo(CreateTodo(command), command.CorrelationId, cancellationToken);
    }

    public Todo CreateTodo(CreateTodoCommand command)
    {
        return new Todo(command.Description, command.IsCompleted);
    }

    public async Task<Outcome> PersistTodo(Todo todo, string correlationId, CancellationToken cancellationToken)
    {
        var dbTodo = _todoRepository.Add(todo);
        var result = await _todoRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        return result.Match(
            success => (Outcome) new CreateTodoResponse(dbTodo.Id),
            error => (Outcome) ErrorOutcome.createFailureResult(correlationId, ErrorType.InternalError,
                new[] {error.Value}),
            exception =>
                (Outcome) ErrorOutcome.createFailureResult(correlationId, ErrorType.InternalError,
                    new[] {exception.Message})
        );
    }
}