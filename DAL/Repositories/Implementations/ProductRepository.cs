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
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
        }

        public async Task IncreaseAmountOfSoldAsync(int productId, int increaseOfAmount)
        {
            var productFromDb = await dbSet.FirstOrDefaultAsync(p => p.Id == productId);

            if (productFromDb != null)
            {
                productFromDb.AmountOfSold += increaseOfAmount;
            }
        }

        public async Task UpdateAsync(Product productForUpdate)
        {
            var productFromDb = await dbSet.FirstOrDefaultAsync(p => p.Id == productForUpdate.Id);

            if(productFromDb != null)
            {
                productFromDb.Name = productForUpdate.Name;
                productFromDb.Description = productForUpdate.Description;
                productFromDb.QuantityInStock = productForUpdate.QuantityInStock;
                productFromDb.Price = productForUpdate.Price;
                productFromDb.PriceFor5To10 = productForUpdate.PriceFor5To10;
                productFromDb.PriceForMoreThan10 = productForUpdate.PriceForMoreThan10;
                productFromDb.OldPrice = productForUpdate.OldPrice;
                productFromDb.CategoryId = productForUpdate.CategoryId;
                productFromDb.ThumbnailImageId = productForUpdate.ThumbnailImageId;
            }
        }
    }
}
