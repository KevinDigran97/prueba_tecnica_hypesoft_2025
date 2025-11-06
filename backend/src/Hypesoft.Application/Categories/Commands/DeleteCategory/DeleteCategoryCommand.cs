namespace Hypesoft.Application.Categories.Commands.DeleteCategory;

using MediatR;

public record DeleteCategoryCommand(string Id) : IRequest<bool>;