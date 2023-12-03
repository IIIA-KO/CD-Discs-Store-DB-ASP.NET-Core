using CdDiskStoreAspNetCore.Data.Repository;
using CdDiskStoreAspNetCore.Models.Enums;
using CdDiskStoreAspNetCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CdDiskStoreAspNetCore.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class QuarterIncomesController : Controller
    {
        private readonly IQuarterIncomeRepository _quarterIncomeRepository;

        public QuarterIncomesController(IQuarterIncomeRepository quarterIncomeRepository)
        {
            this._quarterIncomeRepository = quarterIncomeRepository;
        }

        public async Task<IActionResult> Index(MySortOrder sortOrder, string? sortField = "Id", int skip = 0)
        {
            var model = new IndexViewModel<QuarterIncome>
            {
                SortFieldName = sortField,
                SortOrder = sortOrder,
                Skip = skip,
                PageSize = 20,
                CountItems = await this._quarterIncomeRepository.CountAsync()
            };

            model.Items = await this._quarterIncomeRepository.GetProcessedDataAsync(model.SortOrder, model.SortFieldName, model.Skip, model.PageSize);

            return View(model);
        }
    }
}
