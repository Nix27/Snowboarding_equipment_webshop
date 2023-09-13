using DAL.AppDbContext;
using DAL.Repositories.Implementations;
using DAL.Repositories.Interfaces;

namespace DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Country = new CountryRepository(_db);
            Product = new ProductRepository(_db);
            ThumbnailImage = new ThumbnailImageRepository(_db);
            GalleryImage = new GalleryImageRepository(_db);
            User = new UserRepository(_db);
            ShoppingCartItem = new ShoppingCartItemRepository(_db);
            OrderHeader = new OrderHeaderRepository(_db);
            OrderDetail = new OrderDetailRepository(_db);
        }

        public ICategoryRepository Category { get; init; }

        public ICountryRepository Country { get; init; }

        public IProductRepository Product { get; init; }

        public IThumbnailImageRepository ThumbnailImage { get; init; }

        public IGalleryImageRepository GalleryImage { get; init; }

        public IUserRepository User { get; init; }

        public IShoppingCartItemRepository ShoppingCartItem { get; init; }

        public IOrderHeaderRepository OrderHeader { get; init; }

        public IOrderDetailRepository OrderDetail { get; init; }

        public Task SaveAsync()
        {
            return _db.SaveChangesAsync();
        }
    }
}
