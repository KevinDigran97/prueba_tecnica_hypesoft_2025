namespace Hypesoft.Domain.Tests.Entities;

using Xunit;
using FluentAssertions;
using Hypesoft.Domain.Entities;

public class ProductTests
{
    [Fact]
    public void IsLowStock_StockLessThan10_ReturnsTrue()
    {
        // Arrange
        var product = new Product { Stock = 5 };

        // Act
        var result = product.IsLowStock();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsLowStock_Stock10OrMore_ReturnsFalse()
    {
        // Arrange
        var product = new Product { Stock = 10 };

        // Act
        var result = product.IsLowStock();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void UpdateStock_ValidQuantity_UpdatesStock()
    {
        // Arrange
        var product = new Product { Stock = 10 };

        // Act
        product.UpdateStock(5);

        // Assert
        product.Stock.Should().Be(15);
        product.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void UpdateStock_NegativeResultingStock_ThrowsException()
    {
        // Arrange
        var product = new Product { Stock = 5 };

        // Act & Assert
        Action act = () => product.UpdateStock(-10);
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Stock cannot be negative");
    }

    [Fact]
    public void Update_ValidData_UpdatesAllProperties()
    {
        // Arrange
        var product = new Product
        {
            Name = "Old Name",
            Description = "Old Description",
            Price = 10m,
            Stock = 5,
            CategoryId = "old-category"
        };

        // Act
        product.Update("New Name", "New Description", 20m, 10, "new-category");

        // Assert
        product.Name.Should().Be("New Name");
        product.Description.Should().Be("New Description");
        product.Price.Should().Be(20m);
        product.Stock.Should().Be(10);
        product.CategoryId.Should().Be("new-category");
        product.UpdatedAt.Should().NotBeNull();
    }
}