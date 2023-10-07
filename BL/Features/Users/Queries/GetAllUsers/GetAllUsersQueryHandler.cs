using AutoMapper;
using BL.DTOs;
using DAL.Repositories.Interfaces;
using MediatR;

namespace BL.Features.Users.Queries.GetAllUsers
{
    internal class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetAllUsersQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var allUsers = await _userRepository.GetAllAsync(request.filter, request.includeProperties, request.isTracked);
            return _mapper.Map<IEnumerable<UserDto>>(allUsers);
        }
    }
}
