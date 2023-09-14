using BL.DTOs;
using MediatR;

namespace BL.Features.OrderDetails.Commands.CreateOrderDetail
{
    public record CreateOrderDetailCommand(OrderDetailDto newOrderDetail) : IRequest<int>;
}
