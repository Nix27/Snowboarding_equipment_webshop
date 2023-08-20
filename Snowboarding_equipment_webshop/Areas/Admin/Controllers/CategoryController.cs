using AutoMapper;
using BL.DTOs;
using BL.Services.Interfaces;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Snowboarding_equipment_webshop.ViewModels;

namespace Snowboarding_equipment_webshop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        public async Task<IActionResult> AllCategories()
        {
            var allCategories = await _categoryService.GetAllAsync();
            return View(_mapper.Map<IEnumerable<CategoryVM>>(allCategories));
        }

        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryVM newCategory)
        {
            await _categoryService.CreateAsync(_mapper.Map<CategoryDto>(newCategory));
            return View(nameof(AllCategories));
        }
    }
}
