using DAL.Models;
using MediatR;
using System.Linq.Expressions;

namespace BL.Features.Products.Queries.GetNumberOfProducts
{
    public record GetNumberOfProductsQuery(Expression<Func<Product, bool>>? filter = null) : IRequest<int>;
}
