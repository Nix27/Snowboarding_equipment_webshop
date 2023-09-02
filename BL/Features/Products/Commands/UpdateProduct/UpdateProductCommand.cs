using BL.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Features.Products.Commands.UpdateProduct
{
    public record UpdateProductCommand(ProductDto productForUpdate) : IRequest<int>;
}
