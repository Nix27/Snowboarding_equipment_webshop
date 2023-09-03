using BL.DTOs;
using MediatR;

namespace BL.Features.Categories.Commands.UpdateCategory
{
    public record UpdateCategoryCommand(CategoryDto categoryForUpdate) : IRequest<int>;
}
