using AutoMapper;
using DAL.Models;
using DAL.Repositories.Interfaces;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.Countries.Commands.UpdateCountry
{
    internal class UpdateCountryCommandHandler : IRequestHandler<UpdateCountryCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICountryRepository _countryRepository;

        public UpdateCountryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICountryRepository countryRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _countryRepository = countryRepository;
        }

        public async Task<int> Handle(UpdateCountryCommand request, CancellationToken cancellationToken)
        {
            var countryForUpdate = _mapper.Map<Country>(request.countryForUpdate);

            _countryRepository.Update(countryForUpdate);
            await _unitOfWork.SaveAsync();

            return countryForUpdate.Id;
        }
    }
}
