using DAL.Models;
using MediatR;
using System.Linq.Expressions;

namespace BL.Features.Categories.Queries.GetNumberOfCategories
{
    public record GetNumberOfCategoriesQuery(Expression<Func<Category, bool>>? filter = null) : IRequest<int>;
}
