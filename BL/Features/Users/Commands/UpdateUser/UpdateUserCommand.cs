using BL.DTOs;
using MediatR;

namespace BL.Features.Users.Commands.UpdateUser
{
    public record UpdateUserCommand(UserDto userForUpdate) : IRequest;
}
