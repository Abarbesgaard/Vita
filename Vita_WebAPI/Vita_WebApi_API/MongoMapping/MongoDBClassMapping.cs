using MongoDB.Bson.Serialization;
using Vita_WebAPI_Services;
using Vita_WebApi_Shared;

namespace Vita_WebApi_API.MongoMapping;

public static class MongoDbClassMapping
{
    public static void RegisterClassMaps()
    {
        if (!BsonClassMap.IsClassMapRegistered(typeof(Video)))
        {
            BsonClassMap.RegisterClassMap<Video>(cm =>
            {
                cm.AutoMap();  // Auto-maps all public properties.
                cm.SetIgnoreExtraElements(true);  // Ignores any extra fields not mapped in the class.
            });
        }
        if (!BsonClassMap.IsClassMapRegistered(typeof(AuditLog)))
        {
            BsonClassMap.RegisterClassMap<AuditLog>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });
        } 
    }
}
