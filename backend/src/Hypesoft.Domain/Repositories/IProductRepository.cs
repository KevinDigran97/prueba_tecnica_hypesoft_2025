namespace Hypesoft.Domain.Repositories;

using Hypesoft.Domain.Entities;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<List<Product>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<(List<Product> Items, int TotalCount)> GetPagedAsync(
        int page, 
        int pageSize, 
        string? search = null,
        string? categoryId = null,
        CancellationToken cancellationToken = default);
    Task<List<Product>> GetLowStockAsync(CancellationToken cancellationToken = default);
    Task<List<Product>> GetByCategoryIdAsync(string categoryId, CancellationToken cancellationToken = default);
    Task<Product> CreateAsync(Product product, CancellationToken cancellationToken = default);
    Task<Product> UpdateAsync(Product product, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string id, CancellationToken cancellationToken = default);
    Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default);
    Task<decimal> GetTotalInventoryValueAsync(CancellationToken cancellationToken = default);
}