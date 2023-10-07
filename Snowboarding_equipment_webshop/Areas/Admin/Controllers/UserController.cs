using AutoMapper;
using BL.DTOs;
using BL.Features.Countries.Queries.GetAllCountries;
using BL.Features.Users.Commands.UpdateUser;
using BL.Features.Users.Queries.GetPagedUsers;
using BL.Features.Users.Queries.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Snowboarding_equipment_webshop.ViewModels;
using Utilities.Constants.Role;

namespace Snowboarding_equipment_webshop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = AppRoles.ADMIN)]
    public class UserController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;

        private const string errorMessage = "Something went wrong. Try again later!";

        public UserController(IMediator mediator, IMapper mapper, ILogger<UserController> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> AllUsers(int page, float size, string? searchBy, string? searchTerm)
        {
            if (page == 0) page = 1;
            if (size == 0) size = 5f;
            if (searchBy == null) searchBy = "name";

            try
            {
                if (!String.IsNullOrEmpty(searchTerm))
                {
                    HttpContext.Response.Cookies.Append("usersSearchTerm",searchTerm);
                    HttpContext.Response.Cookies.Append("usersSearchBy", searchBy);
                }
                else
                {
                    HttpContext.Response.Cookies.Delete("usersSearchTerm");
                }

                (var pagedUsers, int numberOfUsers) = await _mediator.Send(new GetPagedUsersQuery(page, size, searchBy, searchTerm));

                ViewData["page"] = page;
                ViewData["size"] = (int)size;
                ViewData["pages"] = (int)Math.Ceiling(numberOfUsers / size);
                ViewData["searchBy"] = searchBy;

                var pagedUsersVm = _mapper.Map<IEnumerable<UserVM>>(pagedUsers);

                return View(pagedUsersVm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace);
                TempData["error"] = errorMessage;
                return StatusCode(500);
            }
        }

        public async Task<IActionResult> UserTableBodyPartial(int page, float size, string? searchBy, string? searchTerm)
        {
            searchTerm = HttpContext.Request.Cookies["usersSearchTerm"];
            searchBy = HttpContext.Request.Cookies["usersSearchBy"];

            try
            {
                (var pagedUsers, int numberOfUsers) = await _mediator.Send(new GetPagedUsersQuery(page, size, searchBy, searchTerm));

                ViewData["page"] = page;
                ViewData["size"] = (int)size;
                ViewData["pages"] = (int)Math.Ceiling(numberOfUsers / size);
                ViewData["searchBy"] = searchBy;

                var pagedUsersVm = _mapper.Map<IEnumerable<UserVM>>(pagedUsers);

                return PartialView("_UserTableBodyPartial", pagedUsersVm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace);
                TempData["error"] = errorMessage;
                return StatusCode(500);
            }
        }

        public async Task<IActionResult> UpdateUser(string id)
        {
            try
            {
                var userForUpdate = await _mediator.Send(new GetUserByIdQuery(id));
                var allCountries = await _mediator.Send(new GetAllCountriesQuery());

                var countryItems = allCountries.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                });

                ViewData["countries"] = countryItems;

                return View(_mapper.Map<UserVM>(userForUpdate));
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
        public async Task<IActionResult> UpdateUser(UserVM userForUpdateVM)
        {
            if(!ModelState.IsValid) return View(userForUpdateVM);

            try
            {
                await _mediator.Send(new UpdateUserCommand(_mapper.Map<UserDto>(userForUpdateVM)));
                TempData["success"] = "User successfully updated";
                return RedirectToAction(nameof(AllUsers));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace);
                TempData["error"] = errorMessage;
                return RedirectToAction(nameof(AllUsers));
            }
        }
    }
}
