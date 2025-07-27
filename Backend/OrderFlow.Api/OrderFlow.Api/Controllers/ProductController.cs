using Microsoft.AspNetCore.Mvc;

namespace OrderFlow.Api.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
