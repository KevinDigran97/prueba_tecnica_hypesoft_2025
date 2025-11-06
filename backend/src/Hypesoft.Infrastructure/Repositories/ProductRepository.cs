namespace Hypesoft.Infrastructure.Repositories;

using MongoDB.Driver;
using Hypesoft.Domain.Entities;
using Hypesoft.Domain.Repositories;
using Hypesoft.Infrastructure.Data;

public class ProductRepository : IProductRepository
{
    private readonly MongoDbContext _context;

    public ProductRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .Find(p => p.Id == id && !p.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .Find(p => !p.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<(List<Product> Items, int TotalCount)> GetPagedAsync(
        int page, 
        int pageSize, 
        string? search = null, 
        string? categoryId = null, 
        CancellationToken cancellationToken = default)
    {
        var filterBuilder = Builders<Product>.Filter;
        var filter = filterBuilder.Eq(p => p.IsDeleted, false);

        
        if (!string.IsNullOrWhiteSpace(search))
        {
            filter &= filterBuilder.Regex(p => p.Name, 
                new MongoDB.Bson.BsonRegularExpression(search, "i"));
        }

        
        if (!string.IsNullOrWhiteSpace(categoryId))
        {
            filter &= filterBuilder.Eq(p => p.CategoryId, categoryId);
        }

        var totalCount = await _context.Products
            .CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        var items = await _context.Products
            .Find(filter)
            .SortByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync(cancellationToken);

        return (items, (int)totalCount);
    }

    public async Task<List<Product>> GetLowStockAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .Find(p => !p.IsDeleted && p.Stock < 10)
            .SortBy(p => p.Stock)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Product>> GetByCategoryIdAsync(
        string categoryId, 
        CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .Find(p => !p.IsDeleted && p.CategoryId == categoryId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Product> CreateAsync(Product product, CancellationToken cancellationToken = default)
    {
        await _context.Products.InsertOneAsync(product, cancellationToken: cancellationToken);
        return product;
    }

    public async Task<Product> UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        await _context.Products.ReplaceOneAsync(
            p => p.Id == product.Id,
            product,
            cancellationToken: cancellationToken);
        
        return product;
    }

    public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        
        var update = Builders<Product>.Update
            .Set(p => p.IsDeleted, true)
            .Set(p => p.UpdatedAt, DateTime.UtcNow);

        var result = await _context.Products.UpdateOneAsync(
            p => p.Id == id,
            update,
            cancellationToken: cancellationToken);

        return result.ModifiedCount > 0;
    }

    public async Task<bool> ExistsAsync(string id, CancellationToken cancellationToken = default)
    {
        var count = await _context.Products
            .CountDocumentsAsync(
                p => p.Id == id && !p.IsDeleted, 
                cancellationToken: cancellationToken);
        
        return count > 0;
    }

    public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default)
    {
        var count = await _context.Products
            .CountDocumentsAsync(
                p => !p.IsDeleted, 
                cancellationToken: cancellationToken);
        
        return (int)count;
    }

    public async Task<decimal> GetTotalInventoryValueAsync(CancellationToken cancellationToken = default)
    {
        var products = await _context.Products
            .Find(p => !p.IsDeleted)
            .ToListAsync(cancellationToken);

        return products.Sum(p => p.Price * p.Stock);
    }
}
