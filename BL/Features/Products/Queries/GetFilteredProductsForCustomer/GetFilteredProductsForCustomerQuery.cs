using BL.DTOs;
using MediatR;

namespace BL.Features.Products.Queries.GetFilteredProductsForCustomer
{
    public record GetFilteredProductsForCustomerQuery(FilterCustomerProductsRequestDto filter) : IRequest<IEnumerable<ProductDto>>;
}
