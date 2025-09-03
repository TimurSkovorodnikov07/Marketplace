using MongoDB.Driver;

namespace server_app.Infrastructure;

public class IMongoDb
{
    public IMongoDatabase Database { get; init; }   
}