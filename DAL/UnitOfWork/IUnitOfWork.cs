using DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.UnitOfWork
{
    public interface IUnitOfWork
    {
        public ICategoryRepository Category { get; }
        public ICompanyRepository Company { get; }
        public ICountryRepository Country { get; }
        public IProductRepository Product { get; }
        public IThumbnailImageRepository ThumbnailImage { get; }
        public IGalleryImageRepository GalleryImage { get; }
        public IUserRepository User { get; }

        Task SaveAsync();
    }
}
