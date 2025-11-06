namespace Hypesoft.Application.Categories.Commands.UpdateCategory;

using MediatR;
using Hypesoft.Application.DTOs;

public record UpdateCategoryCommand(
    string Id,
    string Name,
    string Description
) : IRequest<CategoryDto>;