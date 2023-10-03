using BL.DTOs;
using MediatR;

namespace BL.Features.Categories.Queries.GetPagedCategories
{
    public record GetPagedCategoriesQuery(IEnumerable<CategoryDto> categories, int page, float size) : IRequest<IEnumerable<CategoryDto>>;
}
