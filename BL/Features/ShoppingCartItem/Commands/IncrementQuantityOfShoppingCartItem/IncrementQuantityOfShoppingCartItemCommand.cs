using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Features.ShoppingCartItem.Commands.IncrementQuantityOfShoppingCartItem
{
    public record IncrementQuantityOfShoppingCartItemCommand(int shoppingCartItemId, int quantity) : IRequest;
}
