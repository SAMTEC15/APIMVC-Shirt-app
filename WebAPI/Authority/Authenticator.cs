using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace WebAPI.Authority
{
    public static class Authenticator
    {
        public static bool Authenticate(string clientId, string secret)
        {
            var app = AppRespository.GetApplicationByClientId(clientId);
            if (app == null) 
                return false;
            return (app.ClientId == clientId && app.Secret == secret);
        }

        public static string CreateToken(string clientId, DateTime expiresAt, string strKey)
        {
            //Algo, Payload, Siging Key
            var app = AppRespository.GetApplicationByClientId(clientId);
            var claims = new List<Claim>
            {
                new Claim("AppName", app?.ApplicationName??string.Empty),
                new Claim("Read", (app?.Scopes??string.Empty).Contains("read")?"true":"false"),
                new Claim("Write", (app?.Scopes??string.Empty).Contains("write")?"true":"false")
            };

            var secretKey = Encoding.ASCII.GetBytes(strKey);

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
