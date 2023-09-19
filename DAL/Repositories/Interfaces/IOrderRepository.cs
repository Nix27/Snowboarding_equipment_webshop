using DAL.Models;

namespace DAL.Repositories.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        void Update(Order order);
        void UpdateStatus(int orderId, string orderStatus, string? paymentStatus = null);
        void UpdateStripePaymentId(int orderId, string sessionId, string paymentIntentId);
    }
}
