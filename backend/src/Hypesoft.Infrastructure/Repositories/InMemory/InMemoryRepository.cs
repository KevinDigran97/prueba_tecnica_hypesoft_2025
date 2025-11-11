using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hypesoft.Domain.Common;

namespace Hypesoft.Infrastructure.Repositories.InMemory;

public abstract class InMemoryRepository<T> where T : BaseEntity
{
    protected readonly List<T> _items = new();

    public Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var item = _items.FirstOrDefault(x => x.Id == id && !x.IsDeleted);
        return Task.FromResult(item);
    }

    public Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = _items.Where(x => !x.IsDeleted).ToList();
        return Task.FromResult(items);
    }

    public Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(entity.Id))
        {
            entity.Id = Guid.NewGuid().ToString();
        }
        entity.CreatedAt = DateTime.UtcNow;
        _items.Add(entity);
        return Task.FromResult(entity);
    }

    public Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        var index = _items.FindIndex(x => x.Id == entity.Id);
        if (index != -1)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            _items[index] = entity;
        }
        return Task.FromResult(entity);
    }

    public Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var item = _items.FirstOrDefault(x => x.Id == id);
        if (item != null)
        {
            item.IsDeleted = true;
            item.UpdatedAt = DateTime.UtcNow;
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }

    public Task<bool> ExistsAsync(string id, CancellationToken cancellationToken = default)
    {
        var exists = _items.Any(x => x.Id == id && !x.IsDeleted);
        return Task.FromResult(exists);
    }
}
