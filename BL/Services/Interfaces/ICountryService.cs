using BL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.Interfaces
{
    public interface ICountryService
    {
        Task<IEnumerable<CountryDto>> GetAllAsync();
        Task<CountryDto> GetByIdAsync(int id);
        Task<IEnumerable<CountryDto>> GetPagedCountriesAsync(int page, int size, string? searchTerm);
        Task<int> GetNumberOfCountriesAsync();
        Task CreateAsync(CountryDto countryDto);
        Task UpdateAsync(CountryDto countryDto);
        Task<bool> DeleteAsync(int id);
    }
}
