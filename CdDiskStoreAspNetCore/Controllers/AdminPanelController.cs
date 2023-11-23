using CdDiskStoreAspNetCore.Data.Repository;
using CdDiskStoreAspNetCore.Models;
using CdDiskStoreAspNetCore.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CdDiskStoreAspNetCore.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminPanelController : Controller
    {
        private readonly IIdentityUserRepository _userRepository;

        public AdminPanelController(IIdentityUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        public async Task<IActionResult> Index(string? filter, MySortOrder sortOrder, string? filterFieldName, string? sortField = "Id", int skip = 0)
        {
            var model = new IndexViewModel<IdentityUser>
            {
                Filter = filter,
                FilterFieldName = filterFieldName ?? IndexViewModel<IdentityUser>.FilterableFieldNames[0],
                SortFieldName = sortField,
                SortOrder = sortOrder,
                Skip = skip,
                CountItems = await this._userRepository.GetProcessedDataCountAsync(filter, filterFieldName)
            };

            model.Items = await this._userRepository.GetProcessedDataAsync(model.Filter, model.FilterFieldName, model.SortOrder, model.SortFieldName, model.Skip, model.PageSize);

            return View(model);
        }

        public async Task<IActionResult> Details(string? id)
        {
            if (id == null || this._userRepository == null)
            {
                return NotFound();
            }

            try
            {
                var model = new AdminPanelDetailsViewModel
                {
                    User = await this._userRepository.GetByIdAsync(id),
                    Roles = await this._userRepository.GetRolesAsync(id),
                };

                return View(model);
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
        }


        public async Task<IActionResult> Create(string? id)
        {
            if (id == null)
            {
                ViewData["Action"] = "Create";

                var model = new AdminPanelCreateViewModel
                {
                    User = new IdentityUser(),
                    Roles = await this._userRepository.GetAllRolesAsync(),
                    Password = "",
                    Email = "",
                    PhoneNumber = ""
                };

                return View(model);
            }

            ViewData["Action"] = "Edit";
            try
            {
                var model = new AdminPanelCreateViewModel
                {
                    User = await this._userRepository.GetByIdAsync(id),
                    Roles = await this._userRepository.GetAllRolesAsync(),
                    Password = ""
                };

                model.PhoneNumber = model.User.PhoneNumber;
                model.Email = model.User.Email;

                return View(model);
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string? id, AdminPanelCreateViewModel model)
        {
            if (!ModelState.IsValid || model.User == null)
            {
                return View(model);
            }

            model.User.PhoneNumber = model.PhoneNumber;
            model.User.Email = model.Email;

            try
            {
                if (await this._userRepository.GetByIdAsync(id) != null)
                {
                    await this._userRepository.UpdateAsync(model.User, model.Roles);
                }
                else
                {
                    await this._userRepository.AddAsync(model.User, model.Password);
                }
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (this._userRepository == null)
            {
                return Problem("Repository class implementing 'IIdentityUserRepository' is null.");
            }

            var user = await this._userRepository.GetByIdAsync(id);

            if (user != null)
            {
                await this._userRepository.DeleteAsync(user.Id);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
