using MediatR;
using TodoApi.BuildingBlocks.Core;
using TodoApi.Domain.Models;
using OneOf;
namespace TodoApi.Application.Queries;

public record SearchTodoQuery( string query, string correlationId): IRequest<OneOf<IReadOnlyCollection<Todo>, ErrorResult>>;