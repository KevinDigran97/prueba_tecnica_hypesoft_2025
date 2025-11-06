namespace Hypesoft.Application.Categories.Commands.CreateCategory;

using MediatR;
using Hypesoft.Application.DTOs;

public record CreateCategoryCommand(
    string Name,
    string Description
) : IRequest<CategoryDto>;
