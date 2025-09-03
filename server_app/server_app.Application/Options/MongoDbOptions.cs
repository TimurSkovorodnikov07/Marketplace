namespace server_app.Application.Options;

public class MongoDbOptions
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string ImagesCollectionName { get; set; }
}