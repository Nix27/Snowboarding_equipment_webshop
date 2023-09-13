﻿using DAL.Repositories.Interfaces;

namespace DAL.UnitOfWork
{
    public interface IUnitOfWork
    {
        public ICategoryRepository Category { get; }
        public ICountryRepository Country { get; }
        public IProductRepository Product { get; }
        public IThumbnailImageRepository ThumbnailImage { get; }
        public IGalleryImageRepository GalleryImage { get; }
        public IUserRepository User { get; }
        public IShoppingCartItemRepository ShoppingCartItem { get; }
        public IOrderHeaderRepository OrderHeader { get; }
        public IOrderDetailRepository OrderDetail { get; }

        Task SaveAsync();
    }
}
