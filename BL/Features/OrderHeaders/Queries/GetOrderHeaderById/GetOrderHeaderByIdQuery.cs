using BL.DTOs;
using MediatR;

namespace BL.Features.OrderHeaders.Queries.GetOrderHeaderById
{
    public record GetOrderHeaderByIdQuery(int orderHeaderId) : IRequest<OrderHeaderDto?>;
}
