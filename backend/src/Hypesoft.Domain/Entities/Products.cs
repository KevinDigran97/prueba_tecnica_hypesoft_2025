namespace Hypesoft.Domain.Entities;

using Hypesoft.Domain.Common;

public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string CategoryId { get; set; } = string.Empty;
    public int Stock { get; set; }

    
    public Category? Category { get; set; }

    public bool IsLowStock() => Stock < 10;

    public void UpdateStock(int quantity)
    {
        if (Stock + quantity < 0)
            throw new InvalidOperationException("Stock cannot be negative");
        
        Stock += quantity;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(string name, string description, decimal price, int stock, string categoryId)
    {
        Name = name;
        Description = description;
        Price = price;
        Stock = stock;
        CategoryId = categoryId;
        UpdatedAt = DateTime.UtcNow;
    }
}
