namespace TodoApi.Application.Requests;

public record CreateTodoRequest(string Description, bool IsCompleted);