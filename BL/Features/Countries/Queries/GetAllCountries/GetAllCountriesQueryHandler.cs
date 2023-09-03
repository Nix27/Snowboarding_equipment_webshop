using AutoMapper;
using BL.DTOs;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.Countries.Queries.GetAllCountries
{
    public class GetAllCountriesQueryHandler : IRequestHandler<GetAllCountriesQuery, IEnumerable<CountryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllCountriesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CountryDto>> Handle(GetAllCountriesQuery request, CancellationToken cancellationToken)
        {
            var allCountries = await _unitOfWork.Country.GetAllAsync();
            return _mapper.Map<IEnumerable<CountryDto>>(allCountries);
        }
    }
}
