using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using BackendTask.Shared.Exceptions;
using Microsoft.Extensions.Configuration;

namespace BackendTask.Domain.Models.UserAccount
{
    public class JWTGenerator
    {
        public static TokenDomain GenerateJWTToken(CreateTokenRequest userInfo, IConfiguration appSettings)
        {
            var key = appSettings["Jwt:Key"];

            if (string.IsNullOrEmpty(key) || key.Length < 32)
            {
                throw new BusinessException("The Jwt:Key must be at least 32 characters long.");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, userInfo.UserId.ToString()),
                new(ClaimTypes.Email, userInfo.Email),
                new(JwtRegisteredClaimNames.Sub, userInfo.Email),
                new("IsActive"  , userInfo.IsActive.ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: appSettings["Jwt:Issuer"],
                audience: appSettings["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: credentials
            );
            string tok = new JwtSecurityTokenHandler().WriteToken(token);

            return new TokenDomain
            {
                AccessToken = tok,
                ExpiresIn = 86400
            };
        }
    }
}
