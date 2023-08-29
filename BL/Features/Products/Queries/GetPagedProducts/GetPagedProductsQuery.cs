using BL.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Features.Products.Queries.GetPagedProducts
{
    public record GetPagedProductsQuery(int page, int size, string filterBy, string? searchTerm) : IRequest<IEnumerable<ProductDto>?>;
}
