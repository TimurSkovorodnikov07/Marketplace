using server_app.Domain.Entities;

namespace server_app.Application.Extensions;

public static class EntityExtensions
{
    public static List<Guid> GetIdentifiers<EntityT>(this List<EntityT> entities)
        where EntityT : Entity
    {
        var identifiers = new List<Guid>();

        foreach (var entity in entities)
            identifiers.Add(entity.Id);

        return identifiers;
    }
}