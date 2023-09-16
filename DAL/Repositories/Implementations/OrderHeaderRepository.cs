using DAL.AppDbContext;
using DAL.Models;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories.Implementations
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
        }

        public void Update(OrderHeader orderHeader)
        {
            dbSet.Update(orderHeader);
        }

        public void UpdateStatus(int orderHeaderId, string orderStatus, string? paymentStatus = null)
        {
            var orderHeader = dbSet.FirstOrDefault(o => o.Id == orderHeaderId);

            if (orderHeader == null)
                throw new InvalidOperationException("OrderHeader not found.");

            orderHeader.OrderStatus = orderStatus;

            if(!String.IsNullOrEmpty(paymentStatus))
                orderHeader.PaymentStatus = paymentStatus;
        }

        public void UpdateStripePaymentId(int orderHeaderId, string sessionId, string paymentIntentId)
        {
            var orderHeader = dbSet.FirstOrDefault(o => o.Id == orderHeaderId);

            if(!String.IsNullOrEmpty(sessionId))
                orderHeader.SessionId = sessionId;

            if (!String.IsNullOrEmpty(paymentIntentId))
            {
                orderHeader.PaymentIntentId = paymentIntentId;
                orderHeader.PaymentDate = DateTime.Now;
            }
        }
    }
}
