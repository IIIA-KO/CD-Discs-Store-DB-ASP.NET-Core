using CdDiskStoreAspNetCore.Data.Repository;
using CdDiskStoreAspNetCore.Models.Enums;
using CdDiskStoreAspNetCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CdDiskStoreAspNetCore.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UselessDiscsController : Controller
    {
        private readonly IUselessDiscsRepository _uselessDiscsRepository;

        public UselessDiscsController(IUselessDiscsRepository uselessDiscsRepository)
        {
            this._uselessDiscsRepository = uselessDiscsRepository;
        }

        public async Task<IActionResult> Index(string? filter, MySortOrder sortOrder, string? filterFieldName, string? sortField = "Id", int skip = 0)
        {
            var model = new IndexViewModel<UselessDisc>
            {
                Filter = filter,
                FilterFieldName = filterFieldName ?? IndexViewModel<UselessDisc>.FilterableFieldNames[0],
                SortFieldName = sortField,
                SortOrder = sortOrder,
                Skip = skip,
                CountItems = await this._uselessDiscsRepository.GetProcessedDataCountAsync(filter, filterFieldName)
            };

            model.Items = await this._uselessDiscsRepository.GetProcessedDataAsync(model.Filter, model.FilterFieldName, model.SortOrder, model.SortFieldName, model.Skip, model.PageSize);

            return View(model);
        }
    }
}