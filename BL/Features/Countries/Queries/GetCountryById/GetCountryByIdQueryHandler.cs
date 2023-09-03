using AutoMapper;
using BL.DTOs;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.Countries.Queries.GetCountryById
{
    public class GetCountryByIdQueryHandler : IRequestHandler<GetCountryByIdQuery, CountryDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCountryByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CountryDto?> Handle(GetCountryByIdQuery request, CancellationToken cancellationToken)
        {
            var requestedCountry = await _unitOfWork.Country.GetFirstOrDefaultAsync(c => c.Id == request.id, isTracked: request.isTracked);
            return _mapper.Map<CountryDto>(requestedCountry);
        }
    }
}
