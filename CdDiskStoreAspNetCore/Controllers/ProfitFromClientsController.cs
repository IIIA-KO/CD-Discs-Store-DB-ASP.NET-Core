using CdDiskStoreAspNetCore.Data.Repository;
using CdDiskStoreAspNetCore.Models;
using CdDiskStoreAspNetCore.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CdDiskStoreAspNetCore.Controllers
{
    public class ProfitFromClientsController : Controller
    {
        private readonly IProfitFromClientRepository _profitFromClientRepository;

        public ProfitFromClientsController(IProfitFromClientRepository profitFromClientRepository)
        {
            this._profitFromClientRepository = profitFromClientRepository;
        }

        public async Task<IActionResult> Index(string? filter, MySortOrder sortOrder, string? filterFieldName, string? sortField = "ClientId", int skip = 0)
        {
            var model = new IndexViewModel<ProfitFromClient>
            {
                Filter = filter,
                FilterFieldName = filterFieldName ?? IndexViewModel<ProfitFromClient>.FilterableFieldNames[0],
                SortFieldName = sortField,
                SortOrder = sortOrder,
                Skip = skip,
                PageSize = 20,
                CountItems = await this._profitFromClientRepository.GetProcessedDataCountAsync(filter, filterFieldName)
            };

            model.Items = await this._profitFromClientRepository.GetProcessedDataAsync(model.Filter, model.FilterFieldName, model.SortOrder, model.SortFieldName, model.Skip, model.PageSize);

            return View(model);
        }
    }
}