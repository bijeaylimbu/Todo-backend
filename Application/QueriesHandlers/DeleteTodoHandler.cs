using MediatR;
using OneOf.Types;
using TodoApi.Application.Queries;
using TodoApi.Application.Responses;
using TodoApi.Domain.Interfaces;
using OneOf;
using TodoApi.Application.Constants;
using TodoApi.BuildingBlocks.Core;
using Outcome=OneOf.OneOf<TodoApi.Application.Responses.DeleteTodoResponse, OneOf.Types.NotFound>;

namespace TodoApi.Application.QueriesHandlers;

public class DeleteTodoHandler : IRequestHandler<DeleteTodoQuery, Outcome>
{
    private readonly ITodoRepository _todoRepository;

    public DeleteTodoHandler(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository ?? throw new ArgumentNullException(nameof(todoRepository));
    }

    public async Task<Outcome> Handle(DeleteTodoQuery query, CancellationToken cancellationToken)
    {
        var todo = _todoRepository.DeleteAsync(query.Id, cancellationToken);
        var result = await _todoRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        if (result.TryPickT1(out _, out _))
            return (Outcome) new NotFound();
        return new DeleteTodoResponse(todo.Result);
    }
}