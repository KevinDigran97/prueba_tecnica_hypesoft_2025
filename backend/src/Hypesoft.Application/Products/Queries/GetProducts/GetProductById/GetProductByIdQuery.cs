namespace Hypesoft.Application.Products.Queries.GetProductById;

using MediatR;
using Hypesoft.Application.DTOs;

public record GetProductByIdQuery(string Id) : IRequest<ProductDto>;