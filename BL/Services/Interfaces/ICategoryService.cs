using BL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<CategoryDto> GetByIdAsync(int id);
        Task<IEnumerable<CategoryDto>> GetPagedCategoriesAsync(int page, int size, string? searchTerm);
        Task<int> GetNumberOfCategoriesAsync();
        Task CreateAsync(CategoryDto newCategory);
        Task UpdateAsync(CategoryDto updatedCategory);
        Task<bool> DeleteAsync(int id);
    }
}
