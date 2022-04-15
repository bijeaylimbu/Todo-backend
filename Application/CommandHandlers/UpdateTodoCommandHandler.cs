using MediatR;
using OneOf.Types;
using TodoApi.Application.Commands;
using TodoApi.Application.Responses;
using TodoApi.BuildingBlocks.Core;
using TodoApi.Domain.Interfaces;
using TodoApi.Domain.Models;
using TodoApi.Infrastructure.Repositories;

namespace TodoApi.Application.CommandHandlers;
using Outcome=OneOf.OneOf<UpdateTodoResponse, NotFound, ErrorResult>;

public class UpdateTodoCommandHandler: IRequestHandler<UpdateTodoCommand, Outcome>
{
    public readonly ITodoRepository _todoRepository;

    public UpdateTodoCommandHandler(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository ?? throw new ArgumentNullException(nameof(todoRepository));
    }

    public async Task<Outcome> Handle(UpdateTodoCommand command, CancellationToken cancellationToken)
    {
        var todo = await _todoRepository.GetTodoByIdAsync(command.Id, cancellationToken);
        if (!todo.TryPickT0(out _, out _))
            return todo.AsT1;
        Map(command, todo.AsT0);
        return await PersistTodoUpdate(todo.AsT0, command.CorrelationId, cancellationToken);
    }

    private static void Map(UpdateTodoCommand command, Todo todo)
    {
        todo.Update(command.Description, command.IsCompleted);
    }

    public async Task<Outcome> PersistTodoUpdate(Todo todo, string CorrelationId, CancellationToken cancellationToken)
    {
        var dbTodo = _todoRepository.Update(todo);
        var result = await _todoRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        if (result.TryPickT1(out var error, out _))
            return (Outcome) ErrorOutcome.createFailureResult(CorrelationId, ErrorType.InternalError,
                new[] {error.Value});
        if (result.TryPickT2(out var exception, out _))
            return (Outcome) ErrorOutcome.createFailureResult(CorrelationId, ErrorType.InternalError,
                new[] {exception.Message});
        return new UpdateTodoResponse(dbTodo.Id);
    }
}