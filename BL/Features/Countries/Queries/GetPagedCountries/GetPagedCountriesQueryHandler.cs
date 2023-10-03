using AutoMapper;
using BL.DTOs;
using BL.Features.Countries.Queries.GetAllCountries;
using MediatR;

namespace BL.Features.Countries.Queries.GetPagedCountries
{
    internal class GetPagedCountriesQueryHandler : IRequestHandler<GetPagedCountriesQuery, IEnumerable<CountryDto>>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public GetPagedCountriesQueryHandler(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public Task<IEnumerable<CountryDto>> Handle(GetPagedCountriesQuery request, CancellationToken cancellationToken)
        {
            var pagedCountries = request.countries.Skip((request.page - 1) * (int)request.size).Take((int)request.size);

            return Task.FromResult(pagedCountries);
        }
    }
}
