using AutoMapper;
using BL.DTOs;
using BL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Snowboarding_equipment_webshop.ViewModels;

namespace Snowboarding_equipment_webshop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CountryController : Controller
    {
        private readonly ICountryService _countryService;
        private readonly IMapper _mapper;
        private readonly ILogger<CountryVM> _logger;
        private const string errorMessage = "Something went wrong. Try again later!";

        public CountryController(ICountryService countryService, IMapper mapper, ILogger<CountryVM> logger)
        {
            _countryService = countryService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> AllCountries(int page, int size, string? searchTerm)
        {
            try
            {
                if (size == 0)
                    size = 5;

                var pagedCountries = await _countryService.GetPagedCountriesAsync(page, size, searchTerm);
                var numberOfAllCountries = await _countryService.GetNumberOfCountriesAsync();

                ViewData["page"] = page;
                ViewData["size"] = size;
                ViewData["pages"] = (int)Math.Ceiling((double)numberOfAllCountries / size);

                return View(_mapper.Map<IEnumerable<CountryVM>>(pagedCountries));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                TempData["error"] = errorMessage;
                return StatusCode(500);
            }
        }

        public async Task<IActionResult> CountryTableBodyPartial(int page, int size, string? searchTerm)
        {
            try
            {
                if (size == 0)
                    size = 5;

                var pagedCountries = await _countryService.GetPagedCountriesAsync(page, size, searchTerm);
                var numberOfAllCountries = await _countryService.GetNumberOfCountriesAsync();

                ViewData["page"] = page;
                ViewData["size"] = size;
                ViewData["pages"] = (int)Math.Ceiling((double)numberOfAllCountries / size);

                return PartialView("_CountryTableBodyPartial", _mapper.Map<IEnumerable<CountryVM>>(pagedCountries));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
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
                await _countryService.CreateAsync(_mapper.Map<CountryDto>(newCountry));
                TempData["success"] = "Country added successfully";
                return RedirectToAction(nameof(AllCountries));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                TempData["error"] = errorMessage;
                return StatusCode(500);
            }
        }

        public async Task<IActionResult> UpdateCountry(int id)
        {
            try
            {
                var countryForUpdate = await _countryService.GetByIdAsync(id);
                return View(_mapper.Map<CountryVM>(countryForUpdate));
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
        public async Task<IActionResult> UpdateCountry(CountryVM updatedCountry)
        {
            if (!ModelState.IsValid) return View(updatedCountry);

            try
            {
                await _countryService.UpdateAsync(_mapper.Map<CountryDto>(updatedCountry));
                TempData["success"] = "Country updated successfully";
                return RedirectToAction(nameof(AllCountries));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
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
                var isDeleted = await _countryService.DeleteAsync(id);

                if (!isDeleted)
                    return Json(new { success = false, message = "Country not found." });

                return Json(new { success = true, message = "Successfully deleted country." });
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
