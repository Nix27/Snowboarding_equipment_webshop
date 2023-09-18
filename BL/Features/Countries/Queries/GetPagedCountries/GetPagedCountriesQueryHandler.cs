using AutoMapper;
using BL.DTOs;
using BL.Features.Countries.Queries.GetAllCountries;
using MediatR;

namespace BL.Features.Countries.Queries.GetPagedCountries
{
    internal class GetPagedCountriesQueryHandler : IRequestHandler<GetPagedCountriesQuery, IEnumerable<CountryDto>?>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public GetPagedCountriesQueryHandler(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CountryDto>?> Handle(GetPagedCountriesQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<CountryDto>? countries = request.countries;

            if(countries == null)
            {
                var countriesFromDb = await _mediator.Send(new GetAllCountriesQuery());
                countries = _mapper.Map<IEnumerable<CountryDto>>(countriesFromDb);
            }

            var pagedCountries = countries.Skip(request.page * request.size).Take(request.size);

            return pagedCountries;
        }
    }
}
