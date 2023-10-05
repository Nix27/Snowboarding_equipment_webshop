using AutoMapper;
using BL.DTOs;
using BL.Features.Categories.Queries.GetAllCategories;
using BL.Features.Categories.Queries.GetNumberOfCategories;
using BL.Features.Countries.Commands.CreateCountry;
using BL.Features.Countries.Commands.DeleteCountry;
using BL.Features.Countries.Commands.UpdateCountry;
using BL.Features.Countries.Queries.GetAllCountries;
using BL.Features.Countries.Queries.GetCountryById;
using BL.Features.Countries.Queries.GetNumberOfCountries;
using BL.Features.Countries.Queries.GetPagedCountries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Snowboarding_equipment_webshop.ViewModels;

namespace Snowboarding_equipment_webshop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CountryController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<CountryController> _logger;
        private const string errorMessage = "Something went wrong. Try again later!";

        public CountryController(IMediator mediator, IMapper mapper, ILogger<CountryController> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> AllCountries(int page, float size, string? searchTerm)
        {
            if (size == 0) size = 5f;
            if (page == 0) page = 1;

            IEnumerable<CountryDto>? countries = null;
            int numberOfAllCountries;

            try
            {
                if (!String.IsNullOrEmpty(searchTerm))
                {
                    countries = await _mediator.Send(new GetAllCountriesQuery(c => c.Name.Contains(searchTerm)));
                    numberOfAllCountries = await _mediator.Send(new GetNumberOfCountriesQuery(c => c.Name.Contains(searchTerm)));
                    HttpContext.Response.Cookies.Append("countrySearchTerm", searchTerm);
                }
                else
                {
                    HttpContext.Response.Cookies.Delete("countrySearchTerm");
                    countries = await _mediator.Send(new GetAllCountriesQuery());
                    numberOfAllCountries = await _mediator.Send(new GetNumberOfCountriesQuery());
                }

                var pagedCountries = await _mediator.Send(new GetPagedCountriesQuery(countries, page, size));

                ViewData["page"] = page;
                ViewData["size"] = (int)size;
                ViewData["pages"] = (int)Math.Ceiling(numberOfAllCountries / size);
                ViewData["action"] = nameof(AllCountries);

                return View(_mapper.Map<IEnumerable<CountryVM>>(pagedCountries));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace);
                TempData["error"] = errorMessage;
                return StatusCode(500);
            }
        }

        public async Task<IActionResult> CountryTableBodyPartial(int page, float size, string? searchTerm)
        {
            IEnumerable<CountryDto>? countries = null;
            int numberOfAllCountries;

            searchTerm = HttpContext.Request.Cookies["countrySearchTerm"];

            try
            {
                if (!String.IsNullOrEmpty(searchTerm))
                {
                    countries = await _mediator.Send(new GetAllCountriesQuery(c => c.Name.Contains(searchTerm)));
                    numberOfAllCountries = await _mediator.Send(new GetNumberOfCountriesQuery(c => c.Name.Contains(searchTerm)));
                }
                else
                {
                    countries = await _mediator.Send(new GetAllCountriesQuery());
                    numberOfAllCountries = await _mediator.Send(new GetNumberOfCountriesQuery());
                }

                var pagedCountries = await _mediator.Send(new GetPagedCountriesQuery(countries, page, size));

                ViewData["page"] = page;
                ViewData["size"] = size;
                ViewData["pages"] = (int)Math.Ceiling(numberOfAllCountries / size);

                return PartialView("_CountryTableBodyPartial", _mapper.Map<IEnumerable<CountryVM>>(pagedCountries));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace);
                TempData["error"] = errorMessage;
                return StatusCode(500);
            }
        }

        public IActionResult CreateCountry()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCountry(CountryVM newCountry)
        {
            if (!ModelState.IsValid) return View(newCountry);

            try
            {
                await _mediator.Send(new CreateCountryCommand(_mapper.Map<CountryDto>(newCountry)));

                TempData["success"] = "Country added successfully";
                return RedirectToAction(nameof(AllCountries));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace);
                TempData["error"] = errorMessage;
                return StatusCode(500);
            }
        }

        public async Task<IActionResult> UpdateCountry(int id)
        {
            try
            {
                var countryForUpdate = await _mediator.Send(new GetCountryByIdQuery(id));
                return View(_mapper.Map<CountryVM>(countryForUpdate));
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
        public async Task<IActionResult> UpdateCountry(CountryVM countryForUpdate)
        {
            if (!ModelState.IsValid) return View(countryForUpdate);

            try
            {
                await _mediator.Send(new UpdateCountryCommand(_mapper.Map<CountryDto>(countryForUpdate)));

                TempData["success"] = "Country updated successfully";
                return RedirectToAction(nameof(AllCountries));
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
        public async Task<IActionResult> DeleteCountry(int id)
        {
            try
            {
                var deletedCountry = await _mediator.Send(new DeleteCountryCommand(id));

                return Json(new { success = true, message = "Successfully deleted country." });
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
