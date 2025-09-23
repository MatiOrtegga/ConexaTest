using ConexaTest.Domain.Dto;
using ConexaTest.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ConexaTest.Application.Utils
{
    public class JwtUtils(IConfiguration configuration)
    {
        protected readonly IConfiguration _configuration = configuration;
        public AuthResponseDto GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.Name),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Role, user.Role.Name)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
            );
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiresIn = Convert.ToInt32(_configuration["Jwt:ExpiresIn"]);
            var expires = DateTime.UtcNow.AddSeconds(expiresIn);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );
            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            return new AuthResponseDto
            {
                User = new UserDto
                {
                    Email = user.Email,
                    Name = user.Name,
                    Role = user.Role.Name
                },
                Tokens = new TokensDto
                {
                    AccessToken = accessToken,
                    ExpiresIn = expiresIn
                }
            };
        }
    }
}
