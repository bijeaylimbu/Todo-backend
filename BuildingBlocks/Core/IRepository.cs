namespace TodoApi.BuildingBlocks.Core;

public interface IRepository<T> where  T: class
{ 
    IUnitOfWork UnitOfWork { get; }
}