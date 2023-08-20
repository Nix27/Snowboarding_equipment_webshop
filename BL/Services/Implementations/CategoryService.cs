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

        public Task CreateAsync(CategoryDto categoryDto)
        {
            _unitOfWork.Category.Create(_mapper.Map<Category>(categoryDto));
            return _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var allCategories = await _unitOfWork.Category.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryDto>>(allCategories);
        }
    }
}
