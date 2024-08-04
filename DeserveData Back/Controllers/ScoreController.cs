using Microsoft.AspNetCore.Mvc;

namespace DeserveData_Back.Controllers
{

    public class ScoreController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
