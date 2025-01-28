using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using server_app.Domain.Validations;

namespace server_app.Domain.Entities.ProductCategories.Images;

public class ImageMongoEntity
{
    [BsonId] [BsonElement("_id")] public ObjectId Id { get; set; }

    [BsonElement("guid_id")]
    [BsonRepresentation(BsonType.String)]
    public Guid GuidId { get; set; }

    [BsonElement("mime_type")] public string MimeType { get; set; }

    [BsonElement("image_data")] public byte[] ImageData { get; set; }

    public static ImageMongoEntity? Create(Guid guidId, string mimeType, byte[] imageBytes)
    {
        var newImage = new ImageMongoEntity()
        {
            Id = ObjectId.GenerateNewId(),
            GuidId = guidId,
            MimeType = mimeType,
            ImageData = imageBytes
        };
        return ImageMongoValidator.IsValid(newImage) ? newImage : null;
    }
}