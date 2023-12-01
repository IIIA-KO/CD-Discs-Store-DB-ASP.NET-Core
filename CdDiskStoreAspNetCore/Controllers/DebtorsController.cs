using CdDiskStoreAspNetCore.Data.Repository;
using CdDiskStoreAspNetCore.Models;
using CdDiskStoreAspNetCore.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CdDiskStoreAspNetCore.Controllers
{
    [Authorize(Roles = "Administrator, Manager")]
    public class DebtorsController : Controller
    {
        private readonly IDebtorsRepository _debtorsRepository;

        public DebtorsController(IDebtorsRepository debtorsRepository)
        {
            this._debtorsRepository = debtorsRepository;
        }

        public async Task<IActionResult> Index(string? filter, MySortOrder sortOrder, string? filterFieldName, string? sortField = "Id", int skip = 0)
        {
            var model = new IndexViewModel<Debtor>
            {
                Filter = filter,
                FilterFieldName = filterFieldName ?? IndexViewModel<Debtor>.FilterableFieldNames[0],
                SortFieldName = sortField,
                SortOrder = sortOrder,
                Skip = skip,
                PageSize = 20,
                CountItems = await this._debtorsRepository.GetProcessedDataCountAsync(filter, filterFieldName)
            };

            model.Items = await this._debtorsRepository.GetProcessedDataAsync(model.Filter, model.FilterFieldName, model.SortOrder, model.SortFieldName, model.Skip, model.PageSize);

            return View(model);
        }
    }
}