namespace Hypesoft.Application.Categories.Commands.UpdateCategory;

using MediatR;
using Hypesoft.Domain.Repositories;
using Hypesoft.Application.DTOs;
using Hypesoft.Domain.Exceptions;

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, CategoryDto>
{
    private readonly ICategoryRepository _categoryRepository;

    public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryDto> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException("Category", request.Id);

        category.Update(request.Name, request.Description);

        var updated = await _categoryRepository.UpdateAsync(category, cancellationToken);

        return new CategoryDto
        {
            Id = updated.Id,
            Name = updated.Name,
            Description = updated.Description,
            CreatedAt = updated.CreatedAt,
            UpdatedAt = updated.UpdatedAt
        };
    }
}