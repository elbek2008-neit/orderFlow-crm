using Microsoft.AspNetCore.Mvc;

namespace OrderFlow.Api.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
