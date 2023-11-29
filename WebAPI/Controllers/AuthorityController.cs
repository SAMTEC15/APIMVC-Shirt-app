using Microsoft.AspNetCore.Mvc;
using WebAPI.Authority;

namespace WebAPI.Controllers
{
    [ApiController]
    public class AuthorityController : Controller
    {
        private readonly IConfiguration _configuration;

        public AuthorityController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost("auth")]
        public IActionResult Authenticate([FromBody] AppCredential credential)
        {
            if(Authenticator.Authenticate(credential.ClientId, credential.Secret))
            {
                var expiresAt = DateTime.UtcNow.AddMinutes(10);
                return Ok(new
                {
                    access_token = Authenticator.CreateToken(credential.ClientId, expiresAt, _configuration.GetValue<string>("Key")),
                    expires_at = expiresAt
                });
            }
            else
            {
                ModelState.AddModelError("Unauthorized", "You are not authorized.");
                var problemDetails = new ValidationProblemDetails(ModelState)
                {
                    Status = StatusCodes.Status401Unauthorized
                };
                return new UnauthorizedObjectResult(problemDetails);
            }
           
        }

        
        
    }
}
