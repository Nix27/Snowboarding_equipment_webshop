using AutoMapper;
using BL.DTOs;
using BL.Services.Interfaces;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.Implementations
{
    public class CountryService : ICountryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CountryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateAsync(CountryDto countryDto)
        {
            await _unitOfWork.Country.CreateAsync(_mapper.Map<Country>(countryDto));
            await _unitOfWork.SaveAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var countryForDelete = await _unitOfWork.Country.GetFirstOrDefaultAsync(c => c.Id == id);

            if(countryForDelete == null) return false;

            _unitOfWork.Country.Delete(countryForDelete);
            await _unitOfWork.SaveAsync();

            return true;
        }

        public async Task<IEnumerable<CountryDto>> GetAllAsync()
        {
            var allCountries = await _unitOfWork.Country.GetAllAsync();
            return _mapper.Map<IEnumerable<CountryDto>>(allCountries);
        }

        public async Task<CountryDto> GetByIdAsync(int id)
        {
            var country = await _unitOfWork.Country.GetFirstOrDefaultAsync(c => c.Id == id);
            return _mapper.Map<CountryDto>(country);
        }

        public async Task<int> GetNumberOfCountriesAsync() => _unitOfWork.Country.GetAllAsync()
                                                                                 .GetAwaiter()
                                                                                 .GetResult()
                                                                                 .Count();

        public async Task<IEnumerable<CountryDto>> GetPagedCountriesAsync(int page, int size, string? searchTerm)
        {
            searchTerm = searchTerm?.ToLower();

            var allCountries = await _unitOfWork.Country.GetAllAsync();

            if(searchTerm != null)
            {
                allCountries = allCountries.Where(c => c.Name.ToLower().Contains(searchTerm));
            }

            var pagedCountries = allCountries.Skip(page * size).Take(size);

            return _mapper.Map<IEnumerable<CountryDto>>(pagedCountries);
        }

        public async Task UpdateAsync(CountryDto countryDto)
        {
            _unitOfWork.Country.Update(_mapper.Map<Country>(countryDto));
            await _unitOfWork.SaveAsync();
        }
    }
}
