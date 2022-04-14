using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using TodoApi.BuildingBlocks.Core;
using TodoApi.Domain.Models;
using TodoApi.Infrastructure.Persistence.EntityConfiguration;

namespace TodoApi.Infrastructure.Persistence;
using Serilog;
using ILogger=Serilog.ILogger;
public class TodoDbContext : DbContext, IUnitOfWork
{
    private readonly ILogger _logger;
    public TodoDbContext(DbContextOptions<TodoDbContext> options) 
        : base(options)
    {
     _logger= Log.ForContext<TodoDbContext>();
    }
    public async Task<OneOf<Success, Error<string>, Exception>> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await base.SaveChangesAsync(cancellationToken);
            return new Success();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.Error(ex, "Error saving entity. {message}", ex.Message);
            return new Error<string>(ex.Message);
        }
        catch (Exception e)
        {
            _logger.Error(e, "Error saving entites. {message}", e.Message);
            return e;
        }
    }
    public  DbSet<Todo> Todos { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var assembly = typeof(TodoBuilder).Assembly;
        modelBuilder.ApplyConfigurationsFromAssembly(assembly);
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