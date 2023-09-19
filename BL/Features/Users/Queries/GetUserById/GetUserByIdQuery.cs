using BL.DTOs;
using MediatR;

namespace BL.Features.Users.Queries.GetUserById
{
    public record GetUserByIdQuery(string userId) : IRequest<UserDto>;
}
