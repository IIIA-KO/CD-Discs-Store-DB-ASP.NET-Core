using CdDiskStoreAspNetCore.Data.Models;
using CdDiskStoreAspNetCore.Data.Repository;
using CdDiskStoreAspNetCore.Models;
using CdDiskStoreAspNetCore.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CdDiskStoreAspNetCore.Controllers
{
    public class MusicsController : Controller
    {
        private readonly IMusicRepository _musicRepository;

        public MusicsController(IMusicRepository musicRepository)
        {
            this._musicRepository = musicRepository;
        }

        // GET: Musics
        public async Task<IActionResult> Index(string? filter, MySortOrder sortOrder, string? filterFieldName, string? sortField = "Id", int skip = 0)
        {
            var model = new IndexViewModel<Music>
            {
                Filter = filter,
                FilterFieldName = filterFieldName ?? IndexViewModel<Music>.FilterableFieldNames[0],
                SortFieldName = sortField,
                SortOrder = sortOrder,
                Skip = skip,
                CountItems = await this._musicRepository.GetProcessedDataCountAsync(filter, filterFieldName)
            };

            model.Items = await this._musicRepository.GetProcessedDataAsync(model.Filter, model.FilterFieldName, model.SortOrder, model.SortFieldName, model.Skip, model.PageSize);

            return View(model);
        }

        // GET: Musics/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || this._musicRepository == null)
            {
                return NotFound();
            }

            Music music;
            try
            {
                music = await this._musicRepository.GetByIdAsync(id);
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }

            return View(music);
        }

        // GET: Musics/Create
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
                var film = await this._musicRepository.GetByIdAsync(id);
                return View(film);
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST: Musics/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid? id, [Bind("Id,Name,Genre,Artist,Language")] Music music)
        {
            if (!ModelState.IsValid)
            {
                return View(music);
            }

            if (id == null)
            {
                music.Id = Guid.NewGuid();

                await this._musicRepository.AddAsync(music);

                return RedirectToAction(nameof(Index));
            }

            if (id != music.Id)
            {
                return NotFound();
            }

            try
            {
                await this._musicRepository.UpdateAsync(music);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await this._musicRepository.ExistsAsync(music.Id))
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

        // GET: Musics/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || this._musicRepository == null)
            {
                return NotFound();
            }

            try
            {
                var music = await this._musicRepository.GetByIdAsync(id);
                return Ok();
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST: Musics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (this._musicRepository == null)
            {
                return Problem("Repository class implementing 'IClientRepository' is null.");
            }

            var music = await this._musicRepository.GetByIdAsync(id);

            if (music != null)
            {
                await this._musicRepository.DeleteAsync(music.Id);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
