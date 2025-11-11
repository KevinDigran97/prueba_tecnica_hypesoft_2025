using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hypesoft.Domain.Entities;
using Hypesoft.Domain.Repositories;

namespace Hypesoft.Infrastructure.Repositories.InMemory;

public class InMemoryProductRepository : InMemoryRepository<Product>, IProductRepository
{
    public Task<(List<Product> Items, int TotalCount)> GetPagedAsync(
        int page, 
        int pageSize, 
        string? search = null,
        string? categoryId = null,
        CancellationToken cancellationToken = default)
    {
        var query = _items.Where(p => !p.IsDeleted).AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p => p.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(categoryId))
        {
            query = query.Where(p => p.CategoryId == categoryId);
        }

        var totalCount = query.Count();
        var items = query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return Task.FromResult((items, totalCount));
    }

    public Task<List<Product>> GetLowStockAsync(CancellationToken cancellationToken = default)
    {
        var products = _items
            .Where(p => !p.IsDeleted && p.Stock < 10)
            .OrderBy(p => p.Stock)
            .ToList();
        return Task.FromResult(products);
    }

    public Task<List<Product>> GetByCategoryIdAsync(string categoryId, CancellationToken cancellationToken = default)
    {
        var products = _items
            .Where(p => !p.IsDeleted && p.CategoryId == categoryId)
            .ToList();
        return Task.FromResult(products);
    }

    public Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default)
    {
        var count = _items.Count(p => !p.IsDeleted);
        return Task.FromResult(count);
    }

    public Task<decimal> GetTotalInventoryValueAsync(CancellationToken cancellationToken = default)
    {
        var totalValue = _items
            .Where(p => !p.IsDeleted)
            .Sum(p => p.Price * p.Stock);
        return Task.FromResult(totalValue);
    }
}
