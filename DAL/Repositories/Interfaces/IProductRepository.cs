﻿using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task UpdateAsync(Product productForUpdate);
        Task IncreaseAmountOfSoldAsync(int productId, int increaseOfAmount);
    }
}
