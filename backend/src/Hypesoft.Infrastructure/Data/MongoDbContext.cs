namespace Hypesoft.Infrastructure.Data;

using MongoDB.Driver;
using Hypesoft.Domain.Entities;
using Microsoft.Extensions.Options;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        _database = client.GetDatabase(settings.Value.DatabaseName);
    }

    public IMongoCollection<Product> Products => 
        _database.GetCollection<Product>("products");

    public IMongoCollection<Category> Categories => 
        _database.GetCollection<Category>("categories");

    public async Task CreateIndexesAsync()
    {
        
        var productIndexKeys = Builders<Product>.IndexKeys
            .Ascending(p => p.Name)
            .Ascending(p => p.CategoryId);
        
        await Products.Indexes.CreateOneAsync(
            new CreateIndexModel<Product>(productIndexKeys));

        
        var categoryIndexKeys = Builders<Category>.IndexKeys
            .Ascending(c => c.Name);
        
        await Categories.Indexes.CreateOneAsync(
            new CreateIndexModel<Category>(categoryIndexKeys));
    }
}