using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConexaTest.Domain.Dto
{
    public class AuthResponseDto
    {
        public UserDto User { get; set; } = null!;
        public TokensDto Tokens { get; set; } = null!;
    }

    public class UserDto
    {
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }

    public class TokensDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public int ExpiresIn { get; set; }
    }

}
