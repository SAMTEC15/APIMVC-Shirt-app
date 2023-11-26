using Microsoft.AspNetCore.Mvc;
using WebMVC.Models.Repositories;

namespace WebMVC.Controllers
{
    public class ShirtController : Controller
    {
        public IActionResult Index()
        {
            return View(ShirtRepository.GetShirts());
        }

    }
}
