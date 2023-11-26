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
            if (ModelState.IsValid)
            {
                try
                {
                    var response = await _webAPIExecuter.InvokePost("Shirts", shirt);
                    if (response != null)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (WebApiException ex)
                {
                    HandleWebApiException(ex);
                }
            }
            return View(shirt);
        }

        public async Task<IActionResult> UpdateShirt(int shirtId)
        {
            try
            {
                var shirt = await _webAPIExecuter.InvokeGet<Shirt>($"Shirts/{shirtId}");
                if (shirt != null)
                {
                    return View(shirt);
                }
            }
            catch (WebApiException ex)
            {
                HandleWebApiException(ex);
                return View();
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateShirt(Shirt shirt)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    await _webAPIExecuter.InvokePut($"Shirts/{shirt.ShirtId}", shirt);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (WebApiException ex)
            {
                HandleWebApiException(ex);
               
            }
            return View(shirt);
        }

       /* public async Task<IActionResult> DeleteShirt(int shirtId)
        {
            var shirt = await _webAPIExecuter.InvokeGet<Shirt>($"Shirts/{shirtId}");
            if (shirt != null)
            {
                return View(shirt);
            }
            return NotFound();
        }*/

        public async Task<IActionResult> DeleteShirt(int shirtId)
        {
            try
            {
                await _webAPIExecuter.InvokeDelete($"Shirts/{shirtId}");
                return RedirectToAction(nameof(Index));
            }
            catch (WebApiException ex)
            {
                HandleWebApiException(ex);
                return View(nameof(Index), await _webAPIExecuter.InvokeGet<List<Shirt>>("Shirts"));
            }
        }


        private void HandleWebApiException(WebApiException ex)
        {
            if (ex.ErrorResponse != null && ex.ErrorResponse.Errors != null && ex.ErrorResponse.Errors.Count > 0)
            {
                foreach (var error in ex.ErrorResponse.Errors)
                {
                    ModelState.AddModelError(error.Key, string.Join(";", error.Value));
                }
            }
        }
    }
}
