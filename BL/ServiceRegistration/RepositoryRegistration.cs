using DAL.Repositories.Implementations;
using DAL.Repositories.Interfaces;
using DAL.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;

namespace BL.ServiceRegistration
{
    public static class RepositoryRegistration
    {
        public static void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IGalleryImageRepository, GalleryImageRepository>();
            services.AddScoped<IThumbnailImageRepository, ThumbnailImageRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IShoppingCartItemRepository, ShoppingCartItemRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderItemRepository, OrderItemRepository>();
        }
    }
}
