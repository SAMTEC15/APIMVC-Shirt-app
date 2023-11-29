using Microsoft.AspNetCore.Mvc;
using WebAPI.Attributes;
using WebAPI.Filters.Filters;
using WebAPIDemo.Data;
using WebAPIDemo.Filters.ActionFilters;
using WebAPIDemo.Models;

namespace WebAPIDemo.Controllers
{
    //[ApiVersion("1.0")]
    [ApiController]
    [Route("api/[controller]")]
    //[JwtTokenAuthFilter]
    //[JwtTokenAuthFilter]
    public class ShirtsController: ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public ShirtsController(ApplicationDbContext db)
        {
           _db = db;
        }

        [HttpGet]
        //[RequiredClaim("read", "true")]
        public IActionResult GetShirts()
        {
            return Ok(_db.Shirts.ToList());
        }

        [HttpGet("{id}")]
        [TypeFilter(typeof(Shirt_ValidateShirtIdFilterAttribute))]
       // [RequiredClaim("read", "true")]
        public IActionResult GetShirtById(int id)
        {
            return Ok(HttpContext.Items["shirt"]);
        }

        [HttpPost]
        [TypeFilter(typeof(Shirt_ValidateCreateShirtFilterAttribute))]
        [RequiredClaim("write", "true")]
        public IActionResult CreateShirt([FromBody]Shirt shirt)
        {
           _db.Shirts.Add(shirt);
           _db.SaveChanges();

            return CreatedAtAction(nameof(GetShirtById),
                new { id = shirt.ShirtId },
                shirt);
        }

        [HttpPut("{id}")]
        [TypeFilter(typeof(Shirt_ValidateShirtIdFilterAttribute))]
        [Shirt_ValidateUpdateShirtFilter]
        [TypeFilter(typeof(Shirt_ValidateUpdateShirtFilterAttribute))]
        [RequiredClaim("write", "true")]
        public IActionResult UpdateShirt(int id, Shirt shirt)
        {
            var shirtToUpdate = HttpContext.Items["shirt"] as Shirt;
            shirtToUpdate.Brand = shirt.Brand;
            shirtToUpdate.Price = shirt.Price;
            shirtToUpdate.Size = shirt.Size;
            shirtToUpdate.Color = shirt.Color;
            shirtToUpdate.Gender = shirt.Gender;

            _db.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [TypeFilter(typeof(Shirt_ValidateShirtIdFilterAttribute))]
        [RequiredClaim("delete", "true")]
        public IActionResult DeleteShirt(int id)
        {
            var shirtToDelete = HttpContext.Items["shirt"] as Shirt;

            _db.Shirts.Remove(shirtToDelete);
            _db.SaveChanges();

            return Ok(shirtToDelete);
        }
    }
}
