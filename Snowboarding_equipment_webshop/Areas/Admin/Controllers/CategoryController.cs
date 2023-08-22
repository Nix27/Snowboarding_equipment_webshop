using AutoMapper;
using BL.DTOs;
using BL.Exceptions;
using BL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Snowboarding_equipment_webshop.ViewModels;

namespace Snowboarding_equipment_webshop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryVM> _logger;

        public CategoryController(ICategoryService categoryService, IMapper mapper, ILogger<CategoryVM> logger)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _logger = logger;
        }

        public IActionResult AllCategories()
        {
            return View();
        }

        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryVM newCategory)
        {
            if(!ModelState.IsValid) return View(newCategory);

            try
            {
                await _categoryService.CreateAsync(_mapper.Map<CategoryDto>(newCategory));
                return View(nameof(AllCategories));
            }
            catch (DbCommandException ex)
            {
                _logger.LogError(ex.Message);
                return RedirectToAction("Error", "Home", new { area = "Customer" });
            }
        }

        public async Task<IActionResult> UpdateCategory(int id)
        {
            if(id == 0) return RedirectToAction("Error", "Home", new { area = "Customer" });

            try
            {
                var requestedCategory = await _categoryService.GetByIdAsync(id);
                return View(_mapper.Map<CategoryVM>(requestedCategory));
            }
            catch (DbQueryException ex)
            {
                _logger.LogError(ex.Message);
                return RedirectToAction("Error", "Home", new { area = "Customer" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCategory(CategoryVM updatedCategory)
        {
            if(!ModelState.IsValid) return View(updatedCategory);

            try
            {
                await _categoryService.UpdateAsync(_mapper.Map<CategoryDto>(updatedCategory));
                return View(nameof(AllCategories));
            }
            catch (DbCommandException ex)
            {
                _logger.LogError(ex.Message);
                return RedirectToAction("Error", "Home", new { area = "Customer" });
            }
        }

        #region API Calls
        [HttpGet]
        public async Task<IActionResult> GetAllCategoriesAsync()
        {
            try
            {
                var allCategories = await _categoryService.GetAllAsync();
                return Json(new { data = allCategories });
            }
            catch (DbQueryException ex)
            {
                _logger.LogError(ex.Message);
                return RedirectToAction("Error", "Home", new { area = "Customer" });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var isDeleted = await _categoryService.DeleteAsync(id);

                if(!isDeleted)
                    return Json(new { success = false, message = "Category not found" });

                return Json(new { success = true, message = "Successfully deleted category" });
            }
            catch (Exception ex) when (ex is DbQueryException || ex is DbCommandException)
            {
                _logger.LogError(ex.Message);
                return RedirectToAction("Error", "Home", new { area = "Customer" });
            }
        }
        #endregion
    }
}
