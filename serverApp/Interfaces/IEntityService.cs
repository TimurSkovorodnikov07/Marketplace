public interface IEntityService<EntityT, CreatedType, UpdatedEntityT,
    AddResultT, UpdateResultT, RemoveResultT> where EntityT : Entity
{
    Task<EntityT?> Get(Guid guid);
    Task<AddResultT> Add(CreatedType entity);
    Task<UpdateResultT> Update(UpdatedEntityT entity);
    Task<RemoveResultT> Remove(Guid guid);
}

public interface IEntityService<EntityT, CreatedType, UpdatedEntityT> where EntityT : Entity
{
    Task<EntityT?> Get(Guid guid);
    Task<bool> Add(CreatedType entity);
    Task<bool> Update(UpdatedEntityT entity);
    Task<bool> Remove(Guid guid);
}

public interface IEntityService<EntityT, UpdatedEntityT> where EntityT : Entity
{
    Task<EntityT?> Get(Guid guid);
    Task Add(EntityT entity);
    Task<bool> Update(UpdatedEntityT entity);
    Task<bool> Remove(Guid guid);
}

public interface IEntityService<EntityT> where EntityT : Entity
{
    Task<EntityT?> Get(Guid guid);
    Task Add(EntityT entity);
    Task<bool> Update(Entity updatedEntity);
    Task<bool> Remove(Guid guid);
}