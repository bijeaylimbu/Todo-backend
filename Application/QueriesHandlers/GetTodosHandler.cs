using MediatR;
using TodoApi.Application.Queries;
using TodoApi.BuildingBlocks.Core;
using TodoApi.Domain.Interfaces;
using TodoApi.Domain.Models;

namespace TodoApi.Application.QueriesHandlers;
using Outcome=OneOf.OneOf<IReadOnlyCollection<Todo>, ErrorResult>;
public class GetTodosHandler : IRequestHandler<GetTodoQuery, Outcome>
{
    private readonly ITodoRepository _todoRepository;

    public GetTodosHandler(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository ?? throw new ArgumentNullException(nameof(todoRepository));
    }

    public async Task<Outcome> Handle(GetTodoQuery query, CancellationToken cancellationToken)
    {
        return await GetAllTodos(query.CorrelationId, cancellationToken);
    }
    public async Task<Outcome> GetAllTodos(string correlationId, CancellationToken cancellationToken)
    {
        var result = await _todoRepository.GetTodoAsync(cancellationToken);
        if (!result.TryPickT0(out _, out var errorResult))
            return ErrorOutcome.createFailureResult(correlationId, ErrorType.InternalError, new[] {errorResult.Value});
        return new List<Todo>(result.AsT0);
    }
}