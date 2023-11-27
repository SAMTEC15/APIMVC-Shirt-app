using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
            if(AppRespository.Authenticate(credential.ClientId, credential.Secret))
            {
                var expiresAt = DateTime.UtcNow.AddMinutes(10);
                return Ok(new
                {
                    access_token = CreateToken(credential.ClientId, expiresAt),
                    expires_at = expiresAt
                });
            }

            ModelState.AddModelError("Unauthorized", "You are not authorized.");
            var problemDetails = new ValidationProblemDetails(ModelState)
            {
                Status = StatusCodes.Status401Unauthorized
            };
           return new UnauthorizedObjectResult(problemDetails);
        }

        private string CreateToken(string clientId, DateTime expiresAt)
        {
            //Algo, Payload, Siging Key
            var app = AppRespository.GetApplicationByClientId(clientId);
            var claims = new List<Claim>
            {
                new Claim("AppName", app?.ApplicationName??string.Empty),
                new Claim("Read", (app?.Scopes??string.Empty).Contains("read")?"true":"false"),
                new Claim("Write", (app?.Scopes??string.Empty).Contains("write")?"true":"false")
            };

            var secretKey = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("Key"));

            var jwt = new JwtSecurityToken(
                signingCredentials: new SigningCredentials(
                   new SymmetricSecurityKey(secretKey),
                SecurityAlgorithms.HmacSha256Signature),
                claims: claims,
                expires: expiresAt,
                notBefore: DateTime.UtcNow
                );
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
