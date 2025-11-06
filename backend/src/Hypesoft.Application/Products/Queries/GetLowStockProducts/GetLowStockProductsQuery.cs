namespace Hypesoft.Application.Products.Queries.GetLowStockProducts;

using MediatR;
using Hypesoft.Application.DTOs;

public record GetLowStockProductsQuery : IRequest<List<ProductDto>>;
