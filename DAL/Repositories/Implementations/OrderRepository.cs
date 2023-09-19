using DAL.AppDbContext;
using DAL.Models;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories.Implementations
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext db) : base(db)
        {
        }

        public void Update(Order order)
        {
            dbSet.Update(order);
        }

        public void UpdateStatus(int orderId, string orderStatus, string? paymentStatus = null)
        {
            var order = dbSet.FirstOrDefault(o => o.Id == orderId);

            if (order == null)
                throw new InvalidOperationException("Order not found.");

            order.OrderStatus = orderStatus;

            if(!String.IsNullOrEmpty(paymentStatus))
                order.PaymentStatus = paymentStatus;
        }

        public void UpdateStripePaymentId(int orderId, string sessionId, string paymentIntentId)
        {
            var order = dbSet.FirstOrDefault(o => o.Id == orderId);

            if(!String.IsNullOrEmpty(sessionId))
                order.SessionId = sessionId;

            if (!String.IsNullOrEmpty(paymentIntentId))
            {
                order.PaymentIntentId = paymentIntentId;
                order.PaymentDate = DateTime.Now;
            }
        }
    }
}
