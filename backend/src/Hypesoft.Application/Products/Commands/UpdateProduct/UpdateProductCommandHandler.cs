namespace Hypesoft.Application.Products.Commands.UpdateProduct;

using MediatR;
using Hypesoft.Domain.Repositories;
using Hypesoft.Application.DTOs;
using Hypesoft.Domain.Exceptions;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductDto>
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public UpdateProductCommandHandler(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException("Product", request.Id);

        // Verify category exists
        if (!await _categoryRepository.ExistsAsync(request.CategoryId, cancellationToken))
        {
            throw new EntityNotFoundException("Category", request.CategoryId);
        }

        product.Update(
            request.Name,
            request.Description,
            request.Price,
            request.Stock,
            request.CategoryId
        );

        var updated = await _productRepository.UpdateAsync(product, cancellationToken);
        var category = await _categoryRepository.GetByIdAsync(updated.CategoryId, cancellationToken);

        return new ProductDto
        {
            Id = updated.Id,
            Name = updated.Name,
            Description = updated.Description,
            Price = updated.Price,
            CategoryId = updated.CategoryId,
            CategoryName = category?.Name,
            Stock = updated.Stock,
            IsLowStock = updated.IsLowStock(),
            CreatedAt = updated.CreatedAt,
            UpdatedAt = updated.UpdatedAt
        };
    }
}
