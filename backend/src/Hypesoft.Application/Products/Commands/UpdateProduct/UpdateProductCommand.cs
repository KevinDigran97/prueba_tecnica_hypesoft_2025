namespace Hypesoft.Application.Products.Commands.UpdateProduct;

using MediatR;
using Hypesoft.Application.DTOs;

public record UpdateProductCommand(
    string Id,
    string Name,
    string Description,
    decimal Price,
    string CategoryId,
    int Stock
) : IRequest<ProductDto>;