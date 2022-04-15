using OneOf;
using OneOf.Types;
using TodoApi.BuildingBlocks.Core;
using TodoApi.Domain.Models;

namespace TodoApi.Domain.Interfaces;

public interface ITodoRepository : IRepository<Todo>
{
    Task<OneOf<Todo, NotFound, Error<string>>> GetTodoByIdAsync(int id, CancellationToken cancellationToken);
    Task<OneOf<IEnumerable<Todo>, Error<string>>> GetTodoAsync();
    Todo Add(Todo todo);
    Todo Update(Todo todo);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
}