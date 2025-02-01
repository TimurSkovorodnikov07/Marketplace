using Microsoft.Extensions.Options;
using MongoDB.Driver;
using server_app.Application.Options;

namespace server_app.Infrastructure;

public class MongoDb : IMongoDb
{
    public MongoDb(IOptions<MongoDbOptions> options)
    {
        var optionsValue = options.Value;

        var client = new MongoDB.Driver.MongoClient(optionsValue.ConnectionString);
        Database = client.GetDatabase(optionsValue.DatabaseName);
    }

    public IMongoDatabase Database { get; init; }
}