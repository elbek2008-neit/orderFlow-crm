using Microsoft.AspNetCore.Mvc;

namespace OrderFlow.Api.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
