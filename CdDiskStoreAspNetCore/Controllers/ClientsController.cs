using CdDiskStoreAspNetCore.Data.Models;
using CdDiskStoreAspNetCore.Data.Repository;
using CdDiskStoreAspNetCore.Utilities.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace CdDiskStoreAspNetCore.Controllers
{
    public class ClientsController : Controller
    {
        private readonly IClientRepository _clientRepository;

        public ClientsController(IClientRepository clientRepository)
        {
            this._clientRepository = clientRepository;
        }

        // GET: Clients
        public async Task<IActionResult> Index()
        {
            return View(await this._clientRepository.GetAllAsync());
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || this._clientRepository == null)
            {
                ViewData["Action"] = "Create";
                return NotFound();
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

        // GET: Clients/Create
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

        // POST: Clients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
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

            Client currentClient;
            try
            {
                currentClient = await this._clientRepository.GetByIdAsync(id);
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }

            if (currentClient != null && !IsClientChanged(currentClient, client))
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                int result = await this._clientRepository.UpdateAsync(client);

                if (result == 0)
                {
                    throw new DbUpdateException("Client edit failed");
                }
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
            catch (DbUpdateException ex)
            {
                return Problem(ex.Message);
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

        private bool IsClientChanged(Client currentClient, Client client)
        {
            return currentClient.FirstName != client.FirstName
                || currentClient.LastName != client.LastName
                || currentClient.Address != client.Address
                || currentClient.City != client.City
                || currentClient.ContactPhone != client.ContactPhone
                || currentClient.ContactMail != client.ContactMail
                || currentClient.BirthDay != client.BirthDay
                || currentClient.MarriedStatus != client.MarriedStatus
                || currentClient.Sex != client.Sex
                || currentClient.HasChild != client.HasChild;
        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || this._clientRepository == null)
            {
                return NotFound();
            }

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

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
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