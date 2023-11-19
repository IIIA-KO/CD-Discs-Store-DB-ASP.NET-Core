using CdDiskStoreAspNetCore.Data.Models;
using CdDiskStoreAspNetCore.Data.Repository;
using CdDiskStoreAspNetCore.Models;
using CdDiskStoreAspNetCore.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace CdDiskStoreAspNetCore.Controllers
{
    public class DiscsController : Controller
    {
        private readonly IDiscRepository _discRepository;

        public DiscsController(IDiscRepository discRepository)
        {
            this._discRepository = discRepository;
        }

        // GET: Discs
        public async Task<IActionResult> Index(string? filter, MySortOrder sortOrder, string? filterFieldName = "Name", string? sortField = "Id", int skip = 0)
        {
            var model = new DiscsIndexViewModel
            {
                Filter = filter,
                FilterFieldName = filterFieldName,
                SortFieldName = sortField,
                SortOrder = sortOrder,
                Skip = skip,
                CountItems = await this._discRepository.GetProcessedDataCountAsync(filter, filterFieldName)
            };

            model.Items = await this._discRepository.GetProcessedDataAsync(model.Filter, model.FilterFieldName, model.SortOrder, model.SortFieldName, model.Skip, model.PageSize);

            return View(model);
        }

        // GET: Discs/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || this._discRepository == null)
            {
                return NotFound();
            }

            Disc disc;
            try
            {
                disc = await this._discRepository.GetByIdAsync(id);
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }

            return View(disc);
        }

        // GET: Discs/Create
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
                var client = await this._discRepository.GetByIdAsync(id);
                return View(client);
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST: Discs/Creates
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid? id, [Bind("Id,Name,Price")] Disc disc)
        {
            if (!ModelState.IsValid)
            {
                return View(disc);
            }

            if (id == null)
            {
                disc.Id = Guid.NewGuid();

                await this._discRepository.AddAsync(disc);

                return RedirectToAction(nameof(Index));
            }

            if (id != disc.Id)
            {
                return NotFound();
            }

            try
            {
                await this._discRepository.UpdateAsync(disc);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await this._discRepository.ExistsAsync(disc.Id))
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

        // GET: Discs/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || this._discRepository == null)
            {
                return NotFound();
            }

            try
            {
                var client = await this._discRepository.GetByIdAsync(id);
                return Ok();
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST: Discs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (this._discRepository == null)
            {
                return Problem("Repository class implementing 'IClientRepository' is null.");
            }

            var client = await this._discRepository.GetByIdAsync(id);

            if (client != null)
            {
                await this._discRepository.DeleteAsync(client.Id);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
