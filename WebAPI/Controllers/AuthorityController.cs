using Microsoft.AspNetCore.Mvc;
using WebAPI.Authority;

namespace WebAPI.Controllers
{
    [ApiController]
    public class AuthorityController : Controller
    {
        [HttpPost("auth")]
        public IActionResult Authenticate([FromBody] AppCredential credential)
        {
            if(AppRespository.Authenticate(credential.ClientId, credential.Secret))
            {
                return Ok(new
                {
                    access_token = CreateToken(credential.ClientId),
                    expires_at = DateTime.Now.AddMinutes(10)
                });
            }

            ModelState.AddModelError("Unauthorized", "You are not authorized.");
            var problemDetails = new ValidationProblemDetails(ModelState)
            {
                Status = StatusCodes.Status401Unauthorized
            };
           return new UnauthorizedObjectResult(problemDetails);
        }

        private string CreateToken(string clientId)
        {
            return string.Empty;
        }
    }
}
