namespace TodoApi.Application.Requests;

public record UpdateTodoRequest(string Description, bool IsCompleted);