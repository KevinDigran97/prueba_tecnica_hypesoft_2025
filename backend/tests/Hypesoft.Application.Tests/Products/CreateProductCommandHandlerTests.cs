namespace Hypesoft.Application.Tests.Products.Commands;

using Xunit;
using Moq;
using FluentAssertions;
using Hypesoft.Application.Products.Commands.CreateProduct;
using Hypesoft.Domain.Entities;
using Hypesoft.Domain.Repositories;
using Hypesoft.Domain.Exceptions;

public class CreateProductCommandHandlerTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly CreateProductCommandHandler _handler;

    public CreateProductCommandHandlerTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _handler = new CreateProductCommandHandler(
            _productRepositoryMock.Object,
            _categoryRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ReturnsProductDto()
    {
        // Arrange
        var command = new CreateProductCommand(
            "Test Product",
            "Test Description",
            99.99m,
            "category-123",
            50
        );

        var category = new Category
        {
            Id = "category-123",
            Name = "Electronics",
            Description = "Electronic items"
        };

        _categoryRepositoryMock
            .Setup(x => x.ExistsAsync(command.CategoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _categoryRepositoryMock
            .Setup(x => x.GetByIdAsync(command.CategoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        _productRepositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product p, CancellationToken ct) => p);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(command.Name);
        result.Description.Should().Be(command.Description);
        result.Price.Should().Be(command.Price);
        result.CategoryId.Should().Be(command.CategoryId);
        result.CategoryName.Should().Be(category.Name);
        result.Stock.Should().Be(command.Stock);
        result.IsLowStock.Should().BeFalse();

        _productRepositoryMock.Verify(
            x => x.CreateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_CategoryDoesNotExist_ThrowsEntityNotFoundException()
    {
        // Arrange
        var command = new CreateProductCommand(
            "Test Product",
            "Test Description",
            99.99m,
            "non-existent-category",
            50
        );

        _categoryRepositoryMock
            .Setup(x => x.ExistsAsync(command.CategoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            _handler.Handle(command, CancellationToken.None));

        _productRepositoryMock.Verify(
            x => x.CreateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_LowStock_SetsIsLowStockTrue()
    {
        // Arrange
        var command = new CreateProductCommand(
            "Low Stock Product",
            "Test Description",
            99.99m,
            "category-123",
            5  // Low stock
        );

        var category = new Category { Id = "category-123", Name = "Test" };

        _categoryRepositoryMock
            .Setup(x => x.ExistsAsync(command.CategoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _categoryRepositoryMock
            .Setup(x => x.GetByIdAsync(command.CategoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        _productRepositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product p, CancellationToken ct) => p);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsLowStock.Should().BeTrue();
    }
}