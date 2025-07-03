using MongoDB.Bson.Serialization;

namespace SnapToTable.Infrastructure.IdGenerators;

public class GuidV7Generator : IIdGenerator
{
    public static GuidV7Generator Instance { get; } = new();
    
    public bool IsEmpty(object id)
    {
        return (Guid)id == Guid.Empty;
    }
    
    public object GenerateId(object container, object document)
    {
        return Guid.CreateVersion7();
    }
}