using CdDiskStoreAspNetCore.Data;
using CdDiskStoreAspNetCore.Data.Repository;
using CdDiskStoreAspNetCore.Models;
using CdDiskStoreAspNetCore.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CdDiskStoreAspNetCore.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminPanelController : Controller
    {
        /*private readonly ApplicationDbContext _context;
        private readonly IIdentityUserRepository _userRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _userEmailStore;

        public AdminPanelController(
            ApplicationDbContext context,
            IIdentityUserRepository userRepository,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IUserStore<IdentityUser> userStore)
        {
            this._context = context;
            this._userRepository = userRepository;
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._userStore = userStore;

            this._userEmailStore = (IUserEmailStore<IdentityUser>)this._userStore;
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
                    User = await this._context.Users.FindAsync(id),
                    Roles = this._context.Roles.Select(r => r.Name).ToList(),
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
            if (!ModelState.IsValid || model.User == null || this._context.Users == null)
            {
                return View(model);
            }

            if (await this._context.Users.FindAsync(model.User.Id) != null)
            {
                // Редагування існуючого користувача
                var existingUser = await _userManager.FindByIdAsync(model.User.Id);

                if (existingUser == null)
                {
                    return NotFound();
                }

                existingUser.UserName = model.User.UserName;
                existingUser.Email = model.User.Email;

                // Отримати ролі користувача та видалити їх
                var userRoles = await _userManager.GetRolesAsync(existingUser);
                await _userManager.RemoveFromRolesAsync(existingUser, userRoles);

                // Додати нові ролі користувачу
                await _userManager.AddToRolesAsync(existingUser, model.Roles);

                await _userManager.UpdateAsync(existingUser);
            }
            else
            {
                // Створення нового користувача
                var user = Activator.CreateInstance<IdentityUser>();

                await _userStore.SetUserNameAsync(user, model.User.Email, CancellationToken.None);
                await _userEmailStore.SetEmailAsync(user, model.User.Email, CancellationToken.None);

                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    throw new Exception($"Error create user {model.User.Email} with password {model.Password}");
                }

                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var resultConfirm = await _userManager.ConfirmEmailAsync(user, code);
                if (!resultConfirm.Succeeded)
                {
                    throw new Exception("");
                }

                user = await _userManager.FindByNameAsync(model.User.Email);

                if (model.Roles != null && model.Roles.Count > 0)
                {
                    await _userManager.AddToRolesAsync(user, model.Roles);
                }
            }
            return RedirectToAction(nameof(Index));
        }*/

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
                    await this._userRepository.AddAsync(model.User, model.Password, model.Roles);
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
