using Microsoft.AspNetCore.Mvc;

namespace CdDiskStoreAspNetCore.Controllers
{
    public class AdminPanelController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
