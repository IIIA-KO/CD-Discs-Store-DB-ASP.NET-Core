using CdDiskStoreAspNetCore.Data.Repository;
using CdDiskStoreAspNetCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CdDiskStoreAspNetCore.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ChangeDiscountLevelController : Controller
    {
        private readonly IChangeDiscountLevelRepository _discountLevelRepository;

        public ChangeDiscountLevelController(IChangeDiscountLevelRepository discountLevelRepository)
        {
            this._discountLevelRepository = discountLevelRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Execute(Guid idClient, bool increase)
        {
            if (!ModelState.IsValid || this._discountLevelRepository == null)
            {
                return RedirectToAction("Details", "Clients", new { id = idClient });
            }

            var model = new ChangeDiscountLevelViewModel
            {
                IdClient = idClient,
                Increase = increase
            };

            await this._discountLevelRepository.Execute(model);

            TempData["SuccessMessage"] = "Operation executed successfully!";

            return RedirectToAction("Details", "Clients", new { id = model.IdClient });
        }
    }
}
