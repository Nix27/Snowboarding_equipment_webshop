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
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateAsync(CategoryDto newCategory)
        {
             await _unitOfWork.Category.CreateAsync(_mapper.Map<Category>(newCategory));
             await _unitOfWork.SaveAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var categoryForDelete = await _unitOfWork.Category.GetFirstOrDefaultAsync(c => c.Id == id);
            if (categoryForDelete == null) return false;

            _unitOfWork.Category.Delete(categoryForDelete);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var allCategories = await _unitOfWork.Category.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryDto>>(allCategories);
        }

        public async Task<CategoryDto> GetByIdAsync(int id)
        {
            var category = await _unitOfWork.Category.GetFirstOrDefaultAsync(c => c.Id == id);
            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<int> GetNumberOfCategoriesAsync() => _unitOfWork.Category.GetAllAsync()
                                                                              .GetAwaiter()
                                                                              .GetResult()
                                                                              .Count();

        public async Task<IEnumerable<CategoryDto>> GetPagedCategoriesAsync(int page, int size)
        {
            var allCategories = await _unitOfWork.Category.GetAllAsync();
            var pagedCategories = allCategories.Skip(page * size).Take(size);

            return _mapper.Map<IEnumerable<CategoryDto>>(pagedCategories);
        }

        public Task UpdateAsync(CategoryDto updatedCategory)
        {
            _unitOfWork.Category.Update(_mapper.Map<Category>(updatedCategory));
            return _unitOfWork.SaveAsync();
        }
    }
}
