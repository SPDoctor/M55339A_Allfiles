using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class VacancyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}