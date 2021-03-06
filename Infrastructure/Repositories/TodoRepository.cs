using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using TodoApi.BuildingBlocks.Core;
using TodoApi.Domain.Interfaces;
using TodoApi.Domain.Models;
using TodoApi.Infrastructure.Persistence;
namespace TodoApi.Infrastructure.Repositories;

public class TodoRepository : ITodoRepository
{
    private readonly TodoDbContext _context;

    public TodoRepository(TodoDbContext dbContext)
    {
        _context = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public IUnitOfWork UnitOfWork => _context;
    public async Task<OneOf<Todo, NotFound, Error<string>>> GetTodoByIdAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var todo = await _context.Todos
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id,cancellationToken);
            return  todo is null ? new NotFound(): todo;
        }
        catch (Exception e)
        {
            return new Error<string>(e.Message);
        }
    }

    public async Task<OneOf<IEnumerable<Todo>, Error<string>>> GetTodoAsync(CancellationToken cancellationToken)
    {
        try
        {
            var todos =  await _context.Todos
                .OrderByDescending(x=>x.Id)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
            return todos;
        }
        catch (Exception e)
        {
            return new Error<string>(e.Message);
        }
    }

    public Todo Add(Todo todo)
    {
        return todo.Id == default
            ? _context.Todos.Add(todo).Entity
            : todo;
    }

    public Todo Update(Todo todo)
    {
        return _context.Todos.Update(todo).Entity;
    }

    public async Task<int> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var todo = await _context.Todos.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (todo is null)
            return  default;
        _context.Todos.Attach(todo);
        _context.Todos.Remove(todo);
        return todo.Id;
    }

    public async Task<OneOf<IEnumerable<Todo>, Error<string>>> SearchTodoAsync(string query, CancellationToken cancellationToken)
    {
        try
        {
            var todo = await _context.Todos
                .Where(x=>EF.Functions.ILike(x.Description,$"%{query}%"))
                .ToListAsync(cancellationToken);
            return todo;
        }
        catch (Exception e)
        {
            return new Error<string>(e.Message);
        }
    }
}