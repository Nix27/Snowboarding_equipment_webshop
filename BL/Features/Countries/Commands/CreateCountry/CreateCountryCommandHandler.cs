using AutoMapper;
using DAL.Models;
using DAL.Repositories.Interfaces;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.Countries.Commands.CreateCountry
{
    internal class CreateCountryCommandHandler : IRequestHandler<CreateCountryCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICountryRepository _countryRepository;

        public CreateCountryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICountryRepository countryRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _countryRepository = countryRepository;
        }

        public async Task<int> Handle(CreateCountryCommand request, CancellationToken cancellationToken)
        {
            var newCountry = _mapper.Map<Country>(request.newCountry);

            await _countryRepository.CreateAsync(newCountry);
            await _unitOfWork.SaveAsync();

            return newCountry.Id;
        }
    }
}
