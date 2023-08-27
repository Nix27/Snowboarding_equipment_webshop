using AutoMapper;
using BL.DTOs;
using BL.Features.Countries.Commands.CreateCountry;
using DAL.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Features.Countries.Queries.GetAllCountries
{
    public class GetAllCountriesQueryHandler : IRequestHandler<GetAllCountriesQuery, IEnumerable<CountryDto>?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllCountriesQuery> _logger;

        public GetAllCountriesQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<GetAllCountriesQuery> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<CountryDto>?> Handle(GetAllCountriesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allCountries = await _unitOfWork.Country.GetAllAsync();
                return _mapper.Map<IEnumerable<CountryDto>>(allCountries);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                return null;
            }
        }
    }
}
