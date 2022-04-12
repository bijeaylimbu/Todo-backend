namespace TodoApi.Domain.Models;

public class Todo
{
    public Todo( string description, bool isCompleted)
    {
        Description = description;
        IsCompleted = isCompleted;
    }
    public int Id { get; }
    public string Description { get; private set; }
    public bool IsCompleted { get; private set; }

    public void Update(string description, bool isCompleted)
    {
        Description = description;
        IsCompleted = isCompleted;
    }
}