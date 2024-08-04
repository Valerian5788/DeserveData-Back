using Microsoft.AspNetCore.Mvc;

namespace DeserveData_Back.Controllers
{
    public class FeedBackController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
