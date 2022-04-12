using OneOf;
using OneOf.Types;

namespace TodoApi.BuildingBlocks.Core;

public interface IUnitOfWork : IDisposable
{
    Task<OneOf<Success, Error<string>, Exception>> SaveEntitiesAsync(CancellationToken cancellationToken = default);
}