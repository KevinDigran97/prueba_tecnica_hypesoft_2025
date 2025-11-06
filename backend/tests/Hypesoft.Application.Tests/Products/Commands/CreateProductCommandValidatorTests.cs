namespace Hypesoft.Application.Tests.Products.Commands;

using Xunit;
using FluentAssertions;
using Hypesoft.Application.Products.Commands.CreateProduct;

public class CreateProductCommandValidatorTests
{
    private readonly CreateProductCommandValidator _validator;

    public CreateProductCommandValidatorTests()
    {
        _validator = new CreateProductCommandValidator();
    }

    [Fact]
    public void Validate_ValidCommand_PassesValidation()
    {
        // Arrange
        var command = new CreateProductCommand(
            "Valid Product",
            "Valid Description",
            99.99m,
            "category-123",
            50
        );

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Validate_EmptyName_FailsValidation(string? name)
    {
        // Arrange
        var command = new CreateProductCommand(
            name!,
            "Valid Description",
            99.99m,
            "category-123",
            50
        );

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }

    [Fact]
    public void Validate_NegativePrice_FailsValidation()
    {
        // Arrange
        var command = new CreateProductCommand(
            "Valid Product",
            "Valid Description",
            -10m,
            "category-123",
            50
        );

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Price");
    }

    [Fact]
    public void Validate_NegativeStock_FailsValidation()
    {
        // Arrange
        var command = new CreateProductCommand(
            "Valid Product",
            "Valid Description",
            99.99m,
            "category-123",
            -5
        );

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Stock");
    }
}