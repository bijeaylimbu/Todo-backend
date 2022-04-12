using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using TodoApi.BuildingBlocks.Core;
using TodoApi.Domain.Models;
namespace TodoApi.Infrastructure.Persistence;
public class TodoDbContext : DbContext, IUnitOfWork
{
    private readonly ILogger _logger;
    public TodoDbContext(DbContextOptions<TodoDbContext> options) 
        : base(options)
    {
     _logger.LogInformation("TodoDbcontext");
    }
    public Task<OneOf<Success, Error<string>, Exception>> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
    public  DbSet<Todo> Todos { get; set; }

    public async Task<OneOf<Success, Error<string>, Exception>> SaveEntit(CancellationToken cancellationToken = default)
    {
        try
        {
            await base.SaveChangesAsync(cancellationToken);
            return new Success();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Error saving entity. {message}", ex.Message);
            return new Error<string>(ex.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error saving entites. {message}", e.Message);
            return e;
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseNpgsql()
            .UseSnakeCaseNamingConvention();
    }

    public override int SaveChanges()
    {
        return base.SaveChanges();
    }
}