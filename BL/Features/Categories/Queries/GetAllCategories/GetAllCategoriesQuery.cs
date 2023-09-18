using BL.DTOs;
using DAL.Models;
using MediatR;
using System.Linq.Expressions;

namespace BL.Features.Categories.Queries.GetAllCategories
{
    public record GetAllCategoriesQuery(
        Expression<Func<Category, bool>>? filter = null, 
        string? includeProperties = null, 
        bool isTracked = true) : IRequest<IEnumerable<CategoryDto>>;
}
