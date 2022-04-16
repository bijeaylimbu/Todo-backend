using MediatR;
using TodoApi.Application.Queries;
using TodoApi.BuildingBlocks.Core;
using TodoApi.Domain.Interfaces;
using OneOf.Types;
using TodoApi.Domain.Models;
using Outcome= OneOf.OneOf<System.Collections.Generic.IReadOnlyCollection<TodoApi.Domain.Models.Todo>, TodoApi.BuildingBlocks.Core.ErrorResult>;


namespace TodoApi.Application.QueriesHandlers;

public class SearchTodoHandler: IRequestHandler<SearchTodoQuery, Outcome>
{
    public readonly ITodoRepository _todoRepository;

    public SearchTodoHandler(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository ?? throw new Exception(nameof(todoRepository));
    }

    public async Task<Outcome> Handle(SearchTodoQuery query, CancellationToken cancellationToken)
    {
        return await GetSearchTodo(query.query, query.correlationId, cancellationToken);
    }

    public async Task<Outcome> GetSearchTodo(string query, string correlationId, CancellationToken cancellationToken)
    {
        var result = await _todoRepository.SearchTodoAsync(query, cancellationToken);
        if (!result.TryPickT0(out _, out var errorResult))
            return ErrorOutcome.createFailureResult(correlationId, ErrorType.InternalError, new[] {errorResult.Value});
        return new List<Todo>(result.AsT0);
    }
}