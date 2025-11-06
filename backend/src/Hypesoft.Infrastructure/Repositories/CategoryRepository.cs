namespace Hypesoft.Infrastructure.Repositories;

using MongoDB.Driver;
using Hypesoft.Domain.Entities;
using Hypesoft.Domain.Repositories;
using Hypesoft.Infrastructure.Data;

public class CategoryRepository : ICategoryRepository
{
    private readonly MongoDbContext _context;

    public CategoryRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<Category?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .Find(c => c.Id == id && !c.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<Category>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .Find(c => !c.IsDeleted)
            .SortBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Category> CreateAsync(Category category, CancellationToken cancellationToken = default)
    {
        await _context.Categories.InsertOneAsync(category, cancellationToken: cancellationToken);
        return category;
    }

    public async Task<Category> UpdateAsync(Category category, CancellationToken cancellationToken = default)
    {
        await _context.Categories.ReplaceOneAsync(
            c => c.Id == category.Id,
            category,
            cancellationToken: cancellationToken);
        
        return category;
    }

    public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        
        var update = Builders<Category>.Update
            .Set(c => c.IsDeleted, true)
            .Set(c => c.UpdatedAt, DateTime.UtcNow);

        var result = await _context.Categories.UpdateOneAsync(
            c => c.Id == id,
            update,
            cancellationToken: cancellationToken);

        return result.ModifiedCount > 0;
    }

    public async Task<bool> ExistsAsync(string id, CancellationToken cancellationToken = default)
    {
        var count = await _context.Categories
            .CountDocumentsAsync(
                c => c.Id == id && !c.IsDeleted, 
                cancellationToken: cancellationToken);
        
        return count > 0;
    }

    public async Task<bool> HasProductsAsync(string categoryId, CancellationToken cancellationToken = default)
    {
        var count = await _context.Products
            .CountDocumentsAsync(
                p => p.CategoryId == categoryId && !p.IsDeleted, 
                cancellationToken: cancellationToken);
        
        return count > 0;
    }
}