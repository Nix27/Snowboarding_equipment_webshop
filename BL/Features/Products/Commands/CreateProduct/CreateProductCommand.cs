using BL.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Features.Products.Commands.CreateProduct
{
    public record CreateProductCommand(ProductDto newProduct) : IRequest<int?>;
}
