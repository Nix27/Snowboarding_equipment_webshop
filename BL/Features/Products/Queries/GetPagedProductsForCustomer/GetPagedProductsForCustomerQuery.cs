using BL.DTOs;
using MediatR;

namespace BL.Features.Products.Queries.GetPagedProductsForCustomer
{
    public record GetPagedProductsForCustomerQuery(CustomerProductsPageRequest productsRequest) : IRequest<IEnumerable<ProductDto>>;
}
