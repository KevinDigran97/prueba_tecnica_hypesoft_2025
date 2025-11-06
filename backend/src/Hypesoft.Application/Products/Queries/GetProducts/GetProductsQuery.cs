namespace Hypesoft.Application.Products.Queries.GetProducts;

using MediatR;
using Hypesoft.Application.Common.Models;
using Hypesoft.Application.DTOs;

public record GetProductsQuery(
    int Page = 1,
    int PageSize = 10,
    string? Search = null,
    string? CategoryId = null
) : IRequest<PagedResult<ProductDto>>;