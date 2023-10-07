using BL.DTOs;
using MediatR;

namespace BL.Features.Users.Queries.GetPagedUsers
{
    public record GetPagedUsersQuery(int page, float size, string? searchBy, string? searchTerm) : IRequest<(IEnumerable<UserDto>, int)>;
}
