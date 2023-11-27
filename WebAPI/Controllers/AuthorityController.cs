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
            if(Authenticator.Authenticate(credential.ClientId, credential.Secret))
            {
                var expiresAt = DateTime.UtcNow.AddMinutes(10);
                return Ok(new
                {
                    access_token = Authenticator.CreateToken(credential.ClientId, expiresAt, _configuration.GetValue<string>("Key")),
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

        public static bool VerifyToken(string token, string strKey)
        {
            if(string.IsNullOrEmpty(token))
            {
                return false;
            }
            var secrectKey = Encoding.ASCII.GetBytes(strKey);
            SecurityToken securityToken;
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secrectKey),
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ClockSkew = TimeSpan.Zero
                }, out securityToken);
            }
            catch (SecurityTokenException)
            {
                return false;
            }
            catch
            {
                throw;
            }
            return securityToken != null;
        }
        
    }
}
