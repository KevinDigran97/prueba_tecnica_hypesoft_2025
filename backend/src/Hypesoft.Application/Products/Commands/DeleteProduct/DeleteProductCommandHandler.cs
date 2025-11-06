namespace Hypesoft.Application.Products.Commands.DeleteProduct;

using MediatR;
using Hypesoft.Domain.Repositories;
using Hypesoft.Domain.Exceptions;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
{
    private readonly IProductRepository _productRepository;

    public DeleteProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        if (!await _productRepository.ExistsAsync(request.Id, cancellationToken))
        {
            throw new EntityNotFoundException("Product", request.Id);
        }

        return await _productRepository.DeleteAsync(request.Id, cancellationToken);
    }
}