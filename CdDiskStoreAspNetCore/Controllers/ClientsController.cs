using CdDiskStoreAspNetCore.Data.Models;
using CdDiskStoreAspNetCore.Data.Repository;
using CdDiskStoreAspNetCore.Models;
using CdDiskStoreAspNetCore.Models.Enums;
using CdDiskStoreAspNetCore.Utilities.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace CdDiskStoreAspNetCore.Controllers
{
    [Authorize(Roles = "Administrator, Manager, User")]
    public class ClientsController : Controller
    {
        private readonly IClientRepository _clientRepository;

        public ClientsController(IClientRepository clientRepository)
        {
            this._clientRepository = clientRepository;
        }

        public async Task<IActionResult> Index(string? filter, MySortOrder sortOrder, string? filterFieldName, string? sortField = "Id", int skip = 0)
        {
            var model = new IndexViewModel<Client>
            {
                Filter = filter,
                FilterFieldName = filterFieldName ?? IndexViewModel<Client>.FilterableFieldNames[0],
                SortFieldName = sortField,
                SortOrder = sortOrder,
                Skip = skip,
                CountItems = await this._clientRepository.GetProcessedDataCountAsync(filter, filterFieldName)
            };

            model.Items = await this._clientRepository.GetProcessedDataAsync(model.Filter, model.FilterFieldName, model.SortOrder, model.SortFieldName, model.Skip, model.PageSize);

            return View(model);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || this._clientRepository == null)
            {
                return NotFound();
            }

            try
            {
                var model = new ClientDetailsViewModel()
                {
                    Client = await this._clientRepository.GetByIdAsync(id),
                    PersonalDiscount = await this._clientRepository.GetPersonalDiscountAsync(id),
                    RentProfit = await this._clientRepository.GetTotalRentProfitAsync(id),
                    PurchaseProfit = await this._clientRepository.GetTotalPurchaseProfitAsync(id),
                    TotalProfit = await this._clientRepository.GetTotalProfitAsync(id),
                    ContactPhones = await this._clientRepository.GetPhonesAsync(id),
                    ContactMails = await this._clientRepository.GetMailsAsync(id)
                };

                return View(model);
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "Administrator, Manager")]
        public async Task<IActionResult> Create(Guid? id)
        {
            if (id == null)
            {
                ViewData["Action"] = "Create";
                return View();
            }

            ViewData["Action"] = "Edit";
            try
            {
                var client = await this._clientRepository.GetByIdAsync(id);
                return View(client);
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Manager")]
        public async Task<IActionResult> Create(Guid? id, [Bind("Id,FirstName,LastName,Address,City,ContactPhone,ContactMail,BirthDay,MarriedStatus,Sex,HasChild")] Client client)
        {
            if (!ModelState.IsValid && !ValidateContactDetails(client))
            {
                return View(client);
            }

            if (id == null)
            {
                client.Id = Guid.NewGuid();

                await this._clientRepository.AddAsync(client);

                return RedirectToAction(nameof(Index));
            }

            if (id != client.Id)
            {
                return NotFound();
            }

            try
            {
                await this._clientRepository.UpdateAsync(client);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await this._clientRepository.ExistsAsync(client.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ValidateContactDetails(Client client)
        {
            if (!Regex.IsMatch(client.ContactPhone, PhoneValidation.PhonePattern))
            {
                ModelState.AddModelError("Contact Phone", "Contact phone does not match the pattern: 'xx-xxx-xx-xx'");
                return false;
            }

            if (!Regex.IsMatch(client.ContactMail, EmailAddressValidation.EmailPattern))
            {
                ModelState.AddModelError("Contact Mail", "Contact mail does not match the pattern: 'user@example.com'");
                return false;
            }

            return true;
        }

        [Authorize(Roles = "Administrator, Manager")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || this._clientRepository == null)
            {
                return NotFound();
            }

            try
            {
                var client = await this._clientRepository.GetByIdAsync(id);
                return Ok();
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Manager")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (this._clientRepository == null)
            {
                return Problem("Repository class implementing 'IClientRepository' is null.");
            }

            var client = await this._clientRepository.GetByIdAsync(id);

            if (client != null)
            {
                await this._clientRepository.DeleteAsync(client.Id);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}