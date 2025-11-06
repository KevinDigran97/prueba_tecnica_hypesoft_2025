namespace Hypesoft.Application.Products.Commands.CreateProduct;

using MediatR;
using Hypesoft.Application.DTOs;

public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    string CategoryId,
    int Stock
) : IRequest<ProductDto>;