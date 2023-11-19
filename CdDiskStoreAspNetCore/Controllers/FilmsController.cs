using CdDiskStoreAspNetCore.Data.Models;
using CdDiskStoreAspNetCore.Data.Repository;
using CdDiskStoreAspNetCore.Models;
using CdDiskStoreAspNetCore.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CdDiskStoreAspNetCore.Controllers
{
    public class FilmsController : Controller
    {
        private readonly IFilmRepository _filmRepository;

        public FilmsController(IFilmRepository filmRepository)
        {
            this._filmRepository = filmRepository;
        }

        // GET: Films
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

        // GET: Films/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || this._filmRepository == null)
            {
                return NotFound();
            }

            Film film;
            try
            {
                film = await this._filmRepository.GetByIdAsync(id);
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }

            return View(film);
        }

        // GET: Films/Create
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

        // POST: Films/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid? id, [Bind("Id,Name,Genre,Producer,MainRole,AgeLimit")] Film film)
        {
            if (!ModelState.IsValid)
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

        // GET: Films/Delete/5
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

        // POST: Films/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
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
