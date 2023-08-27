using AutoMapper;
using BL.DTOs;
using BL.Features.Countries.Queries.GetCountryById;
using DAL.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Features.Countries.Queries.GetPagedCountries
{
    public class GetPagedCountriesQueryHandler : IRequestHandler<GetPagedCountriesQuery, IEnumerable<CountryDto>?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetPagedCountriesQuery> _logger;

        public GetPagedCountriesQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<GetPagedCountriesQuery> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<CountryDto>?> Handle(GetPagedCountriesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allCountries = await _unitOfWork.Country.GetAllAsync();

                string? searchTerm = request.searchTerm?.ToLower();

                if(searchTerm != null)
                {
                    allCountries = allCountries.Where(c => c.Name.ToLower().Contains(searchTerm));
                }

                var pagedCountries = allCountries.Skip(request.page * request.size).Take(request.size);

                return _mapper.Map<IEnumerable<CountryDto>>(pagedCountries);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                return null;
            }
        }
    }
}
