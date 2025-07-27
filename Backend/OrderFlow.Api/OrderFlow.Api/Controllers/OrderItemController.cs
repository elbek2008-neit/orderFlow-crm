using Microsoft.AspNetCore.Mvc;

namespace OrderFlow.Api.Controllers
{
    public class OrderItemController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
