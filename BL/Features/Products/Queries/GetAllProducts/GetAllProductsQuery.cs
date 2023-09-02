using BL.DTOs;
using DAL.Models;
using MediatR;
using System.Linq.Expressions;

namespace BL.Features.Products.Queries.GetAllProducts
{
    public record GetAllProductsQuery(Expression<Func<Product, bool>>? filter = null, bool isTracked = true) : IRequest<IEnumerable<ProductDto>>;
}
