using DAL.AppDbContext;
using DAL.Models;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories.Implementations
{
    public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
