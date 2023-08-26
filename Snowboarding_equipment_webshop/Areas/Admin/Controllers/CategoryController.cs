using AutoMapper;
using BL.DTOs;
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
        private const string errorMessage = "Something went wrong. Try again later!";

        public CategoryController(ICategoryService categoryService, IMapper mapper, ILogger<CategoryVM> logger)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> AllCategories(int page, int size, string? searchTerm)
        {
            try
            {
                if (size == 0)
                    size = 5;

                var pagedCategories = await _categoryService.GetPagedCategoriesAsync(page, size, searchTerm);
                var numberOfCategories = await _categoryService.GetNumberOfCategoriesAsync();

                ViewData["page"] = page;
                ViewData["size"] = size;
                ViewData["pages"] = (int)Math.Ceiling((double)numberOfCategories / size);

                return View(_mapper.Map<IEnumerable<CategoryVM>>(pagedCategories));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                TempData["error"] = "Unable to get categories. Please, try again later";
                return StatusCode(500);
            }
        }

        public async Task<IActionResult> CategoryTableBodyPartial(int page, int size, string? searchTerm)
        {
            try
            {
                if (size == 0)
                    size = 5;

                var pagedCategories = await _categoryService.GetPagedCategoriesAsync(page, size, searchTerm);
                var numberOfCategories = await _categoryService.GetNumberOfCategoriesAsync();

                ViewData["page"] = page;
                ViewData["size"] = size;
                ViewData["pages"] = (int)Math.Ceiling((double)numberOfCategories / size);

                return PartialView("_CategoryTableBodyPartial", _mapper.Map<IEnumerable<CategoryVM>>(pagedCategories));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                TempData["error"] = "Unable to get categories. Please, try again later.";
                return StatusCode(500);
            }
        }

        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(CategoryVM newCategory)
        {
            if(!ModelState.IsValid) return View(newCategory);

            try
            {
                await _categoryService.CreateAsync(_mapper.Map<CategoryDto>(newCategory)).ConfigureAwait(false);
                TempData["success"] = "Category added successfully";
                return RedirectToAction(nameof(AllCategories));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                TempData["error"] = errorMessage;
                return View(nameof(AllCategories));
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
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                TempData["error"] = errorMessage;
                return StatusCode(500);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCategory(CategoryVM updatedCategory)
        {
            if(!ModelState.IsValid) return View(updatedCategory);

            try
            {
                await _categoryService.UpdateAsync(_mapper.Map<CategoryDto>(updatedCategory)).ConfigureAwait(false);
                TempData["success"] = "Category updated successfully";
                return RedirectToAction(nameof(AllCategories));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                TempData["error"] = errorMessage;
                return View(nameof(AllCategories));
            }
        }

        #region API Calls
        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var isDeleted = await _categoryService.DeleteAsync(id);

                if (!isDeleted)
                    return Json(new { success = false, message = "Category not found" });

                return Json(new { success = true, message = "Successfully deleted category" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                TempData["error"] = errorMessage;
                return StatusCode(500);
            }
        }
        #endregion
    }
}
