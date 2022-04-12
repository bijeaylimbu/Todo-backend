using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TodoApi.Infrastructure.Persistence.EntityConfiguration;

public class TodoDbContextDesignFactory: IDesignTimeDbContextFactory<TodoDbContext>
{
    public TodoDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TodoDbContext>();
        optionsBuilder.UseNpgsql();
        return new TodoDbContext(optionsBuilder.Options);
    }
}