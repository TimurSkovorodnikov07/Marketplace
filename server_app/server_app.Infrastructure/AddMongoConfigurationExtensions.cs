using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using server_app.Domain.Entities.ProductCategories.Images;

namespace server_app.Infrastructure;

public static class AddMongoConfigurationExtensions
{
    public static void AddMongoConfiguration()
    {
        BsonClassMap.RegisterClassMap<ImageEntity>(cm =>
        {
            cm.AutoMap();
            
            cm.MapIdMember(c => c.Id).SetElementName("id")
                .SetIdGenerator(CombGuidGenerator.Instance);
            
            cm.MapMember(x => x.ProductCategoryId).SetElementName("product_category_id");
            cm.MapMember(x => x.MimeType).SetElementName("mime_type");
            cm.MapMember(x => x.ImageData).SetElementName("image_data");
        });
    }
}