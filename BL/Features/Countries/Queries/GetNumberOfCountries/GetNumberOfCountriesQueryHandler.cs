using BL.Features.Countries.Queries.GetAllCountries;
using MediatR;

namespace BL.Features.Countries.Queries.GetNumberOfCountries
{
    internal class GetNumberOfCountriesQueryHandler : IRequestHandler<GetNumberOfCountriesQuery, int>
    {
        private readonly IMediator _mediator;

        public GetNumberOfCountriesQueryHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<int> Handle(GetNumberOfCountriesQuery request, CancellationToken cancellationToken)
        {
            return _mediator.Send(new GetAllCountriesQuery(request.filter, isTracked:false))
                            .GetAwaiter()
                            .GetResult()
                            .Count();
        }
    }
}
