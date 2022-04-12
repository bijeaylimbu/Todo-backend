
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoApi.Domain.Models;

namespace TodoApi.Infrastructure.Persistence.EntityConfiguration;

internal sealed class TodoBuilder: IEntityTypeConfiguration<Todo>
{
    public void Configure(EntityTypeBuilder<Todo> builder)
    {
        builder.ToTable("todo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Description)
            .HasColumnName("description");
        builder.Property(x => x.IsCompleted)
            .HasColumnName("is_completed");
    }
    
}