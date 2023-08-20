using DAL.AppDbContext;
using DAL.Repositories.Implementations;
using DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Company = new CompanyRepository(_db);
            Country = new CountryRepository(_db);
            Product = new ProductRepository(_db);
            ThumbnailImage = new ThumbnailImageRepository(_db);
            GalleryImage = new GalleryImageRepository(_db);
            User = new UserRepository(_db);
        }

        public ICategoryRepository Category { get; init; }

        public ICompanyRepository Company { get; init; }

        public ICountryRepository Country { get; init; }

        public IProductRepository Product { get; init; }

        public IThumbnailImageRepository ThumbnailImage { get; init; }

        public IGalleryImageRepository GalleryImage { get; init; }

        public IUserRepository User { get; init; }

        public Task SaveAsync()
        {
            return _db.SaveChangesAsync();
        }
    }
}
