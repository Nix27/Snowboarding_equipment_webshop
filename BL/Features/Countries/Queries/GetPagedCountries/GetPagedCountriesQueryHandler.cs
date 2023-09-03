using AutoMapper;
using BL.DTOs;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.Countries.Queries.GetPagedCountries
{
    public class GetPagedCountriesQueryHandler : IRequestHandler<GetPagedCountriesQuery, IEnumerable<CountryDto>?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPagedCountriesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CountryDto>?> Handle(GetPagedCountriesQuery request, CancellationToken cancellationToken)
        {
            var allCountries = await _unitOfWork.Country.GetAllAsync();

            string? searchTerm = request.searchTerm?.ToLower();

            if (searchTerm != null)
            {
                allCountries = allCountries.Where(c => c.Name.ToLower().Contains(searchTerm));
            }

            var pagedCountries = allCountries.Skip(request.page * request.size).Take(request.size);

            return _mapper.Map<IEnumerable<CountryDto>>(pagedCountries);
        }
    }
}
