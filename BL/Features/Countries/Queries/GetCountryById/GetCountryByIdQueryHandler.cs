using AutoMapper;
using BL.DTOs;
using DAL.Repositories.Interfaces;
using MediatR;

namespace BL.Features.Countries.Queries.GetCountryById
{
    internal class GetCountryByIdQueryHandler : IRequestHandler<GetCountryByIdQuery, CountryDto>
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public GetCountryByIdQueryHandler(ICountryRepository countryRepository, IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        public async Task<CountryDto> Handle(GetCountryByIdQuery request, CancellationToken cancellationToken)
        {
            var requestedCountry = await _countryRepository.GetFirstOrDefaultAsync(c => c.Id == request.id, isTracked: request.isTracked);

            if (requestedCountry == null)
                throw new InvalidOperationException("Country not found");

            return _mapper.Map<CountryDto>(requestedCountry);
        }
    }
}
