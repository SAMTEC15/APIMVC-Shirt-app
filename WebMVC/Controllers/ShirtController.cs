using Microsoft.AspNetCore.Mvc;
using WebMVC.Data;
using WebMVC.Models;
using WebMVC.Models.Repositories;

namespace WebMVC.Controllers
{
    public class ShirtController : Controller
    {
        private readonly IWebAPIExecuter _webAPIExecuter;

        public ShirtController(IWebAPIExecuter webAPIExecuter)
        {
            _webAPIExecuter = webAPIExecuter;
        }
        public async Task<IActionResult> Index()
        {
            var Inview = await _webAPIExecuter.InvokeGet<List<Shirt>>("Shirts");
            return View(Inview);
        }

        public IActionResult CreateShirt()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateShirt(Shirt shirt)
        {
            if(ModelState.IsValid)
            {
                var response = await _webAPIExecuter.InvokePost("Shirts", shirt);
                if(response != null)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(shirt);
        }

    }
}
