using CdDiskStoreAspNetCore.Data.Models;
using CdDiskStoreAspNetCore.Models;
using CdDiskStoreAspNetCore.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CdDiskStoreAspNetCore.Data.Repository
{
    public class IdentityUserRepository : IIdentityUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _userEmailStore;

        public IdentityUserRepository(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IUserStore<IdentityUser> userStore,
            IUserEmailStore<IdentityUser> userEmailStore)
        {
            this._context = context;
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._userStore = userStore;
            this._userEmailStore = userEmailStore;
        }

        public async Task<IdentityUser> GetByIdAsync(string? id)
        {
            return await this._userManager.FindByIdAsync(id);
        }

        public async Task<IReadOnlyList<IdentityUser>> GetAllAsync()
        {
            return await this._context.Users.ToListAsync();
        }

        public async Task<bool> AddAsync(IdentityUser entity, string password, IReadOnlyList<string> roles)
        {
            await this._userStore.SetUserNameAsync(entity, entity.Email, CancellationToken.None);
            await this._userEmailStore.SetEmailAsync(entity, entity.Email, CancellationToken.None);

            var result = await _userManager.CreateAsync(entity, password);
            if (!result.Succeeded)
            {
                throw new Exception($"Error create user {entity.Email} with password {password}\nErrors:\n{string.Join('\n', result.Errors.Select(e => e.Description))}");
            }

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(entity);
            var resultConfirm = await _userManager.ConfirmEmailAsync(entity, code);
            if (!resultConfirm.Succeeded)
            {
                throw new Exception("Couldn't confirm users email");
            }

            entity = await _userManager.FindByNameAsync(entity.UserName);

            if (roles != null && roles.Count > 0)
            {
                await _userManager.AddToRolesAsync(entity, roles);
            }

            return result.Succeeded;
        }

        public async Task<bool> UpdateAsync(IdentityUser entity, IReadOnlyList<string> roles)
        {
            IdentityUser currentUser;
            try
            {
                currentUser = await this.GetByIdAsync(entity.Id);
            }
            catch (NullReferenceException)
            {
                throw;
            }

            if (currentUser == null)
            {
                return true;
            }

            currentUser.UserName = entity.UserName.ToLower();
            currentUser.Email = entity.Email;
            currentUser.PhoneNumber = entity.PhoneNumber;

            var userRoles = await _userManager.GetRolesAsync(currentUser);
            await _userManager.RemoveFromRolesAsync(currentUser, userRoles);

            await _userManager.AddToRolesAsync(currentUser, roles);

            var result = await _userManager.UpdateAsync(currentUser);

            return result.Succeeded;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var user = await this._context.Users.FindAsync(id);

            if (user != null)
            {
                this._context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> ExistsAsync(string id)
        {
            return await this.GetByIdAsync(id) != null;
        }

        public async Task<IReadOnlyList<IdentityUser>> GetProcessedDataAsync(string? filter, string? filterField, MySortOrder sortOrder, string? sortField, int skip, int pageSize)
        {
            if (filterField == null || !IndexViewModel<IdentityUser>.FilterableFieldNames.Contains(filterField))
            {
                throw new ArgumentOutOfRangeException(nameof(filterField), "Failed to get filter condition. AspNetUsers table does not have such filterable column");
            }

            if (sortField == null || !IndexViewModel<IdentityUser>.AllFieldNames.Contains(sortField))
            {
                return await this.GetAllAsync();
            }

            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(c => EF.Functions.Like(EF.Property<string>(c, filterField), $"%{filter}%"));
            }

            query = sortOrder switch
            {
                MySortOrder.Ascending => query.OrderBy(c => EF.Property<object>(c, sortField)),
                MySortOrder.Descending => query.OrderByDescending(c => EF.Property<object>(c, sortField)),
                _ => query.OrderBy(c => EF.Property<object>(c, sortField)),
            };

            query = query.Skip(skip).Take(pageSize);

            return await query.ToListAsync();
        }

        public async Task<int> GetProcessedDataCountAsync(string? filter, string? filterField)
        {
            if (filterField == null || !IndexViewModel<Client>.FilterableFieldNames.Contains(filterField))
            {
                return await this.CountAsync();
            }

            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(c => EF.Functions.Like(EF.Property<string>(c, filterField), $"%{filter}%"));
            }

            return await query.CountAsync();
        }

        public async Task<int> CountAsync()
        {
            return await this._context.Users.CountAsync();
        }


        public async Task<IReadOnlyList<string>> GetAllRolesAsync()
        {
            return await this._context.Roles.Select(r => r.Name).ToListAsync();
        }

        public async Task<IReadOnlyList<string>> GetRolesAsync(string? id)
        {
            IdentityUser user = await this.GetByIdAsync(id);

            return (IReadOnlyList<string>)await this._userManager.GetRolesAsync(user);
        }
    }
}