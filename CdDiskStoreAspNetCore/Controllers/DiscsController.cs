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
    [Authorize]
    public class DiscsController : Controller
    {
        private readonly IDiscRepository _discRepository;

        public DiscsController(IDiscRepository discRepository)
        {
            this._discRepository = discRepository;
        }

        public async Task<IActionResult> Index(string? filter, MySortOrder sortOrder, string? filterFieldName, string? sortField = "Id", int skip = 0)
        {
            var model = new IndexViewModel<Disc>
            {
                Filter = filter,
                FilterFieldName = filterFieldName ?? IndexViewModel<Disc>.FilterableFieldNames[0],
                SortFieldName = sortField,
                SortOrder = sortOrder,
                Skip = skip,
                CountItems = await this._discRepository.GetProcessedDataCountAsync(filter, filterFieldName)
            };

            model.Items = await this._discRepository.GetProcessedDataAsync(model.Filter, model.FilterFieldName, model.SortOrder, model.SortFieldName, model.Skip, model.PageSize);

            return View(model);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || this._discRepository == null)
            {
                return NotFound();
            }

            try
            {
                var model = new DiscsDetailsViewModel
                {
                    Disc = await this._discRepository.GetByIdAsync(id),
                    Type = await this._discRepository.GetTypeAsync(id)
                };

                if (model.Type.Contains("Film"))
                {
                    model.Films = await this._discRepository.GetFilmsAsync(id);
                }

                if(model.Type.Contains("Music"))
                {
                    model.Musics = await this._discRepository.GetMusicsAsync(id);
                }
                return View(model);
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "Administrator")]
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
                var disc = await this._discRepository.GetByIdAsync(id);
                return View(disc);
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create(Guid? id, [Bind("Id,Name,Price")] Disc disc)
        {
            if (!ModelState.IsValid && !ValidateDisc(disc))
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

        private bool ValidateDisc(Disc disc)
        {
            if (!Regex.IsMatch(disc.Price.ToString(), PriceValidation.PricePattern))
            {
                ModelState.AddModelError("Price", "Incorrect price set. Price must be set in range from 0 to 0.00 to 99999999.99");
                return false;
            }

            return true;
        }

        [Authorize(Roles = "Administrator, Manager")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || this._discRepository == null)
            {
                return NotFound();
            }

            try
            {
                var disc = await this._discRepository.GetByIdAsync(id);
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
            if (this._discRepository == null)
            {
                return Problem("Repository class implementing 'IClientRepository' is null.");
            }

            var disc = await this._discRepository.GetByIdAsync(id);

            if (disc != null)
            {
                await this._discRepository.DeleteAsync(disc.Id);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
