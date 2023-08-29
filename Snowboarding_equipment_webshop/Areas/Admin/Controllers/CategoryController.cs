using AutoMapper;
using BL.DTOs;
using BL.Features.Categories.Commands.CreateCategory;
using BL.Features.Categories.Commands.DeleteCategory;
using BL.Features.Categories.Commands.UpdateCategory;
using BL.Features.Categories.Queries.GetAllCategories;
using BL.Features.Categories.Queries.GetCategoryById;
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
        private const string errorMessage = "Something went wrong. Try again later!";

        public CategoryController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<IActionResult> AllCategories(int page, int size, string? searchTerm)
        {
            if (size == 0)
                size = 5;

            var pagedCategories = await _mediator.Send(new GetPagedCategoriesQuery(page, size, searchTerm));
            int? numberOfAllCategories = _mediator.Send(new GetAllCategoriesQuery()).GetAwaiter().GetResult()?.Count();

            if (pagedCategories != null && numberOfAllCategories != null)
            {
                ViewData["page"] = page;
                ViewData["size"] = size;
                ViewData["pages"] = (int)Math.Ceiling((double)numberOfAllCategories / size);

                return View(_mapper.Map<IEnumerable<CategoryVM>>(pagedCategories));
            }

            TempData["error"] = errorMessage;
            return StatusCode(500);
        }

        public async Task<IActionResult> CategoryTableBodyPartial(int page, int size, string? searchTerm)
        {
            if (size == 0)
                size = 5;

            var pagedCategories = await _mediator.Send(new GetPagedCategoriesQuery(page, size, searchTerm));
            int? numberOfAllCategories = _mediator.Send(new GetAllCategoriesQuery()).GetAwaiter().GetResult()?.Count();

            if (pagedCategories != null && numberOfAllCategories != null)
            {
                ViewData["page"] = page;
                ViewData["size"] = size;
                ViewData["pages"] = (int)Math.Ceiling((double)numberOfAllCategories / size);
                ViewData["action"] = nameof(AllCategories);

                return PartialView("_CategoryTableBodyPartial", _mapper.Map<IEnumerable<CategoryVM>>(pagedCategories));
            }

            TempData["error"] = errorMessage;
            return StatusCode(500);
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

            var createdCategory = await _mediator.Send(new CreateCategoryCommand(_mapper.Map<CategoryDto>(newCategory)));

            if(createdCategory != null)
            {
                TempData["success"] = "Category added successfully";
                return RedirectToAction(nameof(AllCategories));
            }

            TempData["error"] = errorMessage;
            return View(nameof(AllCategories));
        }

        public async Task<IActionResult> UpdateCategory(int id)
        {
            var categoryForUpdate = await _mediator.Send(new GetCategoryByIdQuery(id));

            if(categoryForUpdate != null)
                return View(_mapper.Map<CategoryVM>(categoryForUpdate));

            TempData["error"] = errorMessage;
            return StatusCode(500);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCategory(CategoryVM categoryForUpdate)
        {
            if(!ModelState.IsValid) return View(categoryForUpdate);

            var updatedCategory = await _mediator.Send(new UpdateCategoryCommand(_mapper.Map<CategoryDto>(categoryForUpdate)));

            if(updatedCategory != null)
            {
                TempData["success"] = "Category updated successfully";
                return RedirectToAction(nameof(AllCategories));
            }

            TempData["error"] = errorMessage;
            return View(nameof(AllCategories));
        }

        #region API Calls
        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var categoryForDelete = await _mediator.Send(new GetCategoryByIdQuery(id, false));

            if(categoryForDelete == null)
                return Json(new { success = false, message = "Category not found" });

            var deletedCategory = await _mediator.Send(new DeleteCategoryCommand(categoryForDelete));

            if(deletedCategory != null)
                return Json(new { success = true, message = "Successfully deleted category" });

            TempData["error"] = errorMessage;
            return StatusCode(500);
        }
        #endregion
    }
}
