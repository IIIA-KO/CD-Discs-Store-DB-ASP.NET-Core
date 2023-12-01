using CdDiskStoreAspNetCore.Data.Repository;
using CdDiskStoreAspNetCore.Models.Enums;
using CdDiskStoreAspNetCore.Models;
using Microsoft.AspNetCore.Mvc;

namespace CdDiskStoreAspNetCore.Controllers
{
    public class AdultFilmRatiosController : Controller
    {
        private readonly IAdultFilmRatioRepository _adultFilmRatioRepository;

        public AdultFilmRatiosController(IAdultFilmRatioRepository adultFilmRatioRepository)
        {
            this._adultFilmRatioRepository = adultFilmRatioRepository;
        }

        public async Task<IActionResult> Index(string? filter, MySortOrder sortOrder, string? filterFieldName, string? sortField = "OrderYear", int skip = 0)
        {
            var model = new IndexViewModel<AdultFilmRatio>
            {
                Filter = filter,
                FilterFieldName = filterFieldName ?? IndexViewModel<AdultFilmRatio>.FilterableFieldNames[0],
                SortFieldName = sortField,
                SortOrder = sortOrder,
                Skip = skip,
                PageSize = 20,
                CountItems = await this._adultFilmRatioRepository.GetProcessedDataCountAsync(filter, filterFieldName)
            };

            model.Items = await this._adultFilmRatioRepository.GetProcessedDataAsync(model.Filter, model.FilterFieldName, model.SortOrder, model.SortFieldName, model.Skip, model.PageSize);

            return View(model);
        }
    }
}
