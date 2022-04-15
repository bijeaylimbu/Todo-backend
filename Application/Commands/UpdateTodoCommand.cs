using MediatR;
using OneOf;
using OneOf.Types;
using TodoApi.Application.Responses;
using TodoApi.BuildingBlocks.Core;

namespace TodoApi.Application.Commands;

public record UpdateTodoCommand( int Id, string CorrelationId, string Description, bool IsCompleted ): IRequest<OneOf<UpdateTodoResponse, NotFound, ErrorResult>>;