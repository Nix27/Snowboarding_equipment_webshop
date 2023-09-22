using BL.Features.ShoppingCartItem.Queries.GetNumberOfShoppingCartItemsForUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Utilities.Constants.SessionKeys;

namespace Snowboarding_equipment_webshop.ViewComponents
{
    public class ShoppingCart : ViewComponent
    {
        private readonly IMediator _mediator;

        public ShoppingCart(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            
            if(claim != null) 
            {
                if(HttpContext.Session.GetInt32(SessionKey.ShoppingCart) == null)
                {
                    var numberOfShoppingCartItems = await _mediator.Send(new GetNumberOfShoppingCartItemsForUserQuery(claim.Value));
                    HttpContext.Session.SetInt32(SessionKey.ShoppingCart, numberOfShoppingCartItems);
                }

                return View(HttpContext.Session.GetInt32(SessionKey.ShoppingCart));
            }
            else
            {
                HttpContext.Session.Remove(SessionKey.ShoppingCart);
                return View(0);
            }
        }
    }
}
