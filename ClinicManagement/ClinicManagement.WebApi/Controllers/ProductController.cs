using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.WebApi.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
