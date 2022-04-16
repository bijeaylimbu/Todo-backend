using MediatR;
using TodoApi.BuildingBlocks.Core;
using TodoApi.Domain.Models;
using OneOf;
namespace TodoApi.Application.Queries;

public record GetTodoQuery(string CorrelationId): IRequest<OneOf<IReadOnlyCollection<Todo>, ErrorResult>>;