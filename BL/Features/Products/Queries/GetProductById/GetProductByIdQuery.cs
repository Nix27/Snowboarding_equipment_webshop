using BL.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Features.Products.Queries.GetProductById
{
    public record GetProductByIdQuery(int id, bool isTracked = true) : IRequest<ProductDto?>;
}
