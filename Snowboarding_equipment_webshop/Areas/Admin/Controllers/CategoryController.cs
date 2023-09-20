using AutoMapper;
using BL.DTOs;
using BL.Features.Categories.Commands.CreateCategory;
using BL.Features.Categories.Commands.DeleteCategory;
using BL.Features.Categories.Commands.UpdateCategory;
using BL.Features.Categories.Queries.GetAllCategories;
using BL.Features.Categories.Queries.GetCategoryById;
using BL.Features.Categories.Queries.GetNumberOfCategories;
using BL.Features.Categories.Queries.GetPagedCategories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Snowboarding_equipment_webshop.ViewModels;

namespace Snowboarding_equipment_webshop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogger<CategoryController> _logger;

        private const string errorMessage = "Something went wrong. Try again later!";

        public CategoryController(IMediator mediator, IMapper mapper, ILogger<CategoryController> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> AllCategories(int page, float size, string? searchTerm)
        {
            if (size == 0) size = 5f;
            if(page == 0) page = 1;

            IEnumerable<CategoryDto>? categories = null;
            int numberOfAllCategories;

            try
            {
                if(searchTerm != null)
                {
                    categories = await _mediator.Send(new GetAllCategoriesQuery(c => c.Name.Contains(searchTerm)));
                    numberOfAllCategories = await _mediator.Send(new GetNumberOfCategoriesQuery(c => c.Name.Contains(searchTerm)));
                }
                else
                {
                    numberOfAllCategories = await _mediator.Send(new GetNumberOfCategoriesQuery());
                }

                var pagedCategories = await _mediator.Send(new GetPagedCategoriesQuery(categories, page, size));
                
                ViewData["page"] = page;
                ViewData["size"] = (int)size;
                ViewData["pages"] = (int)Math.Ceiling(numberOfAllCategories / size);

                return View(_mapper.Map<IEnumerable<CategoryVM>>(pagedCategories));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace);
                TempData["error"] = errorMessage;
                return StatusCode(500);
            }
        }

        public async Task<IActionResult> CategoryTableBodyPartial(int page, float size, string? searchTerm)
        {
            IEnumerable<CategoryDto>? categories = null;
            int numberOfAllCategories;

            try
            {
                if (searchTerm != null)
                {
                    categories = await _mediator.Send(new GetAllCategoriesQuery(c => c.Name.Contains(searchTerm)));
                    numberOfAllCategories = await _mediator.Send(new GetNumberOfCategoriesQuery(c => c.Name.Contains(searchTerm)));
                }
                else
                {
                    numberOfAllCategories = await _mediator.Send(new GetNumberOfCategoriesQuery());
                }

                var pagedCategories = await _mediator.Send(new GetPagedCategoriesQuery(categories, page, size));

                ViewData["page"] = page;
                ViewData["size"] = (int)size;
                ViewData["pages"] = (int)Math.Ceiling(numberOfAllCategories / size);

                return PartialView("_CategoryTableBodyPartial", _mapper.Map<IEnumerable<CategoryVM>>(pagedCategories));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace);
                TempData["error"] = errorMessage;
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
                await _mediator.Send(new CreateCategoryCommand(_mapper.Map<CategoryDto>(newCategory)));

                TempData["success"] = "Category added successfully";
                return RedirectToAction(nameof(AllCategories));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace);
                TempData["error"] = errorMessage;
                return StatusCode(500);
            }
        }

        public async Task<IActionResult> UpdateCategory(int id)
        {
            try
            {
                var categoryForUpdate = await _mediator.Send(new GetCategoryByIdQuery(id));
                return View(_mapper.Map<CategoryVM>(categoryForUpdate));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace);
                TempData["error"] = errorMessage;
                return StatusCode(500);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCategory(CategoryVM categoryForUpdate)
        {
            if(!ModelState.IsValid) return View(categoryForUpdate);

            try
            {
                await _mediator.Send(new UpdateCategoryCommand(_mapper.Map<CategoryDto>(categoryForUpdate)));

                TempData["success"] = "Category updated successfully";
                return RedirectToAction(nameof(AllCategories));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace);
                TempData["error"] = errorMessage;
                return StatusCode(500);
            }
        }

        #region API Calls
        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                await _mediator.Send(new DeleteCategoryCommand(id));

                return Json(new { success = true, message = "Successfully deleted category" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace);
                return Json(new { success = false, message = errorMessage });
            }
        }
        #endregion
    }
}
