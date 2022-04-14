using System.ComponentModel.DataAnnotations;

namespace TodoApi.Domain.Models;

public class Todo
{
    public Todo( string description, bool isCompleted)
    {
        Description = description;
        IsCompleted = isCompleted;
    }
    [Key]
    public int Id { get; set; }
    public string Description { get; private set; }
    public bool IsCompleted { get; private set; }

    public void Update(string description, bool isCompleted)
    {
        Description = description;
        IsCompleted = isCompleted;
    }
}