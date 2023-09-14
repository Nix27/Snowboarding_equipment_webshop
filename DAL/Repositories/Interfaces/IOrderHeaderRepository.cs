using DAL.Models;

namespace DAL.Repositories.Interfaces
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        void UpdateStatus(int orderHeaderId, string orderStatus, string? paymentStatus = null);
        void UpdateStripePaymentId(int orderHeaderId, string sessionId, string paymentIntentId);
    }
}
