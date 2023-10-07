using BL.DTOs;
using DAL.Models;
using MediatR;
using System.Linq.Expressions;

namespace BL.Features.Users.Queries.GetAllUsers
{
    public record GetAllUsersQuery(Expression<Func<User, bool>>? filter = null,
        string? includeProperties = null,
        bool isTracked = true) : IRequest<IEnumerable<UserDto>>;
}
