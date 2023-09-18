using BL.DTOs;
using MediatR;

namespace BL.Features.Categories.Commands.DeleteCategory
{
    public record DeleteCategoryCommand(int categoryId) : IRequest<int>;
}
