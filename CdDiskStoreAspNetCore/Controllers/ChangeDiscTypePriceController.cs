using CdDiskStoreAspNetCore.Data.Repository;
using CdDiskStoreAspNetCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CdDiskStoreAspNetCore.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ChangeDiscTypePriceController : Controller
    {
        private readonly IChangeDiscTypePriceRepository _changePriceRepository;

        public ChangeDiscTypePriceController(IChangeDiscTypePriceRepository changePriceRepository)
        {
            this._changePriceRepository = changePriceRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Execute(ChangeDiscTypePriceViewModel model)
        {
            if (!ModelState.IsValid || this._changePriceRepository == null)
            {
                return View(ModelState);
            }

            await this._changePriceRepository.Execute(model);
            
            TempData["SuccessMessage"] = "Operation executed successfully!";

            return RedirectToAction("Index");
        }
    }
}