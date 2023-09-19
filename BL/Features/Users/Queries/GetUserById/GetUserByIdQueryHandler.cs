using AutoMapper;
using BL.DTOs;
using DAL.Repositories.Interfaces;
using MediatR;

namespace BL.Features.Users.Queries.GetUserById
{
    internal class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUserByIdQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var requestedUser = await _userRepository.GetFirstOrDefaultAsync(u => u.Id == request.userId, includeProperties:"Country");

            if (requestedUser == null)
                throw new InvalidOperationException("User not found");
            
            return _mapper.Map<UserDto>(requestedUser);
        }
    }
}
