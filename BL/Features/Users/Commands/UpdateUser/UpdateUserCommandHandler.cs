using AutoMapper;
using DAL.Models;
using DAL.Repositories.Interfaces;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.Users.Commands.UpdateUser
{
    internal class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserCommandHandler(IUserRepository userRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var userForUpdate = _mapper.Map<User>(request.userForUpdate);
            await _userRepository.UpdateUserAsync(userForUpdate);
            await _unitOfWork.SaveAsync();
        }
    }
}
