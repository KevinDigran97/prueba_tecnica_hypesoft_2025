namespace Hypesoft.Application.Categories.Queries.GetCategoryById;

using MediatR;
using Hypesoft.Application.DTOs;

public record GetCategoryByIdQuery(string Id) : IRequest<CategoryDto>;