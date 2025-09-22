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
        public string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new ("id", user.Id.ToString()),
                new ("name", user.Name.ToString()),
                new("email", user.Email.ToString()),
                new(ClaimTypes.Role, user.Role.Name)            
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(60);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
