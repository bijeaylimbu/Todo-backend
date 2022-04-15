using MediatR;
using TodoApi.Application.Responses;
using TodoApi.BuildingBlocks.Core;
using OneOf;
namespace TodoApi.Application.Commands;

public record CreateTodoCommand(string CorrelationId, string Description, bool IsCompleted) : IRequest<OneOf<CreateTodoResponse, ErrorResult>>;