using MediatR;
using OneOf;
using OneOf.Types;
using TodoApi.Application.Responses;
using Outcome=OneOf.OneOf<TodoApi.Application.Responses.DeleteTodoResponse, OneOf.Types.NotFound>;

namespace TodoApi.Application.Queries;

public record DeleteTodoQuery(int Id, string CorrelationId ): IRequest<Outcome> ;