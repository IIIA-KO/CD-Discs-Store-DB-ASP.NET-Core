using CdDiskStoreAspNetCore.Data.Models;
using CdDiskStoreAspNetCore.Data.Repository;
using CdDiskStoreAspNetCore.Models;
using CdDiskStoreAspNetCore.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CdDiskStoreAspNetCore.Controllers
{
    [Authorize]
    public class FilmsController : Controller
    {
        private readonly IFilmRepository _filmRepository;

        public FilmsController(IFilmRepository filmRepository)
        {
            this._filmRepository = filmRepository;
        }

        public async Task<IActionResult> Index(string? filter, MySortOrder sortOrder, string? filterFieldName, string? sortField = "Id", int skip = 0)
        {
            var model = new IndexViewModel<Film>
            {
                Filter = filter,
                FilterFieldName = filterFieldName ?? IndexViewModel<Film>.FilterableFieldNames[0],
                SortFieldName = sortField,
                SortOrder = sortOrder,
                Skip = skip,
                CountItems = await this._filmRepository.GetProcessedDataCountAsync(filter, filterFieldName)
            };

            model.Items = await this._filmRepository.GetProcessedDataAsync(model.Filter, model.FilterFieldName, model.SortOrder, model.SortFieldName, model.Skip, model.PageSize);

            return View(model);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || this._filmRepository == null)
            {
                return NotFound();
            }

            try
            {
                var model = new FilmsDetailsViewModel
                {
                    Film = await this._filmRepository.GetByIdAsync(id),
                    Discs = await this._filmRepository.GetDiscsAsync(id)
                };
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
                var film = await this._filmRepository.GetByIdAsync(id);
                return View(film);
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create(Guid? id, [Bind("Id,Name,Genre,Producer,MainRole,AgeLimit")] Film film)
        {
            if (!ModelState.IsValid && !ValidateFilm(film))
            {
                return View(film);
            }

            if (id == null)
            {
                film.Id = Guid.NewGuid();

                await this._filmRepository.AddAsync(film);

                return RedirectToAction(nameof(Index));
            }

            if (id != film.Id)
            {
                return NotFound();
            }

            try
            {
                await this._filmRepository.UpdateAsync(film);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await this._filmRepository.ExistsAsync(film.Id))
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

        private bool ValidateFilm(Film film)
        {
            if (film.AgeLimit < 0 || film.AgeLimit > 18)
            {
                ModelState.AddModelError("Age limit", "Incorrect age limit set. Age limit must be set in range from 0 to 18");
                return false;
            }

            return true;
        }

        [Authorize(Roles = "Administrator, Manager")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || this._filmRepository == null)
            {
                return NotFound();
            }

            try
            {
                var film = await this._filmRepository.GetByIdAsync(id);
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
            if (this._filmRepository == null)
            {
                return Problem("Repository class implementing 'IClientRepository' is null.");
            }

            var film = await this._filmRepository.GetByIdAsync(id);

            if (film != null)
            {
                await this._filmRepository.DeleteAsync(film.Id);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
