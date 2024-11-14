using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CryptoAnalyzer.WebApi.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace CryptoAnalyzer.WebApi.JwtHelper
{
    public class JwtHelper
    {

        public static string GenerateJwtToken(JwtDto jwtInfo)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtInfo.SecretKey));

            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtClaimNames.Id, jwtInfo.Id.ToString()),
                new Claim(JwtClaimNames.FirstName, jwtInfo.FirstName.ToString()),
                new Claim(JwtClaimNames.LastName, jwtInfo.LastName.ToString()),
                new Claim(JwtClaimNames.Email, jwtInfo.Email.ToString()),
                new Claim(JwtClaimNames.UserType, jwtInfo.UserType.ToString()),
                new Claim(ClaimTypes.NameIdentifier, jwtInfo.Id.ToString()),

                new Claim(ClaimTypes.Role, jwtInfo.UserType.ToString())
            };

            var expireTime = DateTime.Now.AddMinutes(jwtInfo.ExpireMinutes);

            var tokenDescriptor = new JwtSecurityToken(jwtInfo.Issuer, jwtInfo.Audience, claims, null, expireTime, credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

            return token;


        }
    }
}

