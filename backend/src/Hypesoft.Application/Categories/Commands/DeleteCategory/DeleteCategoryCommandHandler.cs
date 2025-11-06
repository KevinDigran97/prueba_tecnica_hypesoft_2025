namespace Hypesoft.Application.Categories.Commands.DeleteCategory;

using MediatR;
using Hypesoft.Domain.Repositories;
using Hypesoft.Domain.Exceptions;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, bool>
{
    private readonly ICategoryRepository _categoryRepository;

    public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        if (!await _categoryRepository.ExistsAsync(request.Id, cancellationToken))
        {
            throw new EntityNotFoundException("Category", request.Id);
        }

        // Check if category has products
        if (await _categoryRepository.HasProductsAsync(request.Id, cancellationToken))
        {
            throw new InvalidOperationDomainException(
                "Cannot delete category that has associated products");
        }

        return await _categoryRepository.DeleteAsync(request.Id, cancellationToken);
    }
}