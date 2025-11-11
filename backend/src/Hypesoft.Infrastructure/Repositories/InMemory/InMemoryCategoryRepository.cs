using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hypesoft.Domain.Entities;
using Hypesoft.Domain.Repositories;

namespace Hypesoft.Infrastructure.Repositories.InMemory;

public class InMemoryCategoryRepository : InMemoryRepository<Category>, ICategoryRepository
{
    private readonly IProductRepository _productRepository;

    public InMemoryCategoryRepository(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public new Task<List<Category>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var categories = _items
            .Where(c => !c.IsDeleted)
            .OrderBy(c => c.Name)
            .ToList();
        return Task.FromResult(categories);
    }

    public async Task<bool> HasProductsAsync(string categoryId, CancellationToken cancellationToken = default)
    {
        var products = await _productRepository.GetByCategoryIdAsync(categoryId, cancellationToken);
        return products.Any();
    }
}
