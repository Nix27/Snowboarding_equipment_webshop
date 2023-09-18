using BL.DTOs;
using MediatR;

namespace BL.Features.Categories.Queries.GetCategoryById
{
    public record GetCategoryByIdQuery(int id, bool isTracked = true) : IRequest<CategoryDto>;
}
