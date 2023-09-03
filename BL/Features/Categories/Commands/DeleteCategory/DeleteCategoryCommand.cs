using BL.DTOs;
using MediatR;

namespace BL.Features.Categories.Commands.DeleteCategory
{
    public record DeleteCategoryCommand(CategoryDto categoryForDelete) : IRequest<int>;
}
