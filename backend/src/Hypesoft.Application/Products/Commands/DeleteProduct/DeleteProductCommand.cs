namespace Hypesoft.Application.Products.Commands.DeleteProduct;

using MediatR;

public record DeleteProductCommand(string Id) : IRequest<bool>;