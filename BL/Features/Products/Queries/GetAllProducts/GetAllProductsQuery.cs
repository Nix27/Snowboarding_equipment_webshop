using BL.DTOs;
using DAL.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BL.Features.Products.Queries.GetAllProducts
{
    public record GetAllProductsQuery(Expression<Func<Product, bool>>? filter = null) : IRequest<IEnumerable<ProductDto>?>;
}
