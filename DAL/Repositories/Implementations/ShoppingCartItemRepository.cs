using DAL.AppDbContext;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Implementations
{
    public class ShoppingCartItemRepository : Repository<ShoppingCartItem>, IShoppingCartItemRepository
    {
        public ShoppingCartItemRepository(ApplicationDbContext db) : base(db)
        {
        }

        public async Task DecrementQuantity(int shoppingCartItemId, int quantity)
        {
            var shoppingCart = await dbSet.FirstOrDefaultAsync(s => s.Id == shoppingCartItemId);
            shoppingCart.Quantity -= quantity;
        }

        public async Task IncrementQuantity(int shoppingCartItemId, int quantity)
        {
            var shoppingCart = await dbSet.FirstOrDefaultAsync(s => s.Id == shoppingCartItemId);
            shoppingCart.Quantity += quantity;
        }
    }
}
