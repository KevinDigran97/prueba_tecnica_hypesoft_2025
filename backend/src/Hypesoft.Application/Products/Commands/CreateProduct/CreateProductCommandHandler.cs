namespace Hypesoft.Application.Products.Commands.CreateProduct;

using MediatR;
using Hypesoft.Domain.Entities;
using Hypesoft.Domain.Repositories;
using Hypesoft.Application.DTOs;
using Hypesoft.Domain.Exceptions;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public CreateProductCommandHandler(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {

        if (!await _categoryRepository.ExistsAsync(request.CategoryId, cancellationToken))
        {
            throw new EntityNotFoundException("Category", request.CategoryId);
        }

        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            CategoryId = request.CategoryId,
            Stock = request.Stock
        };

        var created = await _productRepository.CreateAsync(product, cancellationToken);
        

        var category = await _categoryRepository.GetByIdAsync(created.CategoryId, cancellationToken);

        return new ProductDto
        {
            Id = created.Id,
            Name = created.Name,
            Description = created.Description,
            Price = created.Price,
            CategoryId = created.CategoryId,
            CategoryName = category?.Name,
            Stock = created.Stock,
            IsLowStock = created.IsLowStock(),
            CreatedAt = created.CreatedAt,
            UpdatedAt = created.UpdatedAt
        };
    }
}
