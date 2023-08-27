using AutoMapper;
using BL.DTOs;
using BL.Features.Countries.Commands.CreateCountry;
using BL.Features.Countries.Commands.DeleteCountry;
using BL.Features.Countries.Commands.UpdateCountry;
using BL.Features.Countries.Queries.GetAllCountries;
using BL.Features.Countries.Queries.GetCountryById;
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
        private const string errorMessage = "Something went wrong. Try again later!";

        public CountryController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<IActionResult> AllCountries(int page, int size)
        {
            if (size == 0)
                size = 5;

            var pagedCountries = await _mediator.Send(new GetPagedCountriesQuery(page, size));
            int? numberOfAllCountries = _mediator.Send(new GetAllCountriesQuery()).GetAwaiter().GetResult()?.Count();

            if(pagedCountries != null && numberOfAllCountries != null)
            {
                ViewData["page"] = page;
                ViewData["size"] = size;
                ViewData["pages"] = (int)Math.Ceiling((double)numberOfAllCountries / size);

                return View(_mapper.Map<IEnumerable<CountryVM>>(pagedCountries));
            }

            TempData["error"] = errorMessage;
            return StatusCode(500);
        }

        public async Task<IActionResult> CountryTableBodyPartial(int page, int size, string? searchTerm)
        {
            if (size == 0)
                size = 5;

            var pagedCountries = await _mediator.Send(new GetPagedCountriesQuery(page, size, searchTerm));
            int? numberOfAllCountries;

            if (searchTerm != null)
            {
                numberOfAllCountries = _mediator.Send(new GetAllCountriesQuery())
                                                 .GetAwaiter()
                                                 .GetResult()?
                                                 .Where(c => c.Name.ToLower().Contains(searchTerm))
                                                 .Count();
            }
            else
            {
                numberOfAllCountries = _mediator.Send(new GetAllCountriesQuery())
                                                 .GetAwaiter()
                                                 .GetResult()?
                                                 .Count();
            }

            if (pagedCountries != null && numberOfAllCountries != null)
            {
                ViewData["page"] = page;
                ViewData["size"] = size;
                ViewData["pages"] = (int)Math.Ceiling((double)numberOfAllCountries / size);
                ViewData["action"] = nameof(AllCountries);

                return PartialView("_CountryTableBodyPartial", _mapper.Map<IEnumerable<CountryVM>>(pagedCountries));
            }

            TempData["error"] = errorMessage;
            return StatusCode(500);
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

            var createdCountry = await _mediator.Send(new CreateCountryCommand(_mapper.Map<CountryDto>(newCountry)));

            if(createdCountry != null)
            {
                TempData["success"] = "Country added successfully";
                return RedirectToAction(nameof(AllCountries));
            }

            TempData["error"] = errorMessage;
            return StatusCode(500);
        }

        public async Task<IActionResult> UpdateCountry(int id)
        {
            var countryForUpdate = await _mediator.Send(new GetCountryByIdQuery(id));

            if(countryForUpdate != null)
                return View(_mapper.Map<CountryVM>(countryForUpdate));

            TempData["error"] = errorMessage;
            return StatusCode(500);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCountry(CountryVM countryForUpdate)
        {
            if (!ModelState.IsValid) return View(countryForUpdate);

            var updatedCountry = await _mediator.Send(new UpdateCountryCommand(_mapper.Map<CountryDto>(countryForUpdate)));

            if(updatedCountry != null)
            {
                TempData["success"] = "Country updated successfully";
                return RedirectToAction(nameof(AllCountries));
            }

            TempData["error"] = errorMessage;
            return StatusCode(500);
        }

        #region API Calls
        [HttpDelete]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var countryForDelete = await _mediator.Send(new GetCountryByIdQuery(id, false));

            if(countryForDelete == null)
                return Json(new { success = false, message = "Country not found." });

            var deletedCountry = await _mediator.Send(new DeleteCountryCommand(countryForDelete));

            if(deletedCountry != null)
                return Json(new { success = true, message = "Successfully deleted country." });

            TempData["error"] = errorMessage;
            return StatusCode(500);
        }
        #endregion
    }
}
