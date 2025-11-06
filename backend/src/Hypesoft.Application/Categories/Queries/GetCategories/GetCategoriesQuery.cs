namespace Hypesoft.Application.Categories.Queries.GetCategories;

using MediatR;
using Hypesoft.Application.DTOs;

public record GetCategoriesQuery : IRequest<List<CategoryDto>>;
