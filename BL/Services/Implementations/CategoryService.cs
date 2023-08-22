using AutoMapper;
using BL.DTOs;
using BL.Exceptions;
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
            try
            {
                await _unitOfWork.Category.CreateAsync(_mapper.Map<Category>(newCategory));
                await _unitOfWork.SaveAsync();
            }
            catch
            {
                throw new DbCommandException();
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var categoryForDelete = await _unitOfWork.Category.GetFirstOrDefaultAsync(c => c.Id == id);
                if (categoryForDelete == null) return false;

                _unitOfWork.Category.Delete(categoryForDelete);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch
            {
                throw new DbCommandException();
            }
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            try
            {
                var allCategories = await _unitOfWork.Category.GetAllAsync();
                return _mapper.Map<IEnumerable<CategoryDto>>(allCategories);
            }
            catch
            {
                throw new DbQueryException();
            }
        }

        public async Task<CategoryDto> GetByIdAsync(int id)
        {
            try
            {
                var category = await _unitOfWork.Category.GetFirstOrDefaultAsync(c => c.Id == id);
                return _mapper.Map<CategoryDto>(category);
            }
            catch
            {
                throw new DbQueryException();
            }
        }

        public Task UpdateAsync(CategoryDto updatedCategory)
        {
            try
            {
                _unitOfWork.Category.Update(_mapper.Map<Category>(updatedCategory));
                return _unitOfWork.SaveAsync();
            }
            catch
            {
                throw new DbCommandException();
            }
        }
    }
}
