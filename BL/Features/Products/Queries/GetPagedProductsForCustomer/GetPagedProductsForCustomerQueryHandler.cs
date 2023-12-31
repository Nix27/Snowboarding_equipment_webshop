﻿using BL.DTOs;
using BL.Features.Products.Queries.GetAllProducts;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace BL.Features.Products.Queries.GetPagedProductsForCustomer
{
    internal class GetPagedProductsForCustomerQueryHandler : IRequestHandler<GetPagedProductsForCustomerQuery, (IEnumerable<ProductDto>, int)>
    {
        private readonly IMediator _mediator;

        public GetPagedProductsForCustomerQueryHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<(IEnumerable<ProductDto>, int)> Handle(GetPagedProductsForCustomerQuery request, CancellationToken cancellationToken)
        {
            var products = await _mediator.Send(new GetAllProductsQuery(includeProperties: "Category"));

            int page = request.productsRequest.Page;
            float size = request.productsRequest.Size;
            var categories = request.productsRequest.Categories;
            double minPrice = request.productsRequest.MinPrice;
            double maxPrice = request.productsRequest.MaxPrice;
            string sortBy = request.productsRequest.SortBy;
            string? searchTerm = request.productsRequest.SearchTerm;

            if (categories.Count() > 0 && !categories.Contains("all"))
            {
                products = products.Where(p => categories.Contains(p.Category.Name.ToLower()));
            }

            if(maxPrice > 0 && minPrice <= maxPrice)
            {
                products = products.Where(p => p.Price >= minPrice && p.Price <= maxPrice);
            }

            if (!String.IsNullOrEmpty(searchTerm))
            {
                products = products.Where(p => p.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            if(sortBy == "lower price")
            {
                products = products.OrderBy(p => p.Price);
            }
            else if(sortBy == "higher price")
            {
                products = products.OrderBy(p => p.Price).Reverse();
            }

            int numberOfProducts = products.Count();

            var pagedProducts = products.Skip((page - 1) * (int)size).Take((int)size);

            return (pagedProducts, numberOfProducts);
        }
    }
}
